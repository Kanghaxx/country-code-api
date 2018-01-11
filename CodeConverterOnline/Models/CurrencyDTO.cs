using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeConverterOnline.Models
{
    public class CurrencyDTO
    {
        public string Name { get; set; }
        public string IsoCode { get; set; }
    }

    public class CurrencyDetailsDTO : CurrencyDTO
    {
        public IEnumerable<CountryDTO> Countries { get; set; }
    }
}