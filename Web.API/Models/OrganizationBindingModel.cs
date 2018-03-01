using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.API.Models
{
    public class OrganizationBindingModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Countries { get; set; }
    }
}