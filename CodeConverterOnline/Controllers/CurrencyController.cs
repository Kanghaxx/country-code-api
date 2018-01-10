using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using CodeConverterOnline.Models;
using Data.Abstract;
using Data.Repository;


namespace CodeConverterOnline.Controllers
{
    /// <summary>
    /// Currencies
    /// </summary>
    [RoutePrefix("api/currency")]
    public class CurrencyController : ControllerBase
    {
        public CurrencyController(IStoreFactory store):base (store)
        {
        }

        /// <summary>
        /// Get all currencies
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IHttpActionResult GetCurrencies()
        {
            using (var rep = new UnitOfWork())
            {
                var c = rep.CurrencyRepository
                    .Get()
                    .AsCurrencyDTO();
                if (c == null)
                {
                    return NotFound();
                }
                return Ok(c);
            }
        }


        /// <summary>
        /// Get single currency by ISO-code
        /// </summary>
        /// <param name="isoCode">ISO-code</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        [Route("{isoCode}")]
        public IHttpActionResult GetCurrency(string isoCode, string culture = "")
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var c = rep.CurrencyRepository.Get(isoCode)
                    .AsCurrencyDTO();
                if (c == null)
                {
                    return NotFound();
                }
                return Ok(c);
            }
        }


        /// <summary>
        /// Get set of currencies by parameters provided
        /// </summary>
        /// <param name="search">Search parameters</param>
        /// <returns></returns>
        [HttpPost]
        [Route("find")]
        public IHttpActionResult FindCountries([FromBody] SearchDTO search)
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var c = rep.CurrencyRepository
                    .Find(search.IsoCodes)
                    .AsCurrencyDTO();
                if (c == null)
                {
                    return NotFound();
                }
                return Ok(c);
            }
        }
    }
}