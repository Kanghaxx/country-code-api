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
    public class CountryController : ApiController
    {
        private IStoreFactory Store { get; set; }
        

        public CountryController(IStoreFactory store)
        {
            Store = store;
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
                var c = rep.CountryRepository.GetCountries()
                    .AsCountryDTO();
                if (c == null)
                {
                    return NotFound();
                }
                return Ok(c);
            }
        }

        [HttpPost]
        [Route("find")]
        public IHttpActionResult FindCountries([FromBody] CountryFindDTO search)
        {
            // todo get array of isoCodes from body in JSON
            // in practice a lot of infrastructure pieces do drop the body of a GET
            // so use POST
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var c = rep.CountryRepository.FindCountries(search.IsoCodes, search.CountryNames)
                    .AsCountryDTO();
                if (c == null)
                {
                    return NotFound();
                }
                return Ok(c);
            }
        }

        /// <summary>
        /// Get country name by ISO-code
        /// </summary>
        /// <param name="isoCode">ISO-code</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        [Route("{isoCode}")]
        public IHttpActionResult GetCountry(string isoCode, string culture = "")
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var c = rep.CountryRepository.GetCountry(isoCode)
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
                var c = rep.CountryRepository.GetCountry(isoCode)
                    .AsCountryDTO();
                if (c == null)
                {
                    return NotFound();
                }

                c.IsoCode = country.IsoCode;
                c.CountryName = country.Name;

                rep.Complete();

                return Ok(c);
            }
        }

        // POST api/values
        //public void Post([FromBody]string value)
        //{
        //}
        //
        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}
        //
        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
    }
}
