using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Data.Abstract;
using Data.Common.Model;
using Data.Repository.Context;


namespace Data.Repository
{
    public class CurrencyRepository : Repository<Currency>, ICurrencyRepository
    {
        private CountryContext CountryContext { get; set; }
        
        public CurrencyRepository(CountryContext countryContext): base (countryContext)
        {
            CountryContext = countryContext;
        }

        public Currency Get(string isoCode)
        {
            return CountryContext.Currencies
                .Where(c => c.IsoCode == isoCode)
                .Include(c => c.Countries)
                .FirstOrDefault();
        }

        public IEnumerable<Currency> Get()
        {
            return CountryContext.Currencies
                .Include(c => c.Countries)
                .OrderBy(c => c.IsoCode)
                .ToList();
        }

        public IEnumerable<Currency> Find(string[] isoCodes)
        {
            var query = CountryContext.Currencies.AsQueryable();
            if (isoCodes != null && isoCodes.Count() > 0)
            {
                query = query.Where(c => isoCodes.Contains(c.IsoCode));
            }
            var result = query
                .Include(c => c.Countries)
                .OrderBy(c => c.IsoCode)
                .ToList();
            return result;
        }
    }
}
