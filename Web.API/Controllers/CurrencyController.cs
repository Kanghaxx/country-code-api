using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;

using Web.API.Models;
using Data.Common.Abstract;
using Data.Repository;
using Data.Common.Model;
using System.Net.Http;
using System.Net;
using System.Web.Http.Description;

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
        [HttpGet]
        [Route("", Name = "GetCurrencies")]
        [ResponseType(typeof(GetCurrenciesResult))]
        public async Task<IHttpActionResult> GetCurrencies()
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var items = await rep.CurrencyRepository.GetAsync();
                if (items == null)
                {
                    return NotFound();
                }

                var result = new GetCurrenciesResult()
                {
                    PostURL = Url.Link("PostCurrency", null),
                    Currencies = items.AsCurrencyDTO(Url, false)
                };

                return Ok(result);
            }
        }


        /// <summary>
        /// Get a single currency by ISO-code
        /// </summary>
        /// <param name="isoCode">ISO-code</param>
        [HttpGet]
        [Route("{isoCode}", Name = "Currency")]
        [ResponseType(typeof(CurrencyDetailsDTO))]
        public async Task<IHttpActionResult> GetCurrency(string isoCode)
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var item = await rep.CurrencyRepository.GetAsync(isoCode);
                if (item == null)
                {
                    return NotFound();
                }
                return Ok(item.AsCurrencyDTO(Url));
            }
        }


        /// <summary>
        /// Get a set of currencies by parameters provided
        /// </summary>
        /// <param name="search">Search parameters</param>
        [HttpPost]
        [Route("find", Name = "FindCurrency")]
        [ResponseType(typeof(IEnumerable<CurrencyDTO>))]
        public async Task<IHttpActionResult> FindCurrencies([FromBody] SearchDTO search)
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var items = await rep.CurrencyRepository.FindAsync(search.IsoCodes);
                if (items == null)
                {
                    return NotFound();
                }
                return Ok(items.AsCurrencyDTO(Url, false));
            }
        }


        /// <summary>
        /// Add a new currency
        /// </summary>
        /// <param name="currency"></param>
        [HttpPost]
        [Route("", Name = "PostCurrency")]
        [Authorize]
        [ResponseType(typeof(CurrencyDetailsDTO))]
        public async Task<IHttpActionResult> PostCurrency([FromBody] CurrencyDetailsDTO currency)
        {
            if (currency == null)
            {
                return BadRequest($"Request content is empty");
            }
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var item = await rep.CurrencyRepository.GetAsync(currency.IsoCode);
                if (item != null)
                {
                    return BadRequest("Currency already exists");
                }

                var newCurrency = new Currency()
                {
                    IsoCode = currency.IsoCode.ToUpper(),
                    Name = currency.Name,
                    Countries = new List<Country>(),
                };

                if (currency.Countries != null)
                {
                    foreach (var cDto in currency.Countries)
                    {
                        var c = await rep.CountryRepository.GetAsync(cDto.IsoCode);
                        if (c == null)
                        {
                            return BadRequest($"Country {cDto.IsoCode} not found");
                        }
                        newCurrency.Countries.Add(c);
                    }
                }
                
                newCurrency = rep.CurrencyRepository.Add(newCurrency);

                await rep.CompleteAsync();

                var result = newCurrency.AsCurrencyDTO(Url);
                var response = Request.CreateResponse(HttpStatusCode.Created, result);
                response.Headers.Location = new Uri(result.GetUrl);
                return ResponseMessage(response);
            }
        }


        /// <summary>
        /// Update a currency
        /// </summary>
        /// <param name="isoCode"></param>
        /// <param name="currency"></param>
        [ResponseType(typeof(CurrencyDetailsDTO))]
        [Route("{isoCode}", Name = "PutCurrency")]
        [HttpPut]
        [Authorize]
        public async Task<IHttpActionResult> UpdateCurrency(string isoCode, 
            [FromBody] CurrencyDetailsDTO currency)
        {
            if (currency == null)
            {
                return BadRequest($"Request content is empty");
            }
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var item = await rep.CurrencyRepository.GetAsync(isoCode);
                if (item == null)
                {
                    return NotFound();
                }

                item.IsoCode = currency.IsoCode.ToUpper();
                item.Name = currency.Name;

                await rep.CompleteAsync();

                var result = item.AsCurrencyDTO(Url);
                var response = Request.CreateResponse(HttpStatusCode.Created, result);
                response.Headers.Location = new Uri(result.GetUrl);
                return ResponseMessage(response);
            }
        }


        /// <summary>
        /// Delete a currency
        /// </summary>
        /// <param name="isoCode"></param>
        [Route("{isoCode}", Name = "DeleteCurrency")]
        [HttpDelete]
        [Authorize]
        public async Task<IHttpActionResult> DeleteCurrency(string isoCode)
        {
            using (IUnitOfWork rep = Store.CreateUnitOfWork())
            {
                var item = await rep.CurrencyRepository.GetAsync(isoCode);
                if (item == null)
                {
                    return NotFound();
                }

                rep.CurrencyRepository.Remove(item);

                await rep.CompleteAsync();

                return Ok();
            }
        }
    }
}