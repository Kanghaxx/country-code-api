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
            /*
             TODO
             DB
                # Descriptions: use custom migration operations to create in DB 

                # Country: normalize iso-codes by country (there are 4 types for country code in ISO-3166).
                    https://ru.wikipedia.org/wiki/ISO_3166-1
                    https://ru.wikipedia.org/wiki/ISO_3166-2
                    All types in JSON:
                    https://github.com/lukes/ISO-3166-Countries-with-Regional-Codes
                    Main entity should have most common code (3166 alpha-2).
                # Currency: normalize iso-codes by currency
                    https://en.wikipedia.org/wiki/ISO_4217
                    https://www.iban.com/currency-codes.html
            */

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
