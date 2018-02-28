using Data.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.API.Models
{
    public class GetCurrenciesResult
    {
        public string PostURL { get; set; }
        public IEnumerable<CurrencyDTO> Currencies { get; set; }
    }
}