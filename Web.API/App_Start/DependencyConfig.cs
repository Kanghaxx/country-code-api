 using System;
using Ninject;
using Microsoft.AspNet.Identity;
using Ninject.Web.Common;

using Data.Repository;
using Data.Common.Abstract;
using Data.Identity.Abstract;

namespace Web.API
{
    public static class DependencyConfig 
    {
        public static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IStoreFactory, IUserFactory>().To<DbFactory>().InRequestScope();
        }        
    }
}
