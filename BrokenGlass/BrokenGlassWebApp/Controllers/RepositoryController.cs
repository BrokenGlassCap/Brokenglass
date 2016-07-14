using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrokenGlassWebApp.Controllers
{
    public class RepositoryController : Controller
    {

        // GET: Repository
        public ActionResult Index()
        {
            return View();
        }
        //@Deprecate
        /*public FileResult getIOSPackage()
        {
            string fileName = "ios-broken-glass.ipa";
            string path = getPathPackage(fileName);
            byte [] array = System.IO.File.ReadAllBytes(path);
            return File(array, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public FileResult getIOSPackageName(string fileName)
        {
            string path = getPathPackage(fileName);
            byte[] array = System.IO.File.ReadAllBytes(path);
            return File(array, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        private string getPathPackage(string namePackage)
        {
            return Path.Combine(HttpContext.Server.MapPath("~/Repository"), namePackage);
        }*/
    }
}