using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using Data.Abstract;
using Data.Common.Model;
using Data.Repository.Context;


namespace Data.Repository
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        private CountryContext CountryContext { get; set; }


        public CountryRepository(CountryContext countryContext): base (countryContext)
        {
            CountryContext = countryContext;
        }


        private IQueryable<Country> GetQuery(string isoCode)
        {
            return CountryContext.Countries
                .Where(c => c.IsoCode == isoCode)
                .Include(c => c.Currencies);
        }

        public Country Get(string isoCode)
        {
            return GetQuery(isoCode).FirstOrDefault();
        }

        public async Task<Country> GetAsync(string isoCode)
        {
            return await GetQuery(isoCode).FirstOrDefaultAsync();
        }

        private IQueryable<Country> GetQuery()
        {
            return CountryContext.Countries
                .Include(c => c.Currencies)
                .OrderBy(c => c.IsoCode);
        }

        public IEnumerable<Country> Get()
        {
            return GetQuery().ToList();
        }

        public async Task<IEnumerable<Country>> GetAsync()
        {
            return await GetQuery().ToListAsync();
        }
        

        private IQueryable<Country> FindQuery(string[] isoCodes)
        {
            var query = CountryContext.Countries.AsQueryable();
            if (isoCodes != null && isoCodes.Count() > 0)
            {
                query = query.Where(c => isoCodes.Contains(c.IsoCode));
            }
            //if (countryNames != null && countryNames.Count() > 0)
            //{
            //    query = query.Where(c => countryNames.Contains(c.Name));
            //}
            var result = query
                .Include(c => c.Currencies)
                .OrderBy(c => c.IsoCode);
            return result;
        }

        public IEnumerable<Country> Find(string[] isoCodes)
        {
            return FindQuery(isoCodes).ToList();
        }

        public async Task<IEnumerable<Country>> FindAsync(string[] isoCodes)
        {
            return await FindQuery(isoCodes).ToListAsync();
        }
    }
}
