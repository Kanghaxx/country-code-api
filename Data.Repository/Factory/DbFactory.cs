using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using Data.Common.Abstract;
using Data.Common.Model;
using Data.Repository.Context;
using Data.Identity.Model;
using Data.Identity.Abstract;

namespace Data.Repository
{
    public class DbFactory : IStoreFactory, IUserFactory
    {
        protected CountryContext CountryContext { get; set; }

        public DbFactory()
        {
            CountryContext = new CountryContext("name=CountriesConnectionString");
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(CountryContext);
        }

        public IUserStore<User> CreateUserStore()
        {
            return new UserStore<User>(CountryContext);
        }
        
        public void Dispose()
        {
            CountryContext.Dispose();
        }
    }
}
