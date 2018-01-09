using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Abstract;

namespace Data.Repository
{
    public class DbFactory : IStoreFactory
    {
        public IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork();
        }
    }
}
