using CodeConverterOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Data.Repository;
using Data.Common.Model;
using Data.Abstract;

namespace CodeConverterOnline.Controllers
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
        public IHttpActionResult GetCountries()
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var c = rep.CountryRepository
                    .Get()
                    .AsCountryDTO();
                if (c == null)
                {
                    return NotFound();
                }
                return Ok(c);
            }
        }


        /// <summary>
        /// Get set of countries by parameters provided
        /// </summary>
        /// <param name="search">Search parameters</param>
        /// <returns></returns>
        [HttpPost]
        [Route("find")]
        public IHttpActionResult FindCountries([FromBody] SearchDTO search)
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var c = rep.CountryRepository
                    .Find(search.IsoCodes)
                    .AsCountryDTO();
                if (c == null)
                {
                    return NotFound();
                }
                return Ok(c);
            }
        }


        /// <summary>
        /// Get single country by ISO-code
        /// </summary>
        /// <param name="isoCode">ISO-code</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        [Route("{isoCode}")]
        public IHttpActionResult GetCountry(string isoCode, string culture = "")
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var c = rep.CountryRepository.Get(isoCode)
                    .AsCountryDTO();
                if (c == null)
                {
                    return NotFound();
                }
                return Ok(c);
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
        public IHttpActionResult UpdateCountry(string isoCode, [FromBody] Country country)
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var c = rep.CountryRepository.Get(isoCode)
                    .AsCountryDTO();
                if (c == null)
                {
                    return NotFound();
                }

                c.IsoCode = country.IsoCode;
                c.Name = country.Name;

                rep.Complete();

                return Ok(c);
            }
        }
        
        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
    }
}
