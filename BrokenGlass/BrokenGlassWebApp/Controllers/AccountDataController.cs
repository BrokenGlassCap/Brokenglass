using BrokenGlassDomain.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BrokenGlassWebApp.Controllers
{
    public class AccountDataController : Controller
    {
        // GET: AccountData
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserLoginModel model)
        {
            var userFactory = UserFactory.GetUser();

            if (!ModelState.IsValid || !await userFactory.CheckUserAccess(model))
            {
                ModelState.AddModelError("Summary", "Неверный логин или пароль!");
                return View();
            }

            List<Claim> listClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(ClaimTypes.Email, model.Email)
            };

            try
            {
                var listRoles = from r in await userFactory.GetUsersRoles(model.Email)
                                select new Claim(ClaimTypes.Role, r);
                listClaims.AddRange(listRoles);

            }
            catch (Exception ex)
            {
                ApplicationLogger.Instance.Error($"Users Data has validated but {ex.Message}. Stack: {ex.StackTrace}");
                throw ex;
            }
          
            var identityClaims = new ClaimsIdentity(listClaims, DefaultAuthenticationTypes.ApplicationCookie);
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignIn(new Microsoft.Owin.Security.AuthenticationProperties() { IsPersistent = false },identityClaims);
            
            return RedirectToAction("Index","Home");
        }

        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
    }
}