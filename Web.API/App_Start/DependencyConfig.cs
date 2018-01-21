 using System;
using Ninject;

using Data.Repository;
using Data.Common.Abstract;

namespace Web.API
{
    public static class DependencyConfig 
    {
        public static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IStoreFactory>().To<DbFactory>().InSingletonScope();
        }        
    }
}
