﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrokenGlassDomain.DataLayer;
using BrokenGlassDomain.Exceptions;
using BrokenGlassDomain.ServiceUtils;
using System.Security.Claims;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

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

        public async Task<IdentityUser> RegistrationAsync(UserRegistrationModel user)
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
                var stringError = CreateStringErrorByIdentityResultErrors(identityCreateResult);
                throw new IdentityUserCreateException($"A object of IdentityUser type was not create.\n{stringError}");
            }

            IdentityUser identityNewUser = null;
            try
            {
                identityNewUser = await FindIdentityUserAsync(user.Email);
                var userDb = new User()
                {
                    Email = identityNewUser.UserName,
                    IdentityUserId = identityNewUser.Id,
                    UpdateAt = DateTime.UtcNow
                };
                m_db.UserRepository.Insert(userDb);
                await m_db.SaveAsync();
            }
            catch (Exception ex)
            {
                await m_userManager.DeleteAsync(identityNewUser);
                throw new IdentityUserCreateException($"At momet creating Broken Glass Db User was throw exception {ex.Message} {ex.StackTrace}");
            }

            return identityNewUser;
        }

        public async Task<IdentityUser> FindIdentityUserAsync(string email)
        {
            return await m_userManager.FindByNameAsync(email);
        }
        public async Task<IdentityUser> FindIdentityUserByIdAsync(string userId)
        {
            return await m_userManager.FindByIdAsync(userId);
        }

        public async Task<IdentityUser> FindIdentityUserNameAndPasswordAsync(string email, string password)
        {
            return await m_userManager.FindAsync(email, password);
        }

        public async Task<ClaimsIdentity> CreateIdentityAsync(IdentityUser user, string authenticationType )
        {
            return await m_userManager.CreateIdentityAsync(user, authenticationType);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(string userId)
        {
            m_userManager.UserTokenProvider = CreateUserTokenProvider();
            return await m_userManager.GenerateEmailConfirmationTokenAsync(userId);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            m_userManager.UserTokenProvider = CreateUserTokenProvider();
            return await m_userManager.ConfirmEmailAsync(userId, token);
        }

        private DataProtectorTokenProvider<IdentityUser> CreateUserTokenProvider()
        {
            var factoryOptions = new IdentityFactoryOptions<UserManager<IdentityUser>>();
            factoryOptions.DataProtectionProvider = new DpapiDataProtectionProvider("BGWebApi");
            return new DataProtectorTokenProvider<IdentityUser>(factoryOptions.DataProtectionProvider.Create("ASP.NET Identity"))
            {
                TokenLifespan = TimeSpan.FromHours(6)
            };
        }

        public string CreateStringErrorByIdentityResultErrors(IdentityResult identityResult)
        {
            StringBuilder strBuilder = new StringBuilder();
            foreach (var error in identityResult.Errors)
            {
                strBuilder.AppendLine(error);
            }

            return strBuilder.ToString();
        }

        public async Task<bool> CheckUserAccess(UserLoginModel model)
        {
            var userIdentity = await FindIdentityUserAsync(model.Email);
            return await m_userManager.CheckPasswordAsync(userIdentity, model.Password);   
        }

        public async Task<IEnumerable<string>> GetUsersRoles(string email)
        {
            var userIdentity = await FindIdentityUserAsync(email);
            if (userIdentity == null)
            {
                throw new Exception("User is not exist with specify Email");
            }

           return await m_userManager.GetRolesAsync(userIdentity.Id);

        }


        public void Dispose()
        {
            m_db.Dispose();
            m_userManager.Dispose();
            authContext.Dispose();
        }
    }
}
