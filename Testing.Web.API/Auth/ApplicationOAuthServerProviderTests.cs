using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNet.Identity.Owin;

using Web.API.Auth;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin;
using Microsoft.AspNet.Identity;
using Data.Identity.Model;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Testing.Web.API.Auth
{
    [TestCategory("Web.Auth.ApplicationOAuthServerProvider")]
    [TestClass]
    public class ApplicationOAuthServerProviderTests
    {
        private User CreateUser()
        {
            var user = new User()
            {
                Id = "UserId1",
                UserName = "Name1"
            };

            user.Claims.Add(
                new IdentityUserClaim()
                {
                    ClaimType = "role",
                    ClaimValue = "admin",
                    Id = 1,
                    UserId = user.Id
                });
            user.Claims.Add(
                new IdentityUserClaim()
                {
                    ClaimType = "username",
                    ClaimValue = user.UserName,
                    Id = 2,
                    UserId = user.Id
                });
            return user;
        }

        [TestMethod]
        public async Task GrantCredentials_Grants_ValidUser()
        {
            string testPass = "Pass1";
            var user = CreateUser();

            var userStore = new Mock<IUserStore<User>>();
            
            var userManager = new Mock<UserManager>(userStore.Object);
            userManager.Setup(m => m.FindAsync(user.UserName, testPass))
                .Returns(Task.FromResult(user));

            var owinCotext = new OwinContext();
            owinCotext.Set(userManager.Object);
             
            var oAuthContext = new OAuthGrantResourceOwnerCredentialsContext(owinCotext, 
                new OAuthAuthorizationServerOptions(),
                user.Id, user.UserName, testPass, new List<string>());
            
            var provider = new ApplicationOAuthServerProvider();

            // Act
            await provider.GrantResourceOwnerCredentials(oAuthContext);
            
            Assert.IsTrue(oAuthContext.IsValidated);
            Assert.IsNotNull(oAuthContext.Ticket.Identity);
            Assert.IsNotNull(oAuthContext.Ticket.Identity.Claims);
            Assert.IsTrue(oAuthContext.Ticket.Identity.Claims.Count() == 2);
            userManager.Verify(m => m.FindAsync(user.UserName, testPass), Times.Once);

        }

        [TestMethod]
        public async Task GrantCredentials_Refuses_InvalidUser()
        {
            string testPass = "Pass1";
            var user = CreateUser();

            var userStore = new Mock<IUserStore<User>>();

            var userManager = new Mock<UserManager>(userStore.Object);
            userManager.Setup(m => m.FindAsync(user.UserName, testPass))
                .Returns(Task.FromResult<User>(null));

            var owinCotext = new OwinContext();
            owinCotext.Set(userManager.Object);

            var oAuthContext = new OAuthGrantResourceOwnerCredentialsContext(owinCotext,
                new OAuthAuthorizationServerOptions(),
                user.Id, user.UserName, testPass, new List<string>());

            var provider = new ApplicationOAuthServerProvider();

            // Act
            await provider.GrantResourceOwnerCredentials(oAuthContext);

            Assert.IsFalse(oAuthContext.IsValidated);
            Assert.IsNotNull(oAuthContext.Error);
            Assert.IsTrue(oAuthContext.Error == "invalid_grant");
            Assert.IsNull(oAuthContext.Ticket);
            userManager.Verify(m => m.FindAsync(user.UserName, testPass), Times.Once);

        }
    }
}
