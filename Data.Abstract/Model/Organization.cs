using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Common.Model
{
    public class Organization
    {
        public long OrganizationId { get; set; }
        
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }
        
        public string Description { get; set; }

        public ICollection<Country> Countries { get; set; }
    }
}
