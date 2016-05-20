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
    public class ClaimsController : ApiController
    {
        private IUnitOfWork m_db;
        public ClaimsController()
        {
            m_db = NinjectService.Instance.GetService<IUnitOfWork>();
        }
        public ClaimsController(IUnitOfWork unitOfWork)
        {
            m_db = unitOfWork;
        }

        public IEnumerable<Claim> Get(int userId)
        {
            var claims = m_db.ClaimRepository.GetAll().Where(w => w.UserId == userId);

            if (claims.Count() == 0)
            {
                ApplicationLogger.Instance.Trace(string.Format("Claims/GET: request had requested Claims object with non-existen UserId."));
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return claims;
        }

        public IEnumerable<Claim> Get(string userEmail)
        {

            var user = m_db.UserRepository.GetAll().FirstOrDefault(f => f.Email == userEmail);
            if (user == null)
            {
                ApplicationLogger.Instance.Trace(string.Format("Claims/GET: User was not found with specified Email: {0}",userEmail));
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            if (user.Claim.Count() == 0)
            {
                ApplicationLogger.Instance.Trace(string.Format("Claims/GET: request not have result like a Claims object with specified UserEmail."));
                throw new HttpResponseException(HttpStatusCode.NoContent);
            }

            user.Claim.ToList().ForEach(p => p.Photo.Clear());
            return user.Claim;
        }
    }
}
