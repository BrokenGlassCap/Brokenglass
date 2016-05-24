using BrokenGlassDomain;
using BrokenGlassDomain.DataLayer;
using BrokenGlassDomain.ServiceUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

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

        public Claim GetClaimById(int id)
        {
            var claim = m_db.ClaimRepository.GetById(id);
            if (claim == null)
            {
                ApplicationLogger.Instance.Trace(string.Format("Claims/GET: Claim was not found with specified ID: {0}", id));
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return claim;

        }
        [HttpPost]
        public HttpResponseMessage PostClaim(Claim claim)
        {
            try
            {
                if (claim == null) throw new ArgumentNullException();

                var state = m_db.ClaimStateRepository.GetAll().FirstOrDefault(f => f.Code == "STATE_SENT");
                if (state == null)
                {
                    throw new NullReferenceException("ClaimState with code STATE_SENT is not exist");
                }

                if (claim.UserId <= 0)
                {
                    SetClaimUserId(claim);
                }

                SetNullClaimForeignEntities(claim);
                claim.CreateAt = DateTime.UtcNow;
                claim.ClaimStateId = state.Id;

                m_db.ClaimRepository.Insert(claim);
                m_db.Save();
            }
            catch (Exception ex)
            {
                ApplicationLogger.Instance.Trace(string.Format("Claims/POST: Claim didnt created: {0} {1}", ex.Message, ex.StackTrace));
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            claim.User = null;
            claim.Photo.Clear();
            var httpResponse = Request.CreateResponse<Claim>(HttpStatusCode.Created, claim);
            return httpResponse;
        }

        private void SetNullClaimForeignEntities(Claim claim)
        {
            claim.ClaimState = null;
            claim.Adress = null;
            claim.Service = null;
            claim.User = null;
        }

        private void SetClaimUserId(Claim claim)
        {
            
            var userEmail = claim.User.Email;
            if (string.IsNullOrEmpty(userEmail)) throw new NullReferenceException("Claim.User.Email equals null!");

            var user = m_db.UserRepository.GetAll().FirstOrDefault(f => f.Email == userEmail);
            if (user == null)
            {
                throw new Exception(string.Format("Пользователь с email {0} не найден.", userEmail));
            }
            claim.UserId = user.Id;
        }

       
    }

    public class StubClaimObject
    {
        public Claim claim { get; set; }
        public Photo photo { get; set; }
    }

}
