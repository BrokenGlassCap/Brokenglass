﻿using BrokenGlassDomain.DataLayer;
using BrokenGlassDomain.Entities;
using BrokenGlassDomain.ServiceUtils;
using BrokenGlassWebApp.Infostracture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BrokenGlassWebApp.Controllers.api
{
    [AllowAnonymous]
    public class AccountController : ApiController
    {
        [HttpPost]
        [Route("Registration", Name = "RegistrationRoute")]
        public async Task<HttpResponseMessage> Registration(UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", "Model is invalid.");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            try
            {
                using (var userContext = UserFactory.GetUser())
                {
                    var identityUser = await userContext.RegistrationAsync(model);
                    await SendConfirmationMessageToUser(userContext, identityUser);
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.Instance.Error($"{ex.Message} {ex.StackTrace}");
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, ex);
            }
            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        private async Task SendConfirmationMessageToUser(UserFactory userContext, Microsoft.AspNet.Identity.EntityFramework.IdentityUser identityUser)
        {
            string clientTextMessage = await ReturnConfirmationMessageFromMetaData();

            var emailService = new EmailService();
            var token = await userContext.GenerateEmailConfirmationTokenAsync(identityUser.Id);
            var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = identityUser.Id, token = token }));
            var msgBody = $"{clientTextMessage} <a href=\"{callbackUrl}\">Перейти</a>";

            await emailService.SendMessageAsync(identityUser.UserName, "Confirm Account", msgBody);
        }

        private static async Task<string> ReturnConfirmationMessageFromMetaData()
        {
            string clientTextMessage = null;
            using (var db = NinjectService.Instance.GetService<IUnitOfWork>())
            {
                try
                {
                    var dataCleintMessage = await db.MetaDataDictionaryRepository.FindAsync(f => f.Code == "CONFIRM_MESSAGE_TEXT");
                    if (dataCleintMessage == null) throw new Exception("Setting CONFIRM_MESSAGE_TEXT is not exist in DataBase.");

                    clientTextMessage = dataCleintMessage.Value;
                }
                catch (Exception ex)
                {
                    ApplicationLogger.Instance.Error($"{ex.Message} {ex.StackTrace}");
                    clientTextMessage = "Please, confirm your Account email on this link";
                }
            }

            return clientTextMessage;
        }

        [Route("ConfirmEmail",Name = "ConfirmEmailRoute")]
        [HttpGet]
        public async Task<IHttpActionResult> ConfirmEmail(string userId, string token)
        {
            try
            {
                using (var user = UserFactory.GetUser())
                {
                    var identityResult = await user.ConfirmEmailAsync(userId, token);
                    if (!identityResult.Succeeded)
                    {
                        if(identityResult.Errors.Contains("Invalid token."))
                        {
                            await ResendUserConfiramtionMessage(userId, user);
                            return Redirect(new Uri(Url.Content("~/ClientInfo/ExpireToken")));
                        }

                        var errors = user.CreateStringErrorByIdentityResultErrors(identityResult);
                        ApplicationLogger.Instance.Error($"В момент подтверждения аккаунта  c ID {userId} произошли ошибки на серер авторизации Web Api: {errors}");
                        
                        return BadRequest(errors);
                    }
                    return Redirect(new Uri(Url.Content("~/ClientInfo/SuccesConfirmationAccount")));
                }
            }
            catch (Exception ex)
            {
                ApplicationLogger.Instance.Error($"В момент подтверждения аккаунта  c ID {userId} на сервере возникло исключение: {ex.Message} {ex.StackTrace}");
                return BadRequest(ex.Message);
            }
        }


        private async Task ResendUserConfiramtionMessage(string userId, UserFactory user)
        {
            var userIdentity = await user.FindIdentityUserByIdAsync(userId);
            if (userIdentity == null)
            {
                throw new Exception("Попытка подвтеждения незарегестрированного аккаунта");
            }
            else if (userIdentity.EmailConfirmed)
            {
                throw new Exception($"Попытка подвтеждения уже подвержденного аккаунта {userIdentity.UserName}");
            }

            await SendConfirmationMessageToUser(user, userIdentity);
        }

    }
}
