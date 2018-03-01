using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.API.Models
{
    public class CountryBindingModel
    {
        public string Name { get; set; }
        public string IsoCode { get; set; }
        public int? CallingCode { get; set; }
        public string DateFormat { get; set; }
        public IEnumerable<string> Currencies { get; set; }
        public IEnumerable<string> Organizations { get; set; }
    }
}