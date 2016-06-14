using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrokenGlassWebApp.Controllers
{
    [Authorize(Roles = "ADMIN_BG")]
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }
    }
}