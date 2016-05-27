using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrokenGlassDomain.DataLayer;
using BrokenGlassDomain.Exceptions;
using BrokenGlassDomain.ServiceUtils;

namespace BrokenGlassDomain.Entities
{
    public class UserFactory: IDisposable
    {
        private IUnitOfWork m_db;
        private AuthDBContext authContext;
        private UserManager<IdentityUser> m_userManager;

        protected UserManager<IdentityUser> UserManager {
            get {
                return m_userManager;
            }
            private set {
                m_userManager = value;
            }
        }
        private UserFactory()
        {
            m_db = NinjectService.Instance.GetService<IUnitOfWork>();
            authContext = new AuthDBContext();
            m_userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(authContext));
        }
        private UserFactory(UserManager<IdentityUser> obj, AuthDBContext ctx, IUnitOfWork db)
        {
            m_db = db;
            m_userManager = obj;
            authContext = ctx;
        }

        public static UserFactory GetUser()
        {
            return new UserFactory();
        }

        public static UserFactory GetUser(UserManager<IdentityUser> obj, AuthDBContext ctx, IUnitOfWork db)
        {
            return new UserFactory(obj,ctx,db);
        }

        public async Task RegistrationAsync(UserRegistrationModel user)
        {
            var identityUser = new IdentityUser() {
                UserName = user.Email,
                Email = user.Email
            };
            var identityCreateResult = await m_userManager.CreateAsync(identityUser, user.Password);

            if (identityCreateResult == null)
            {
                throw new IdentityUserCreateException("A result object of IdentityUser type equals null");
            }

            if (!identityCreateResult.Succeeded)
            {
                throw new IdentityUserCreateException("A object of IdentityUser type was not create.");
            }

            var identityNewUser = await FindIdentityUserAsync(user.Email);
            var userDb = new User()
            {
                Email = identityNewUser.UserName,
                IdentityUserId = identityNewUser.Id,
                UpdateAt = DateTime.UtcNow
            };
            m_db.UserRepository.Insert(userDb);
            await m_db.SaveAsync();
        }

        public async Task<IdentityUser> FindIdentityUserAsync(string email)
        {
            return await m_userManager.FindByNameAsync(email);
        }

        public void Dispose()
        {
            m_db.Dispose();
            m_userManager.Dispose();
            authContext.Dispose();
        }
    }
}
