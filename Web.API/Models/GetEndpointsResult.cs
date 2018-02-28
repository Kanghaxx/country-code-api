using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.API.Models
{
    public class GetEndpointsResult
    {
        public IDictionary<string, string> Endpoints { get; set; }
    }
}