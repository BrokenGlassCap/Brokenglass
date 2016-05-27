using BrokenGlassDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BrokenGlassWebApp.Controllers.api
{
    public class AccountController : ApiController
    {
        public async Task<HttpResponseMessage> Registration(UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("error", "Model is invalid.");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            try
            {
                await UserFactory.GetUser().RegistrationAsync(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("error", ex.Message);
                ApplicationLogger.Instance.Error($"{ex.Message} {ex.StackTrace}");
                throw new HttpResponseException(HttpStatusCode.Conflict);
            }

            return new HttpResponseMessage(HttpStatusCode.Created);
            
        } 
    }
}
