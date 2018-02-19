using Data.Common.Abstract;
using Data.Identity.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Identity
{
    /* 
        UserManager is here to decouple client apps from EntityFramework package.
        Better it should be in the app level, but IdentityUser class which User is derived from 
        is defined in Identity.EntityFramework.
    */
    public class UserManager: UserManager<User>
    {
        public UserManager(IUserStore<User> store)
            : base(store) { }
    }
    
}