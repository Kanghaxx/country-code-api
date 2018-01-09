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
            foreach (Country value in data)
            {
                result.Add(value.AsCountryDTO());
            }
            return result;
        }

        public static CountryDTO AsCountryDTO(this Country country)
        {
            return new CountryDTO()
            {
                IsoCode = country.IsoCode,
                CountryName = country.Name
            };
        }
    }
}