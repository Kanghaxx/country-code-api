using Data.Identity.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Identity.Abstract
{
    public interface IUserFactory
    {
        IUserStore<User> CreateUserStore();
    }
}
