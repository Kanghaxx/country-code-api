using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Identity.Model
{
    public class User : IdentityUser
    {
        public User() { }

        public User(string email) : base(email)
        {
            UserName = email;
        }
    }
}
