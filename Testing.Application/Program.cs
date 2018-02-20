using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;

using Data.Repository;
using Data.Common.Abstract;
using Data.Repository.Context;
using Data.Common.Model;
using Data.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Data.Identity.Model;
using Web.API.Auth;

namespace Testing.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new CountryContext("name=CountriesConnectionString"))
            {
                using (var store = new UserStore<User>(context))
                {
                    using (var manager = new UserManager(store))
                    {
                        User u = new User()
                        {
                            UserName = "TestName",
                            PhoneNumber = "8123456",
                            Email = "vray@bk.ru"
                        };

                        var res = manager.CreateAsync(u, "password").Result;
                        

                    }
                    
                }
            }
            //using (IUnitOfWork uow = new UnitOfWork(new CountryContext()))
            //{
            //    var countries = uow.CountryRepository.Find(
            //        //new string[] { "RU", "LV" },
            //        null
            //        //new string[] { "USA", "Latvia", "XXX"});
            //        );
            //    uow.Complete();
            //}

                Console.WriteLine("Ready");
            Console.ReadKey();
        }
    }
}
