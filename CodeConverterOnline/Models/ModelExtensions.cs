using Data.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.API.Models
{
    public static class CountryExtensions
    {
        public static IEnumerable<CountryDTO> AsCountryDTO(this IEnumerable<Country> data, bool details = true)
        {
            var result = new List<CountryDTO>();
            foreach (Country country in data)
            {
                result.Add(country.AsCountryDTO(details));
            }
            return result;
        }


        public static CountryDTO AsCountryDTO(this Country country, bool details = true)
        {
            CountryDTO dto;
            if (details)
            {
                dto = new CountryDetailsDTO()
                {
                    Currencies = country.Currencies == null?
                        new List<CurrencyDTO>()
                        : country.Currencies.AsCurrencyDTO(false),

                    Organizations = country.Organizations == null ?
                        new List<OrganizationDTO>()
                        : country.Organizations.AsDTO(false),
                    CallingCode = country.CallingCode,
                    DateFormat = country.DateFormat
                };
            }
            else
            {
                dto = new CountryDTO();
            }
            dto.IsoCode = country.IsoCode;
            dto.Name = country.Name;

            return dto;
        }
    }


    public static class CurrencyExtensions
    {
        public static IEnumerable<CurrencyDTO> AsCurrencyDTO(this IEnumerable<Currency> data, bool details = true)
        {
            var result = new List<CurrencyDTO>();
            foreach (Currency c in data)
            {
                result.Add(c.AsCurrencyDTO(details));
            }
            return result;
        }

        public static CurrencyDTO AsCurrencyDTO(this Currency currency, bool details = true)
        {
            CurrencyDTO dto;
            if (details)
            {
                dto = new CurrencyDetailsDTO()
                {
                    Countries = currency.Countries == null?
                        new List<CountryDTO>()
                        : currency.Countries.AsCountryDTO(false)
                };
            }
            else
            {
                dto = new CurrencyDTO();
            }
            dto.IsoCode = currency.IsoCode;
            dto.Name = currency.Name;

            return dto;
        }
    }


    public static class OrganizationExtensions
    {
        public static IEnumerable<OrganizationDTO> AsDTO(this IEnumerable<Organization> data, bool details = true)
        {
            var result = new List<OrganizationDTO>();
            foreach (Organization item in data)
            {
                result.Add(item.AsDTO(details));
            }
            return result;
        }


        public static OrganizationDTO AsDTO(this Organization item, bool details = true)
        {
            OrganizationDTO dto;
            if (details)
            {
                dto = new OrganizationDetailsDTO()
                {
                    Countries = item.Countries == null ?
                        new List<CountryDTO>()
                        : item.Countries.AsCountryDTO(false),
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