using Data.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeConverterOnline.Models
{
    public static class CountryExtensions
    {
        public static IEnumerable<CountryDTO> AsCountryDTO(this IEnumerable<Country> data)
        {
            var result = new List<CountryDTO>();
            foreach (Country country in data)
            {
                var countryDTO = country.AsCountryDTO();
                countryDTO.Currencies = country.Currencies.AsCurrencyDTO();
                result.Add(countryDTO);
            }
            return result;
        }

        public static CountryDTO AsCountryDTO(this Country country)
        {
            return new CountryDTO()
            {
                IsoCode = country.IsoCode,
                CountryName = country.Name,
                Currencies = country.Currencies.AsCurrencyDTO()
        };
        }
    }


    public static class CurrencyExtensions
    {
        public static IEnumerable<CurrencyDTO> AsCurrencyDTO(this IEnumerable<Currency> data)
        {
            var result = new List<CurrencyDTO>();
            foreach (Currency c in data)
            {
                var dto = c.AsCurrencyDTO();
                result.Add(dto);
            }
            return result;
        }

        public static CurrencyDTO AsCurrencyDTO(this Currency country)
        {
            return new CurrencyDTO()
            {
                IsoCode = country.IsoCode,
                CountryName = country.Name
            };
        }
    }
}