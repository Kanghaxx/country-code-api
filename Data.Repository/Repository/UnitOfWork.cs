using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using Data.Common.Abstract;
using Data.Repository.Context;

namespace Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ICountryRepository _countryRepository;
        public ICountryRepository CountryRepository
        {
            get
            {
                if (_countryRepository == null)
                {
                    _countryRepository = new CountryRepository(CountryContext);
                }
                return _countryRepository;
            }
        }

        private ICurrencyRepository _currencyRepository;
        public ICurrencyRepository CurrencyRepository
        {
            get
            {
                if (_currencyRepository == null)
                {
                    _currencyRepository = new CurrencyRepository(CountryContext);
                }
                return _currencyRepository;
            }
        }

        
        private IOrganizationRepository _organizationRepository;
        public IOrganizationRepository OrganizationRepository
        {
            get
            {
                if (_organizationRepository == null)
                {
                    _organizationRepository = new OrganizationRepository(CountryContext);
                }
                return _organizationRepository;
            }
        }

        protected CountryContext CountryContext { get; set; }

        public UnitOfWork(CountryContext context)
        {
            CountryContext = context;
        }
        

        public void Complete()
        {
            CountryContext.SaveChanges();
        }

        public async Task CompleteAsync()
        {
            await CountryContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            //CountryContext.Dispose();
        }

    }
}
