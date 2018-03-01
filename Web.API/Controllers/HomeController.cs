using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Web.API.Models;

namespace Web.API.Controllers
{
    [RoutePrefix("api")]
    public class HomeController : ApiController
    {
        /// <summary>
        /// Get API endpoints
        /// </summary>
        [HttpGet]
        [Route("", Name = "GetEndpoints")]
        [ResponseType(typeof(GetEndpointsResult))]
        public IHttpActionResult GetEndpoints()
        {
            var result = new GetEndpointsResult()
            {
                Endpoints = new Dictionary<string, string>()
                {
                    { "GetCountries", Url.Link("GetCountries", null) },
                    { "GetCurrencies", Url.Link("GetCurrencies", null) },
                    { "GetOrganizations", Url.Link("GetOrganizations", null) },
                    { "GetOAuthToken", Url.Content("~/api/login")},
                    { "GetHelpHtml", Url.Content("~/api/help/index")},
                }
            };

            return Ok(result);
        }
    }
}
