using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

using Data.Repository;
using Data.Common.Model;
using Data.Common.Abstract;
using Web.API.Models;

namespace Web.API.Controllers
{
    /// <summary>
    /// Coutries
    /// </summary>
    [RoutePrefix("api/country")]
    public class CountryController : ControllerBase
    {
        public CountryController(IStoreFactory store):base (store)
        {
        }


        /// <summary>
        /// Get all countries
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public async Task<IHttpActionResult> GetCountries()
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var items = await rep.CountryRepository.GetAsync();
                if (items == null)
                {
                    return NotFound();
                }
                return Ok(items.AsCountryDTO());
            }
        }


        /// <summary>
        /// Get set of countries by parameters provided
        /// </summary>
        /// <param name="search">Search parameters</param>
        /// <returns></returns>
        [HttpPost]
        [Route("find")]
        public async Task<IHttpActionResult> FindCountries([FromBody] SearchDTO search)
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var items = await rep.CountryRepository.FindAsync(search.IsoCodes);
                if (items == null)
                {
                    return NotFound();
                }
                return Ok(items.AsCountryDTO());
            }
        }


        /// <summary>
        /// Get single country by ISO-code
        /// </summary>
        /// <param name="isoCode">ISO-code</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        [Route("{isoCode}")]
        public async Task<IHttpActionResult> GetCountry(string isoCode, string culture = "")
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var item = await rep.CountryRepository.GetAsync(isoCode);
                if (item == null)
                {
                    return NotFound();
                }
                return Ok(item.AsCountryDTO());
            }
        }
        

        /// <summary>
        /// Update country
        /// </summary>
        /// <param name="isoCode"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        [Route("{isoCode}/update")]
        [HttpPost]
        [Authorize]
        public async Task<IHttpActionResult> UpdateCountry(string isoCode, [FromBody] Country country)
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var item = await rep.CountryRepository.GetAsync(isoCode);
                if (item == null)
                {
                    return NotFound();
                }

                item.IsoCode = country.IsoCode;
                item.Name = country.Name;

                await rep.CompleteAsync();

                return Ok(item.AsCountryDTO());
            }
        }
        
        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
    }
}
