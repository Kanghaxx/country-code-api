using System;
using System.Threading.Tasks;
using System.Reflection;
using System.Web.Http;
using System.Web.Optimization;
using Owin;
using Microsoft.Owin;
using Ninject;
using Ninject.Web.WebApi.OwinHost;
using Ninject.Web.Common.OwinHost;
using Microsoft.Owin.Security.OAuth;
using Web.API.Auth;
using Data.Common.Abstract;
using Data.Repository;
using Data.Identity;
using Data.Identity.Model;
using Microsoft.AspNet.Identity;
using Data.Identity.Abstract;
using Microsoft.AspNet.Identity.Owin;

[assembly: OwinStartup(typeof(Web.API.Startup))]

namespace Web.API
{
    public class Startup
    {
        private IKernel kernel;
        
        private UserManager CreateUserManager(
            IdentityFactoryOptions<UserManager> options, 
            IOwinContext context)
        {
            var factory = kernel.Get<IUserFactory>();
            return new UserManager(factory.CreateUserStore());
        }


        public void Configuration(IAppBuilder app)
        {
            // Configuration() is executed once per AppDomain.
            // CreatePerOwinContext(Delegate) is executed once per request. 
            //   Instance returned from Delegate will be disposed after every request.

            kernel = CreateKernel();
            app.CreatePerOwinContext<UserManager>(CreateUserManager);

            ConfigureAuth(app);

            HttpConfiguration config = new HttpConfiguration();
            
            //AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            WebApiConfig.Register(config);

            app.UseNinjectMiddleware(() => kernel).UseNinjectWebApi(config);

        }

        private void ConfigureAuth(IAppBuilder app)
        {
            var OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                
                // for testing purpose only
                AllowInsecureHttp = true
            };

            // Inserting Auth Server to OWIN pipeline
            app.UseOAuthAuthorizationServer(OAuthOptions);

            // Inserting OAuth Token validation to OWIN pipeline
            app.UseOAuthBearerAuthentication(
                    new OAuthBearerAuthenticationOptions());
        }
        

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            DependencyConfig.RegisterServices(kernel);
            
            return kernel;
        }
    }
}
