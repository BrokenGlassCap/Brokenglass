﻿using BrokenGlassDomain;
using BrokenGlassDomain.DataLayer;
using BrokenGlassDomain.ServiceUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace BrokenGlassWebApp.Controllers.api
{
    [Authorize]
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

        public async Task<IEnumerable<Service>> Get()
        {
            
            return await m_dbRepositories.ServiceRepository.GetAllAsync();
        }

        public async Task<Service> Get(string code)
        {
            var service = await m_dbRepositories.ServiceRepository.FindAsync(f => f.Code == code);

            if (service == null)
            {
                ApplicationLogger.Instance.Trace(string.Format("Service/GET: request had requested Service object with non-existen Code."));
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return service;
        }

        public async Task<Service> Get(int id)
        {
            var service = await m_dbRepositories.ServiceRepository.FindAsync(f => f.Id == id);
                
            if (service == null)
            {
                ApplicationLogger.Instance.Trace(string.Format("Service/GET: request had requested Service object with non-existen Id."));
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return service;
        }

        public async Task<IEnumerable<Service>> Get(DateTime lastUpdateDate)
        {
            var service = await m_dbRepositories.ServiceRepository.FindAllAsync(f => f.UpdateAt > lastUpdateDate);

            if (service.Count() == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NoContent);
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
