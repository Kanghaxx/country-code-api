using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.API.Models
{
    public class CurrencyDTO
    {
        public string Name { get; set; }
        public string IsoCode { get; set; }
        public string GetUrl { get; set; }
    }

    public class CountryCurrencyDTO : CurrencyDTO
    {
        public string DeleteCurrencyUrl { get; set; }
    }

    public class CurrencyDetailsDTO : CurrencyDTO
    {
        public string PostUrl { get; set; }
        public string PutUrl { get; set; }
        public string DeleteUrl { get; set; }

        public IEnumerable<CountryDTO> Countries { get; set; }
    }
}