using BrokenGlassDomain;
using BrokenGlassDomain.DataLayer;
using BrokenGlassDomain.ServiceUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<ClaimState>> Get()
        {
            return await db.ClaimStateRepository.GetAllAsync();
        }

        public async Task<ClaimState> Get(string code)
        {
            var claimState = await db.ClaimStateRepository.FindAsync(f => f.Code == code);
            if (claimState == null)
            {
                ApplicationLogger.Instance.Trace(string.Format("ClaimState/GET: request had requested ClaimState object with non-existen Code."));
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return claimState;
        }

        public async Task<ClaimState> Get(int id)
        {
            var claimState = await db.ClaimStateRepository.FindAsync(f => f.Id == id);
            if (claimState == null)
            {
                ApplicationLogger.Instance.Trace(string.Format("ClaimState/GET: request had requested ClaimState object with non-existen ID."));
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return claimState;
        }

        public async Task<IEnumerable<ClaimState>> Get(DateTime lastUpdateDate)
        {
            var claimStates = await db.ClaimStateRepository.FindAllAsync(f => f.UpdateAt >= lastUpdateDate);

            if (claimStates.Count() == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NoContent);
            }

            return claimStates;
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
