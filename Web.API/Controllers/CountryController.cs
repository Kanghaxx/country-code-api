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
using System.Web.Http.Description;

namespace Web.API.Controllers
{
    /// <summary>
    /// Country API
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
        [HttpGet]
        [Route("", Name = "GetCountries")]
        [ResponseType(typeof(GetCountriesResult))]
        public async Task<IHttpActionResult> GetCountries()
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var items = await rep.CountryRepository.GetAsync();
                if (items == null)
                {
                    return NotFound();
                }

                var result = new GetCountriesResult()
                {
                    PostURL = Url.Link("PostCountry", null),
                    Countries = items.AsCountryDTO(Url, false)
                };

                return Ok(result);
            }
        }
        

        /// <summary>
        /// Get set of countries by parameters provided
        /// </summary>
        /// <param name="search">Search parameters</param>
        [HttpPost]
        [Route("find", Name = "FindCountry")]
        [ResponseType(typeof(IEnumerable<CountryDTO>))]
        public async Task<IHttpActionResult> FindCountries([FromBody] SearchBindingModel search)
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var items = await rep.CountryRepository.FindAsync(search.IsoCodes);
                if (items == null)
                {
                    return NotFound();
                }
                return Ok(items.AsCountryDTO(Url, false));
            }
        }


        /// <summary>
        /// Get a single country by ISO-code
        /// </summary>
        /// <param name="isoCode">ISO-code</param>
        [Route("{isoCode}", Name = "Country")]
        [HttpGet]
        [ResponseType(typeof(CountryDetailsDTO))]
        public async Task<IHttpActionResult> GetCountry(string isoCode)
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
        /// Add a new country
        /// </summary>
        /// <param name="country"></param>
        [HttpPost]
        [Route("", Name = "PostCountry")]
        [Authorize]
        [ResponseType(typeof(CountryDetailsDTO))]
        public async Task<IHttpActionResult> PostCountry([FromBody] CountryBindingModel country)
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
                    IsoCode = country.IsoCode.ToUpper(),
                    Name = country.Name,
                    DateFormat = country.DateFormat,
                    CallingCode = country.CallingCode,
                    Currencies = new List<Currency>(),
                    Organizations = new List<Organization>()
                };

                if (country.Currencies != null)
                {
                    foreach (var isoCode in country.Currencies)
                    {
                        var cur = await rep.CurrencyRepository.GetAsync(isoCode);
                        if (cur == null)
                        {
                            return BadRequest($"Currency {isoCode} not found");
                        }
                        newCountry.Currencies.Add(cur);
                    }
                }

                if (country.Organizations != null)
                {
                    foreach (var isoCode in country.Organizations)
                    {
                        var org = await rep.OrganizationRepository.GetAsync(isoCode);
                        if (org == null)
                        {
                            return BadRequest($"Organization {isoCode} not found");
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
        /// Update a country
        /// </summary>
        /// <param name="isoCode"></param>
        /// <param name="country"></param>
        [Route("{isoCode}", Name = "PutCountry")]
        [HttpPut]
        [Authorize]
        [ResponseType(typeof(CountryDetailsDTO))]
        public async Task<IHttpActionResult> UpdateCountry(string isoCode, [FromBody] CountryBindingModel country)
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

                item.IsoCode = country.IsoCode.ToUpper();
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
        /// Delete a country
        /// </summary>
        /// <param name="isoCode"></param>
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
        /// Add an existing currency to the country
        /// </summary>
        /// <param name="isoCode"></param>
        /// <param name="isoCodeCurrency"></param>
        [Route("{isoCode}/currency", Name = "PostCurrencyUrl")]
        [HttpPost]
        [Authorize]
        [ResponseType(typeof(CountryDetailsDTO))]
        public async Task<IHttpActionResult> AddCurrency(string isoCode, [FromBody] string isoCodeCurrency)
        {
            if (isoCodeCurrency == null)
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
                var cur = await rep.CurrencyRepository.GetAsync(isoCodeCurrency);
                if (cur == null)
                {
                    return BadRequest($"Currency {isoCodeCurrency} not found");
                }

                item.Currencies.Add(cur);

                await rep.CompleteAsync();

                return Ok(item.AsCountryDTO(Url));
            }
        }

        /// <summary>
        /// Remove an existing currency from the country
        /// </summary>
        /// <param name="isoCode"></param>
        /// <param name="isoCodeCurrency"></param>
        [Route("{isoCode}/currency/{isoCodeCurrency}", Name = "DeleteCurrencyUrl")]
        [HttpDelete]
        [Authorize]
        [ResponseType(typeof(CountryDetailsDTO))]
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
