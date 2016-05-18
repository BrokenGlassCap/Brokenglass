using BrokenGlassDomain;
using BrokenGlassDomain.DataLayer;
using BrokenGlassDomain.ServiceUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BrokenGlassWebApp.Controllers.api
{
    public class ClaimStatesController : ApiController
    {
        private IUnitOfWork db;
        public ClaimStatesController()
        {
            db = NinjectService.Instance.GetService<IUnitOfWork>();
        }
        public ClaimStatesController(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }

        public IEnumerable<ClaimState> Get()
        {
            return db.ClaimStateRepository.GetAll();
        }

        public ClaimState Get(string code)
        {
            var claimState = db.ClaimStateRepository.GetAll().FirstOrDefault(f => f.Code == code);
            if (claimState == null)
            {
                ApplicationLogger.Instance.Trace(string.Format("ClaimState/GET: request had requested ClaimState object with non-existen Code."));
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return claimState;
        }

        public ClaimState Get(int id)
        {
            var claimState = db.ClaimStateRepository.GetAll().FirstOrDefault(f => f.Id == id);
            if (claimState == null)
            {
                ApplicationLogger.Instance.Trace(string.Format("ClaimState/GET: request had requested ClaimState object with non-existen ID."));
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return claimState;
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
