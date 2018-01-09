using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Data.Abstract;

namespace Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public DbContext Context { get; set; }

        public T Add(T item)
        {
            return Context.Set<T>().Add(item);
        }

        public T Get(long id)
        {
            return Context.Set<T>().Find(id);
        }

        public async Task<T> GetAsync(long id)
        {
            return await Context.Set<T>().FindAsync(id);
        }

        public Repository(DbContext context)
        {
            Context = context;
        }
    }
}
