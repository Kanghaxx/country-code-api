using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Common.Model
{
    public class Country
    {
        public long CountryId { get; set; }

        /// <summary>
        /// Country code (ISO 3166-1 Alpha-2)
        /// </summary>
        [Required]
        [MaxLength(2)]
        public string IsoCode { get; set; }

        //[Description("ISO 3166-1 numeric")]
        //public int IsoCodeNumber { get; set; }
        //
        //[MaxLength(3)]
        //[Description("ISO 3166-1 Alpha-3")]
        //public string IsoCode_alpha3 { get; set; }
        //
        //[MaxLength(10)]
        //[Description("ISO 3166-2")]
        //public string IsoCode2 { get; set; }

        /// <summary>
        /// Country name
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        /// <summary>
        /// Calling code
        /// </summary>
        public int? CallingCode { get; set; }


        /// <summary>
        /// Date format
        /// </summary>
        [MaxLength(100)]
        public string DateFormat { get; set; }

        public ICollection<Currency> Currencies { get; set; }
    }
}
