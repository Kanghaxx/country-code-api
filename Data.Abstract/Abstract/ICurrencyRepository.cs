using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.Common.Model;
using System.Threading.Tasks;

namespace Data.Abstract
{
    public interface ICurrencyRepository : IRepository<Currency>
    {
        Currency Get(string isoCode);
        Task<Currency> GetAsync(string isoCode);

        IEnumerable<Currency> Get();
        Task<IEnumerable<Currency>> GetAsync();

        IEnumerable<Currency> Find(string[] isoCodes);
        Task<IEnumerable<Currency>> FindAsync(string[] isoCodes);
    }
}
