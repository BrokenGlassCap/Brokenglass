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
    public class ServicesController : ApiController
    {
        private IUnitOfWork m_dbRepositories;
        public ServicesController()
        {
            m_dbRepositories = NinjectService.Instance.GetService<IUnitOfWork>();
        }

        public ServicesController(IUnitOfWork dbContext)
        {
            m_dbRepositories = dbContext;
        }

        public IEnumerable<Service> Get()
        {
            return m_dbRepositories.ServiceRepository.GetAll();
        }

        public Service Get(string code)
        {
            var service = m_dbRepositories.ServiceRepository.GetAll()
                .FirstOrDefault(f => f.Code == code);
            if (service == null)
            {
                ApplicationLogger.Instance.Trace(string.Format("Service/GET: request had requested Service object with non-existen Code."));
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return service;
        }

        public Service Get(int id)
        {
            var service = m_dbRepositories.ServiceRepository.GetAll()
                .FirstOrDefault(f => f.Id == id);
            if (service == null)
            {
                ApplicationLogger.Instance.Trace(string.Format("Service/GET: request had requested Service object with non-existen Id."));
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return service;
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
