using Data.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Routing;

namespace Web.API.Models
{
    public static class CountryExtensions
    {
        public static IEnumerable<CountryDTO> AsCountryDTO(this IEnumerable<Country> data, 
            UrlHelper urlHelper, bool details = true)
        {
            var result = new List<CountryDTO>();
            foreach (Country country in data)
            {
                result.Add(country.AsCountryDTO(urlHelper, details));
            }
            return result;
        }


        public static CountryDTO AsCountryDTO(this Country country, 
            UrlHelper urlHelper, bool details = true)
        {
            CountryDTO dto;
            if (details)
            {
                dto = new CountryDetailsDTO()
                {
                    Currencies = country.Currencies == null?
                        new List<CurrencyDTO>()
                        : country.Currencies.AsCurrencyDTO(urlHelper, false, true),
                    Organizations = country.Organizations == null ?
                        new List<OrganizationDTO>()
                        : country.Organizations.AsDTO(urlHelper, false),
                    CallingCode = country.CallingCode,
                    DateFormat = country.DateFormat,
                    PostUrl = urlHelper == null ? ""
                        : urlHelper.Link("PostCountry", null),
                    PutUrl = urlHelper == null ? ""
                        : urlHelper.Link("PutCountry", new { isoCode = country.IsoCode }),
                    DeleteUrl = urlHelper == null ? ""
                        : urlHelper.Link("DeleteCountry", new { isoCode = country.IsoCode }),
                    PostCurrencyUrl = urlHelper == null ? ""
                        : urlHelper.Link("PostCurrencyUrl", new { isoCode = country.IsoCode }),
                };
            }
            else
            {
                dto = new CountryDTO();
            }
            dto.IsoCode = country.IsoCode;
            dto.Name = country.Name;
            dto.GetUrl = urlHelper == null ? "" 
                : urlHelper.Link("Country", new { isoCode = country.IsoCode });

            return dto;
        }
    }


    public static class CurrencyExtensions
    {
        public static IEnumerable<CurrencyDTO> AsCurrencyDTO(this IEnumerable<Currency> data, 
            UrlHelper urlHelper, bool details = true, bool slave = false)
        {
            var result = new List<CurrencyDTO>();
            foreach (Currency c in data)
            {
                result.Add(c.AsCurrencyDTO(urlHelper, details, slave));
            }
            return result;
        }

        public static CurrencyDTO AsCurrencyDTO(this Currency currency, 
            UrlHelper urlHelper, bool details = true, bool slave = false)
        {
            CurrencyDTO dto;
            if (details)
            {
                dto = new CurrencyDetailsDTO()
                {
                    PostUrl = urlHelper == null ? ""
                        : urlHelper.Link("PostCurrency", null),
                    PutUrl = urlHelper == null ? ""
                        : urlHelper.Link("PutCurrency", new { isoCode = currency.IsoCode }),
                    DeleteUrl = urlHelper == null ? ""
                        : urlHelper.Link("DeleteCurrency", new { isoCode = currency.IsoCode }),
                    Countries = currency.Countries == null?
                        new List<CountryDTO>()
                        : currency.Countries.AsCountryDTO(urlHelper, false)
                };
            }
            else
            {
                if (slave)
                {
                    dto = new CountryCurrencyDTO()
                    {
                        DeleteCurrencyUrl = urlHelper == null ? "" :
                            urlHelper.Link("DeleteCurrencyUrl", new {isoCodeCurrency = currency.IsoCode})
                    };
                }
                else
                {
                    dto = new CurrencyDTO();
                }
            }
            dto.IsoCode = currency.IsoCode;
            dto.Name = currency.Name;
            dto.GetUrl = urlHelper == null ? "" : 
                urlHelper.Link("Currency", new { isoCode = currency.IsoCode });

            return dto;
        }
    }


    public static class OrganizationExtensions
    {
        public static IEnumerable<OrganizationDTO> AsDTO(
            this IEnumerable<Organization> data, 
            UrlHelper urlHelper, bool details = true)
        {
            var result = new List<OrganizationDTO>();
            foreach (Organization item in data)
            {
                result.Add(item.AsDTO(urlHelper, details));
            }
            return result;
        }


        public static OrganizationDTO AsDTO(this Organization item, 
            UrlHelper urlHelper, bool details = true)
        {
            OrganizationDTO dto;
            if (details)
            {
                dto = new OrganizationDetailsDTO()
                {
                    Countries = item.Countries == null ?
                        new List<CountryDTO>()
                        : item.Countries.AsCountryDTO(urlHelper, false),
                    Description = item.Description
                };
            }
            else
            {
                dto = new OrganizationDTO();
            }
            dto.Name = item.Name;

            return dto;
        }
    }

}