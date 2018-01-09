using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Abstract
{
    public interface IUnitOfWork: IDisposable
    {
        ICountryRepository CountryRepository { get; }
        ICurrencyRepository CurrencyRepository { get; }
        void Complete();
        Task CompleteAsync();
    }
}
