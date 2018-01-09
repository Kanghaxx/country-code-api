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

        public Country GetCountry(string isoCode)
        {
            return CountryContext.Countries
                .Where(c => c.IsoCode == isoCode)
                .FirstOrDefault();
        }

        public IEnumerable<Country> GetCountries()
        {
            return CountryContext.Countries
                .OrderBy(c => c.IsoCode)
                .ToList();
        }

        public IEnumerable<Country> FindCountries(string[] isoCodes, string[] countryNames)
        {
            // todo look at SQL
            var query = CountryContext.Countries.AsQueryable();
            if (isoCodes != null && isoCodes.Count() > 0)
            {
                query = query.Where(c => isoCodes.Contains(c.IsoCode)); 
            }
            if (countryNames != null && countryNames.Count() > 0)
            {
                query = query.Where(c => countryNames.Contains(c.Name));
            }
            var result = query.OrderBy(c => c.IsoCode)
                .ToList();
            return result;
        }
    }
}
