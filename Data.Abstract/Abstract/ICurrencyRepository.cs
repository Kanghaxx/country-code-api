using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.Common.Model;

namespace Data.Abstract
{
    public interface ICurrencyRepository : IRepository<Currency>
    {
        Currency Get(string isoCode);
        IEnumerable<Currency> Get();
        IEnumerable<Currency> Find(string[] isoCodes);
    }
}
