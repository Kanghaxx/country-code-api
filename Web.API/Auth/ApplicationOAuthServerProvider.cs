using Data.Identity;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;
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
            // Retrieve user with name/password combo from Identity UserManager
            var manager = context.OwinContext.Get<UserManager>();
            var user = await manager.FindAsync(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("invalid_grant", 
                    "The user name or password is incorrect.");
                context.Rejected();
                return;
            }

            // Add claims associated with this user to the ClaimsIdentity object
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            foreach (var userClaim in user.Claims)
            {
                identity.AddClaim(new Claim(userClaim.ClaimType, userClaim.ClaimValue));
            }

            context.Validated(identity);
        }
    }
}