using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Common.Abstract
{
    public interface IRepository<T> where T: class
    {
        T Add(T item);
        T Remove(T item);
        T Get(long id);
        Task<T> GetAsync(long id);
    }
}
