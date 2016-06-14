using BrokenGlassDomain;
using BrokenGlassDomain.DataLayer;
using BrokenGlassDomain.ServiceUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrokenGlassWebApp.Controllers
{
    public class TestController : Controller
    {
        private IUnitOfWork db;

        public TestController()
        {
            db = NinjectService.Instance.GetService<IUnitOfWork>();
        }

        // GET: Test
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UploadClaimFile(int claimId)
        {

            return View(claimId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadClaimFile(int claimId, HttpPostedFileBase fileContent)
        {
            var claim = db.ClaimRepository.GetById(claimId);
            if (claim == null)
            {
                return new HttpNotFoundResult();
            }

            var photo = new Photo()
            {
                ClaimId = claimId,
                UpdateAt = DateTime.UtcNow,
            };
            using (MemoryStream stream = new MemoryStream())
            {
                fileContent.InputStream.CopyTo(stream);
                photo.FullPic = stream.ToArray();
            }

            db.PhotoRepository.Insert(photo);
            db.Save();
            ViewBag.UploadState = "Файл добавлен";
            return View(claimId);
        }
    }
}