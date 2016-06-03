using BrokenGlassDomain;
using BrokenGlassDomain.DataLayer;
using BrokenGlassWebApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrokenGlassWebApp.Controllers
{
    public class ClaimDataController : BaseController
    {
        private IUnitOfWork db;

        public ClaimDataController(IUnitOfWork instanceDb)
        {
            db = instanceDb;
        }
        public ActionResult Index()
        {
            var model = db.ClaimRepository.GetAll().OrderByDescending(d => d.CreateAt);
            List<object> services = new List<object>(){
                new { Id = 0, StateName = string.Empty  }
            };
            List<object> states = new List<object>(){
                new { Id = 0, StateName = string.Empty  }
            };
            states.AddRange(from s in db.ClaimStateRepository.GetAll()
                            select new { Id = s.Id, StateName = s.Name });

            services.AddRange(from srv in db.ServiceRepository.GetAll() select new { Id = srv.Id, Name = srv.Name });
            
            ViewBag.JsonStates = JsonConvert.SerializeObject(states, Formatting.None);
            ViewBag.JsonServices = JsonConvert.SerializeObject(services, Formatting.None);
            return View(model);
        }

        public string GetClaim()
        {
            var filterObject = GetFilter();

            var model = from c in db.ClaimRepository.GetAll()
                        where
                        (filterObject.Id == 0 || c.Id == filterObject.Id) &&
                        (string.IsNullOrEmpty(filterObject.StateName) || c.ClaimState.Name.Contains(filterObject.StateName)) &&
                        (string.IsNullOrEmpty(filterObject.ServiceName) || c.Service.Name.Contains(filterObject.ServiceName)) &&
                        (string.IsNullOrEmpty(filterObject.AdressName) || c.Adress.AdressName.Contains(filterObject.AdressName)) &&
                        (filterObject.OsbCode == 0 || c.Adress.OsbCode == filterObject.OsbCode)
                        select new ClaimGridViewModel
                        {
                            Id = c.Id,
                            DateCreate = c.CreateAt,
                            StateName = c.ClaimState.Name,
                            ServiceName = c.Service.Name,
                            AdressName = c.Adress.AdressName,
                            OsbCode = c.Adress.OsbCode
                            
                        };
            return JsonConvert.SerializeObject(model, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        private ClaimGridViewModel GetFilter()
        {
            var filter = Request.QueryString;

            return new ClaimGridViewModel()
            {
                Id = string.IsNullOrEmpty(filter.Get("Id")) ? 0 : int.Parse(filter.Get("Id")),
                StateName = filter.Get("StateName"),
                ServiceName = filter.Get("ServiceName"),
                AdressName = filter.Get("AdressName"),
                OsbCode = string.IsNullOrEmpty(filter.Get("OsbCode")) ? 0 : int.Parse(filter.Get("OsbCode"))
            };
        }

        public void DeleteClaim(int id)
        {
            try
            {
                db.ClaimRepository.DeleteById(id);
                db.Save();
            }
            catch (Exception ex)
            {

                ApplicationLogger.Instance.Error($"Ошибка во время ручного удаления заявки. {ex.Message} {ex.StackTrace}");
            }
            
        }

        public ActionResult Edit(int id)
        {
            var model = db.ClaimRepository.GetById(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Claim model)
        {

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