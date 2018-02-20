using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;

using Web.API.Models;
using Data.Common.Abstract;
using Data.Repository;

namespace Web.API.Controllers
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
        public async Task<IHttpActionResult> GetCurrencies()
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var items = await rep.CurrencyRepository.GetAsync();
                if (items == null)
                {
                    return NotFound();
                }
                return Ok(items.AsCurrencyDTO());
            }
        }


        /// <summary>
        /// Get single currency by ISO-code
        /// </summary>
        /// <param name="isoCode">ISO-code</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        [Route("{isoCode}")]
        public async Task<IHttpActionResult> GetCurrency(string isoCode, string culture = "")
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var item = await rep.CurrencyRepository.GetAsync(isoCode);
                if (item == null)
                {
                    return NotFound();
                }
                return Ok(item.AsCurrencyDTO());
            }
        }


        /// <summary>
        /// Get set of currencies by parameters provided
        /// </summary>
        /// <param name="search">Search parameters</param>
        /// <returns></returns>
        [HttpPost]
        [Route("find")]
        public async Task<IHttpActionResult> FindCurrencies([FromBody] SearchDTO search)
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var items = await rep.CurrencyRepository.FindAsync(search.IsoCodes);
                if (items == null)
                {
                    return NotFound();
                }
                return Ok(items.AsCurrencyDTO());
            }
        }
    }
}