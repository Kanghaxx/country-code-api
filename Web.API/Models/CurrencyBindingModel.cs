using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.API.Models
{
    public class CurrencyBindingModel
    {
        public string Name { get; set; }
        public string IsoCode { get; set; }
        public IEnumerable<string> Countries { get; set; }
    }
}