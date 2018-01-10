using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.Common.Model;

namespace Data.Abstract
{
    public interface ICountryRepository : IRepository<Country>
    {
        Country Get(string isoCode);
        IEnumerable<Country> Get();
        IEnumerable<Country> Find(string[] isoCodes);
    }
}
