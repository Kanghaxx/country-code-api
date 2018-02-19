using Data.Identity;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;
using System.Security.Claims;
using Microsoft.AspNet.Identity.Owin;
using Data.Identity.Model;

namespace Web.API.Auth
{
    public class ApplicationOAuthServerProvider: OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(
            OAuthValidateClientAuthenticationContext context)
        {
            // Client authentication goes here
            await Task.FromResult(context.Validated());
        }


        public override async Task GrantResourceOwnerCredentials(
            OAuthGrantResourceOwnerCredentialsContext context)
        {
            // TODO Identity
            var manager = context.OwinContext.GetUserManager<UserManager>();
            if (context.Password != "password")
            {
                context.SetError("invalid_grant", 
                    "The user name or password is incorrect.");
                context.Rejected();
                return;
            }

            // Create ClaimsIdentity object to represent the authenticated user
            ClaimsIdentity identity =
                new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("user_name", context.UserName));

            // validate context to return access token to client
            context.Validated(identity);
        }
    }
}