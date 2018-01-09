using Data.Abstract;
using Data.Common.Model;
using Data.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class CurrencyRepository : Repository<Currency>, ICurrencyRepository
    {
        private CountryContext CountryContext { get; set; }
        

        public CurrencyRepository(CountryContext countryContext): base (countryContext)
        {
            CountryContext = countryContext;
        }
        
        
        public Currency GetCurrency(string isoCode)
        {
            return CountryContext.Currencies
                .Where(c => c.IsoCode == isoCode)
                .FirstOrDefault();
        }

        public IEnumerable<Currency> GetCurrencies()
        {
            return CountryContext.Currencies.ToList();
        }
    }
}
