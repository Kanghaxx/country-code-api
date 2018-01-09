using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.Common.Model;

namespace Data.Abstract
{
    public interface ICountryRepository : IRepository<Country>
    {
        Country GetCountry(string isoCode);
        IEnumerable<Country> GetCountries();
        IEnumerable<Country> FindCountries(string[] isoCodes, string[] countryNames);
    }
}
