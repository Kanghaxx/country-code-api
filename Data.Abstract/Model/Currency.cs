using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Common.Model
{
    public class Currency
    {
        public long CurrencyId { get; set; }

        [Required]
        [MaxLength(3)]
        [Description("ISO 4217")]
        public string IsoCode { get; set; }

        [Required]
        [MaxLength(300)]
        public string Name { get; set; }
        
        public ICollection<Country> Countries { get; set; }
    }
}
