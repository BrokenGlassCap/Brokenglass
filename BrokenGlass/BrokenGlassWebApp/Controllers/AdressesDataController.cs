using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BrokenGlassDomain;

namespace BrokenGlassWebApp.Controllers
{
    public class AdressesDataController : Controller
    {
        private BROKEN_GLASSEntities db = new BROKEN_GLASSEntities();

        // GET: AdressesData
        public ActionResult Index()
        {
            return View(db.Adress.ToList());
        }

        // GET: AdressesData/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adress adress = db.Adress.Find(id);
            if (adress == null)
            {
                return HttpNotFound();
            }
            return View(adress);
        }

        // GET: AdressesData/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdressesData/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OsbCode,OsbName,City,AdressName,Location,Latitude,Longitude,UpdateAt,UpdateBy")] Adress adress)
        {
            if (ModelState.IsValid)
            {
                adress.UpdateAt = DateTime.UtcNow;
                db.Adress.Add(adress);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adress);
        }

        // GET: AdressesData/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adress adress = db.Adress.Find(id);
            if (adress == null)
            {
                return HttpNotFound();
            }
            return View(adress);
        }

        // POST: AdressesData/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OsbCode,OsbName,City,AdressName,Location,Latitude,Longitude,UpdateAt,UpdateBy")] Adress adress)
        {
            if (ModelState.IsValid)
            {
                adress.UpdateAt = DateTime.UtcNow;
                db.Entry(adress).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(adress);
        }

        // GET: AdressesData/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adress adress = db.Adress.Find(id);
            if (adress == null)
            {
                return HttpNotFound();
            }
            return View(adress);
        }

        // POST: AdressesData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Adress adress = db.Adress.Find(id);
            db.Adress.Remove(adress);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
