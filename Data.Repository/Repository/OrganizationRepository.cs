using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using Data.Common.Abstract;
using Data.Common.Model;
using Data.Repository.Context;


namespace Data.Repository
{
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {
        private CountryContext CountryContext { get; set; }


        public OrganizationRepository(CountryContext countryContext): base (countryContext)
        {
            CountryContext = countryContext;
        }


        private IQueryable<Organization> GetQuery(string name)
        {
            return CountryContext.Organizations
                .Where(o => o.Name == name)
                .Include(o => o.Countries)
                .OrderBy(o => o.Name);
        }

        public Organization Get(string name)
        {
            return GetQuery(name).FirstOrDefault();
        }

        public async Task<Organization> GetAsync(string isoCode)
        {
            return await GetQuery(isoCode).FirstOrDefaultAsync();
        }

        private IQueryable<Organization> GetQuery()
        {
            return CountryContext.Organizations
                .Include(o => o.Countries)
                .OrderBy(o => o.Name);
        }

        public IEnumerable<Organization> Get()
        {
            return GetQuery().ToList();
        }

        public async Task<IEnumerable<Organization>> GetAsync()
        {
            return await GetQuery().ToListAsync();
        }
        

        private IQueryable<Organization> FindQuery(string[] names)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Organization> Find(string[] isoCodes)
        {
            return FindQuery(isoCodes).ToList();
        }

        public async Task<IEnumerable<Organization>> FindAsync(string[] isoCodes)
        {
            return await FindQuery(isoCodes).ToListAsync();
        }
    }
}
