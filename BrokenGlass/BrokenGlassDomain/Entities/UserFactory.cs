using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrokenGlassDomain.DataLayer;
using BrokenGlassDomain.Exceptions;

namespace BrokenGlassDomain.Entities
{
    public class UserFactory
    {
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
            authContext = new AuthDBContext();
            m_userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(authContext));
        }

        public static UserFactory GetUser()
        {
            return new UserFactory();
        }

        public async Task Registration(UserRegistrationModel user)
        {
            var identityUser = new IdentityUser() {
                UserName = user.Email
            };
            var identityResult = await this.UserManager.CreateAsync(identityUser, user.Password);

            if (identityResult == null)
            {
                throw new IdentityUserCreateException("A result object of IdentityUser type equals null");
            }
            
        }
    }
}
