using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Data.Common.Model;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using Data.Identity.Model;

namespace Data.Repository.Context
{
    public class CountryContext: IdentityDbContext<User>
    {
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }

        public void FixEfProviderServicesProblem()
        {
            //The Entity Framework provider type 'System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer'
            //for the 'System.Data.SqlClient' ADO.NET provider could not be loaded. 
            //Make sure the provider assembly is available to the running application. 
            //See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.

            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public CountryContext() 
        {
        }

        public CountryContext(string cs) : base (cs)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Country>()
                .Property(t => t.IsoCode)
                .HasColumnAnnotation("Index", new IndexAnnotation(
                    new IndexAttribute()
                    {
                        IsUnique = true
                    }
            ));

            modelBuilder.Entity<Currency>()
                .Property(t => t.IsoCode)
                .HasColumnAnnotation("Index", new IndexAnnotation(
                    new IndexAttribute()
                    {
                        IsUnique = true
                    }
            ));
            
        }


    }
}
