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
    public class CurrencyExtensionsTests
    {
        [TestMethod]
        public void AsCurrencyDTO_Returns_DtoWithDetails()
        {
            Country c1 = new Country()
            {
                CountryId = 1,
                IsoCode = "AA",
                Name = "Country1"
            };

            Country c2 = new Country()
            {
                CountryId = 1,
                IsoCode = "BB",
                Name = "Country2"
            };

            Currency cur = new Currency()
            {
                IsoCode = "XX",
                Name = "Currency",
                Countries = new List<Country>() { c1, c2}
            };
            
            c1.Currencies = new List<Currency>() { cur };
            c2.Currencies = new List<Currency>() { cur };

            // Act
            CurrencyDTO dto = cur.AsCurrencyDTO(details: true);
            
            Assert.AreEqual(dto.IsoCode, dto.IsoCode);
            Assert.AreEqual(dto.Name, dto.Name);
            Assert.IsInstanceOfType(dto, typeof(CurrencyDetailsDTO));
            Assert.IsNotNull((dto as CurrencyDetailsDTO).Countries);
            Assert.IsTrue((dto as CurrencyDetailsDTO).Countries.Count() == 2);

            var cDto1 = (dto as CurrencyDetailsDTO)
                .Countries
                .Where(x => x.IsoCode == c1.IsoCode);
            Assert.IsNotNull(cDto1);
            Assert.IsNotInstanceOfType(cDto1, typeof(CountryDetailsDTO));

            var cDto2 = (dto as CurrencyDetailsDTO)
                .Countries
                .Where(x => x.IsoCode == c2.IsoCode);
            Assert.IsNotNull(cDto2);
            Assert.IsNotInstanceOfType(cDto2, typeof(CountryDetailsDTO));
        }
    }
}
