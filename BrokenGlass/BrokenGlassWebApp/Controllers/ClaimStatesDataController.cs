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
    public class ClaimStatesDataController : Controller
    {
        private BROKEN_GLASSEntities db = new BROKEN_GLASSEntities();

        // GET: ClaimStatesData
        public ActionResult Index()
        {
            return View(db.ClaimState.ToList());
        }

        // GET: ClaimStatesData/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaimState claimState = db.ClaimState.Find(id);
            if (claimState == null)
            {
                return HttpNotFound();
            }
            return View(claimState);
        }

        // GET: ClaimStatesData/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClaimStatesData/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,Name,UpdateAt,UpdateBy")] ClaimState claimState)
        {
            if (ModelState.IsValid)
            {
                claimState.UpdateAt = DateTime.Now;
                db.ClaimState.Add(claimState);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(claimState);
        }

        // GET: ClaimStatesData/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaimState claimState = db.ClaimState.Find(id);
            if (claimState == null)
            {
                return HttpNotFound();
            }
            return View(claimState);
        }

        // POST: ClaimStatesData/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,Name,UpdateAt,UpdateBy")] ClaimState claimState)
        {
            if (ModelState.IsValid)
            {
                claimState.UpdateAt = DateTime.Now;
                db.Entry(claimState).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(claimState);
        }

        // GET: ClaimStatesData/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaimState claimState = db.ClaimState.Find(id);
            if (claimState == null)
            {
                return HttpNotFound();
            }
            return View(claimState);
        }

        // POST: ClaimStatesData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClaimState claimState = db.ClaimState.Find(id);
            db.ClaimState.Remove(claimState);
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
