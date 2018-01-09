using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeConverterOnline.Models
{
    public class CountryFindDTO
    {
        public string[] IsoCodes { get; set; }
        public string[] CountryNames { get; set; }
    }
}