using CodeConverterOnline.Models;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CodeConverterOnline.Controllers
{
    /// <summary>
    /// Coutries
    /// </summary>
    [RoutePrefix("api/currency")]
    public class CurrencyController : ApiController
    {
        /// <summary>
        /// Get all countries
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult GetCurrencies()
        {
            using (var rep = new UnitOfWork())
            {
                var c = rep.CountryRepository.GetCountries()
                    .AsCountryDTO();
                if (c == null)
                {
                    return NotFound();
                }
                return Ok(c);
            }
        }
    }
}