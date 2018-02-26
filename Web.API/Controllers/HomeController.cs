using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Web.API.Models;

namespace Web.API.Controllers
{
    [RoutePrefix("api")]
    public class HomeController : ApiController
    {
        /// <summary>
        /// Get endpoints
        /// </summary>
        [HttpGet]
        [Route("", Name = "GetEndpoints")]
        public IHttpActionResult Get()
        {
            var result = new HomeResponseModel()
            {
                Endpoints = new Dictionary<string, string>()
                {
                    {"GetCountries", Url.Link("GetCountries", null) },
                    {"GetCurrencies", Url.Link("GetCurrencies", null) },
                    {"GetHelpHtml", Url.Content("~/api/help/index")},
                }
            };

            return Ok(result);
        }
    }
}
