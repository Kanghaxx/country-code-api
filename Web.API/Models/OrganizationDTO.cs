using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.API.Models
{
    public class OrganizationDTO
    {
        public string Name { get; set; }
    }

    public class OrganizationDetailsDTO: OrganizationDTO
    {
        public string Description { get; set; }

        public IEnumerable<CountryDTO> Countries { get; set; }
    }
}