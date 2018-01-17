using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

using Data.Common.Model;
using Web.API.Models;

namespace Testing.Web.API.Model
{
    [TestCategory("Web.Model.Extensions")]
    [TestClass]
    public class CountryExtensionsTests
    {
        [TestMethod]
        public void AsCountryDTO_Returns_DtoWithDetails()
        {
            Currency cur1 = new Currency()
            {
                IsoCode = "XX",
                Name = "Currency 1",
            };

            Currency cur2 = new Currency()
            {
                IsoCode = "YY",
                Name = "Currency 2",
            };

            Country c = new Country()
            {
                CountryId = 1,
                IsoCode = "AA",
                Name = "Country",
                Currencies = new List<Currency>()
                {
                    cur1,
                    cur2,
                }
            };

            cur1.Countries = new List<Country>() { c };
            cur2.Countries = new List<Country>() { c };

            // Act
            CountryDTO dto = c.AsCountryDTO(details: true);
            
            Assert.AreEqual(dto.IsoCode, c.IsoCode);
            Assert.AreEqual(dto.Name, c.Name);
            Assert.IsInstanceOfType(dto, typeof(CountryDetailsDTO));
            Assert.IsNotNull((dto as CountryDetailsDTO).Currencies);
            Assert.IsTrue((dto as CountryDetailsDTO).Currencies.Count() == 2);

            var curDto1 = (dto as CountryDetailsDTO)
                .Currencies
                .Where(x => x.IsoCode == cur1.IsoCode);
            Assert.IsNotNull(curDto1);
            Assert.IsNotInstanceOfType(curDto1, typeof(CurrencyDetailsDTO));

            var curDto2 = (dto as CountryDetailsDTO)
                .Currencies
                .Where(x => x.IsoCode == cur2.IsoCode);
            Assert.IsNotNull(curDto2);
            Assert.IsNotInstanceOfType(curDto2, typeof(CurrencyDetailsDTO));
        }

        [TestMethod]
        public void AsCountryDTO_Returns_DtoWithNoDetailsIfNull()
        {
            Country c = new Country()
            {
                CountryId = 1,
                IsoCode = "AA",
                Name = "Country",
                Currencies = null
            };

            // Act
            CountryDTO dto = c.AsCountryDTO(details: true);

            Assert.AreEqual(dto.IsoCode, c.IsoCode);
            Assert.AreEqual(dto.Name, c.Name);
            Assert.IsInstanceOfType(dto, typeof(CountryDetailsDTO));
            Assert.IsNotNull((dto as CountryDetailsDTO).Currencies);
            Assert.IsTrue((dto as CountryDetailsDTO).Currencies.Count() == 0);
        }

    }
}
