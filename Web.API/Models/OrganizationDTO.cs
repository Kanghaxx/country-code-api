using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.API.Models
{
    public class OrganizationDTO
    {
        public string Name { get; set; }
        public string GetUrl { get; set; }
    }

    public class OrganizationDetailsDTO: OrganizationDTO
    {
        public string PostUrl { get; set; }
        public string PutUrl { get; set; }
        public string DeleteUrl { get; set; }

        public string Description { get; set; }

        public IEnumerable<CountryDTO> Countries { get; set; }
    }
}