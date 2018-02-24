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
        [HttpGet]
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
                return Ok(items.AsCountryDTO(Url, false));
            }
        }
        

        /// <summary>
        /// Get set of countries by parameters provided
        /// </summary>
        /// <param name="search">Search parameters</param>
        /// <returns></returns>
        [HttpPost]
        [Route("find", Name = "FindCountry")]
        public async Task<IHttpActionResult> FindCountries([FromBody] SearchDTO search)
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var items = await rep.CountryRepository.FindAsync(search.IsoCodes);
                if (items == null)
                {
                    return NotFound();
                }
                return Ok(items.AsCountryDTO(Url));
            }
        }


        /// <summary>
        /// Get single country by ISO-code
        /// </summary>
        /// <param name="isoCode">ISO-code</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        [Route("{isoCode}", Name = "Country")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCountry(string isoCode, string culture = "")
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var item = await rep.CountryRepository.GetAsync(isoCode);
                if (item == null)
                {
                    return NotFound();
                }
                return Ok(item.AsCountryDTO(Url));
            }
        }


        /// <summary>
        /// Add new country
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("", Name = "PostCountry")]
        [Authorize]
        public async Task<IHttpActionResult> PostCountry([FromBody] CountryDetailsDTO country)
        {
            if (country == null)
            {
                return BadRequest($"Request content is empty");
            }
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var item = await rep.CountryRepository.GetAsync(country.IsoCode);
                if (item != null)
                {
                    return BadRequest("Country already exists");
                }

                var newCountry = new Country()
                {
                    IsoCode = country.IsoCode,
                    Name = country.Name,
                    DateFormat = country.DateFormat,
                    CallingCode = country.CallingCode,
                    Currencies = new List<Currency>(),
                    Organizations = new List<Organization>()
                };

                if (country.Currencies != null)
                {
                    foreach (var curDto in country.Currencies)
                    {
                        var cur = await rep.CurrencyRepository.GetAsync(curDto.IsoCode);
                        if (cur == null)
                        {
                            return BadRequest($"Currency {curDto.IsoCode} not found");
                        }
                        newCountry.Currencies.Add(cur);
                    }
                }

                if (country.Organizations != null)
                {
                    foreach (var orgDto in country.Organizations)
                    {
                        var org = await rep.OrganizationRepository.GetAsync(orgDto.Name);
                        if (org == null)
                        {
                            return BadRequest($"Organization {orgDto.Name} not found");
                        }
                        newCountry.Organizations.Add(org);
                    }
                }
            
                newCountry = rep.CountryRepository.Add(newCountry);

                await rep.CompleteAsync();
                
                var result = newCountry.AsCountryDTO(Url);
                var response = Request.CreateResponse(HttpStatusCode.Created, result);
                response.Headers.Location = new Uri(result.GetUrl);
                return ResponseMessage(response);
            }
        }


        /// <summary>
        /// Update country
        /// </summary>
        /// <param name="isoCode"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        [Route("{isoCode}", Name = "PutCountry")]
        [HttpPut]
        [Authorize]
        public async Task<IHttpActionResult> UpdateCountry(string isoCode, [FromBody] CountryDetailsDTO country)
        {
            if (country == null)
            {
                return BadRequest($"Request content is empty");
            }
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var item = await rep.CountryRepository.GetAsync(isoCode);
                if (item == null)
                {
                    return NotFound();
                }

                item.IsoCode = country.IsoCode;
                item.Name = country.Name;
                item.CallingCode = country.CallingCode;
                item.DateFormat = country.DateFormat;

                await rep.CompleteAsync();

                var result = item.AsCountryDTO(Url);
                var response = Request.CreateResponse(HttpStatusCode.Created, result);
                response.Headers.Location = new Uri(result.GetUrl);
                return ResponseMessage(response);
            }
        }

        /// <summary>
        /// Delete country
        /// </summary>
        /// <param name="isoCode"></param>
        /// <returns></returns>
        [Route("{isoCode}", Name = "DeleteCountry")]
        [HttpDelete]
        [Authorize]
        public async Task<IHttpActionResult> DeleteCountry(string isoCode)
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var item = await rep.CountryRepository.GetAsync(isoCode);
                if (item == null)
                {
                    return NotFound();
                }

                rep.CountryRepository.Remove(item);

                await rep.CompleteAsync();

                return Ok();
            }
        }

        /// <summary>
        /// Add currency to country
        /// </summary>
        /// <param name="isoCode"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        [Route("{isoCode}/currency", Name = "PostCurrencyUrl")]
        [HttpPost]
        [Authorize]
        public async Task<IHttpActionResult> AddCurrency(string isoCode, [FromBody] CurrencyDTO currency)
        {
            if (currency == null)
            {
                return BadRequest($"Request content is empty");
            }
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var item = await rep.CountryRepository.GetAsync(isoCode);
                if (item == null)
                {
                    return NotFound();
                }
                var cur = await rep.CurrencyRepository.GetAsync(currency.IsoCode);
                if (cur == null)
                {
                    return BadRequest($"Currency {currency.IsoCode} not found");
                }

                item.Currencies.Add(cur);

                await rep.CompleteAsync();

                return Ok(item.AsCountryDTO(Url));
            }
        }

        /// <summary>
        /// Remove currency to country
        /// </summary>
        /// <param name="isoCode"></param>
        /// <param name="isoCodeCurrency"></param>
        /// <returns></returns>
        [Route("{isoCode}/currency/{isoCodeCurrency}")]
        [HttpDelete]
        [Authorize]
        public async Task<IHttpActionResult> RemoveCurrency(string isoCode, string isoCodeCurrency)
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var item = await rep.CountryRepository.GetAsync(isoCode);
                if (item == null)
                {
                    return NotFound();
                }
                var cur = item.Currencies.Where(c => c.IsoCode == isoCodeCurrency)
                    .FirstOrDefault();
                if (cur == null)
                {
                    return BadRequest($"Currency {isoCodeCurrency} not found");
                }

                item.Currencies.Remove(cur);

                await rep.CompleteAsync();

                return Ok(item.AsCountryDTO(Url));
            }
        }

    }
}
