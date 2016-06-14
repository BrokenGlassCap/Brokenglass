using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrokenGlassWebApp.Controllers
{
    public class ClientInfoController : Controller
    {
        // GET: ClientInfo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ExpireToken()
        {
            return View();
        }
        public ActionResult SuccesConfirmationAccount()
        {
            return View();
        }

    }
}