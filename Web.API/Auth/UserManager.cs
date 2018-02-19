using Data.Common.Abstract;
using Data.Identity.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.API.Auth
{
    public class UserManager: UserManager<User>
    {
        public UserManager(IUserStore<User> store)
            : base(store) { }
    }
    
}