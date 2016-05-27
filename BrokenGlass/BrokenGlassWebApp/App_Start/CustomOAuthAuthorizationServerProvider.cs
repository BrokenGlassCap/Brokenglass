using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using BrokenGlassDomain.Entities;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace BrokenGlassWebApp.App_Start
{
    public class CustomOAuthAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private string clientId;
        private string clientSecret;
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.Validated();
            }
            else
            {
                context.SetError("invalid_client", "Client credentials could not be retrieved from the Authorization header");
                context.Rejected();
            }
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (var userFactory = UserFactory.GetUser())
            {
                try
                {
                    var user = await userFactory.FindIdentityUserNameAndPasswordAsync(context.UserName, context.Password);
                    if (user == null)
                    {
                        context.SetError("invalid_client");
                        context.Rejected();
                        return;
                    }

                    if (!user.EmailConfirmed)
                    {
                        context.SetError("invalid_client", "Your email is not confirmed");
                        context.Rejected();
                        return;
                    }
                    ClaimsIdentity claim = await userFactory.CreateIdentityAsync(user, DefaultAuthenticationTypes.ExternalBearer);
                    context.Validated(claim);

                }
                catch (Exception ex)
                {
                    context.SetError("server_error");
                    context.Rejected();
                    return;
                }
                
            }   
        }
    }
}