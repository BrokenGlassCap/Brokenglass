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

        public void SetDbContext(IUnitOfWork dbContext)
        {
            m_dbRepositories = dbContext;
        }

        public IEnumerable<Service> Get()
        {
            return m_dbRepositories.ServiceRepository.GetAll();
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
