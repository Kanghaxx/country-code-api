using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.Common.Model;

namespace Data.Abstract
{
    public interface ICurrencyRepository : IRepository<Currency>
    {
        Currency GetCurrency(string isoCode);
        IEnumerable<Currency> GetCurrencies();
    }
}
