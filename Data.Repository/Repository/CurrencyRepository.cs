using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Data.Common.Abstract;
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


        private IQueryable<Currency> GetQuery(string isoCode)
        {
            return CountryContext.Currencies
                .Where(c => c.IsoCode == isoCode)
                .Include(c => c.Countries);
        }

        public Currency Get(string isoCode)
        {
            return GetQuery(isoCode).FirstOrDefault();
        }

        public async Task<Currency> GetAsync(string isoCode)
        {
            return await GetQuery(isoCode).FirstOrDefaultAsync();
        }


        private IQueryable<Currency> GetQuery()
        {
            return CountryContext.Currencies
                .Include(c => c.Countries)
                .OrderBy(c => c.IsoCode);
        }

        public IEnumerable<Currency> Get()
        {
            return GetQuery().ToList();
        }

        public async Task<IEnumerable<Currency>> GetAsync()
        {
            return await GetQuery().ToListAsync();
        }


        private IQueryable<Currency> FindQuery(string[] isoCodes)
        {
            var query = CountryContext.Currencies.AsQueryable();
            if (isoCodes != null && isoCodes.Count() > 0)
            {
                query = query.Where(c => isoCodes.Contains(c.IsoCode));
            }
            return query
                .Include(c => c.Countries)
                .OrderBy(c => c.IsoCode);
        }

        public IEnumerable<Currency> Find(string[] isoCodes)
        {
            return FindQuery(isoCodes).ToList();
        }
        
        public async Task<IEnumerable<Currency>> FindAsync(string[] isoCodes)
        {
            return await FindQuery(isoCodes).ToListAsync();
        }
    }
}
