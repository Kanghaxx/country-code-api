using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.Common.Model;
using System.Threading.Tasks;

namespace Data.Common.Abstract
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        Organization Get(string name);
        Task<Organization> GetAsync(string name);

        IEnumerable<Organization> Get();
        Task<IEnumerable<Organization>> GetAsync();

        IEnumerable<Organization> Find(string[] name);
        Task<IEnumerable<Organization>> FindAsync(string[] name);
    }
}
