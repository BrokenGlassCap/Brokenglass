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
    public class AdressesController : ApiController
    {
        private IUnitOfWork m_dbRepositories;
        public AdressesController()
        {
            m_dbRepositories = NinjectService.Instance.GetService<IUnitOfWork>();
        }

        public AdressesController(IUnitOfWork dbContext)
        {
            m_dbRepositories = dbContext;
        }

        public IEnumerable<Adress> Get()
        {
            return m_dbRepositories.AdressRepository.GetAll();
        }

        public Adress Get(int id)
        {
            var adress = m_dbRepositories.AdressRepository.GetAll()
                .FirstOrDefault(f => f.Id == id);
            if (adress == null)
            {
                ApplicationLogger.Instance.Trace(string.Format("Adress/GET: request had requested Adress object with non-existen Id."));
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return adress;
        }

        public IEnumerable<Adress> Get(DateTime lastUpdateDate)
        {
            var adress = m_dbRepositories.AdressRepository.GetAll()
                .Where(f => f.UpdateAt >= lastUpdateDate);

            if (adress.Count() == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NoContent);
            }

            return adress;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_dbRepositories.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
