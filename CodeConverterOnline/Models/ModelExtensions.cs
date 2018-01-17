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
                        : country.Currencies.AsCurrencyDTO(false)
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
}