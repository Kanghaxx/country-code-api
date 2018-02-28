using Data.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.API.Models
{
    public class GetOrganizationsResult
    {
        public string PostURL { get; set; }
        public IEnumerable<OrganizationDTO> Organizations { get; set; }
    }
}