using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using Data.Common.Abstract;

namespace Web.API.Controllers
{
    public class ControllerBase : ApiController
    {
        protected IStoreFactory Store { get; set; }

        public ControllerBase(IStoreFactory store)
        {
            Store = store;
        }
    }
}