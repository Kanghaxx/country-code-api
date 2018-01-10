using Data.Repository.Context;
using Data.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repository;
using Data.Abstract;
using System.Drawing;
using System.Net;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {

            using (IUnitOfWork uow = new UnitOfWork())
            {
                var countries = uow.CountryRepository.Find(
                    //new string[] { "RU", "LV" },
                    null
                    //new string[] { "USA", "Latvia", "XXX"});
                    );
                uow.Complete();
            }

            //using (var context = new CountryContext())
            //{
            //    context.Countries.Add(
            //        new Country()
            //        {
            //            Name = "test",
            //            IsoCode = "RU"
            //            
            //        }
            //        );
            //    context.SaveChanges();
            //}

            Console.WriteLine("Ready");
            Console.ReadKey();
        }
    }
}
