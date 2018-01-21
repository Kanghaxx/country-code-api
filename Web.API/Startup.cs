﻿using System;
using System.Threading.Tasks;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using Owin;
using Microsoft.Owin;
using Ninject;
using Ninject.Web.WebApi.OwinHost;
using Ninject.Web.Common.OwinHost;

[assembly: OwinStartup(typeof(Web.API.Startup))]

namespace Web.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            
            app.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(config);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private static StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            DependencyConfig.RegisterServices(kernel);

            return kernel;
        }
    }
}
