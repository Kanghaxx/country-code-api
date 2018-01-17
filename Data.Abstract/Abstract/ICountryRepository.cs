using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.Common.Model;
using System.Threading.Tasks;

namespace Data.Common.Abstract
{
    public interface ICountryRepository : IRepository<Country>
    {
        Country Get(string isoCode);
        Task<Country> GetAsync(string isoCode);

        IEnumerable<Country> Get();
        Task<IEnumerable<Country>> GetAsync();

        IEnumerable<Country> Find(string[] isoCodes);
        Task<IEnumerable<Country>> FindAsync(string[] isoCodes);
    }
}
