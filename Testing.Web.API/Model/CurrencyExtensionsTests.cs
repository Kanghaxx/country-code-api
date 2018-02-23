using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

using Data.Common.Model;
using Web.API.Models;
using System.Web.Http.Routing;
using Moq;

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

            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(m => m.Link("Country", It.IsAny<object>()))
                .Returns("api/country/1");
            urlHelper.Setup(m => m.Link("Currency", It.IsAny<object>()))
                .Returns("api/currency/1");

            // Act
            CurrencyDTO dto = cur.AsCurrencyDTO(urlHelper.Object, details: true);
            
            Assert.AreEqual(dto.IsoCode, dto.IsoCode);
            Assert.AreEqual(dto.Name, dto.Name);
            Assert.IsInstanceOfType(dto, typeof(CurrencyDetailsDTO));
            Assert.IsNotNull((dto as CurrencyDetailsDTO).Countries);
            Assert.IsTrue((dto as CurrencyDetailsDTO).Countries.Count() == 2);
            Assert.IsTrue(dto.GetUrl == "api/currency/1");

            var cDto1 = (dto as CurrencyDetailsDTO)
                .Countries
                .Where(x => x.IsoCode == c1.IsoCode)
                .FirstOrDefault();
            Assert.IsNotNull(cDto1);
            Assert.IsNotInstanceOfType(cDto1, typeof(CountryDetailsDTO));
            Assert.IsTrue(cDto1.GetUrl == "api/country/1");

            var cDto2 = (dto as CurrencyDetailsDTO)
                .Countries
                .Where(x => x.IsoCode == c2.IsoCode)
                .FirstOrDefault();
            Assert.IsNotNull(cDto2);
            Assert.IsNotInstanceOfType(cDto2, typeof(CountryDetailsDTO));
            Assert.IsTrue(cDto2.GetUrl == "api/country/1");
        }

        [TestMethod]
        public void AsCurrencyDTO_Returns_DtoWithNoDetailsIfNull()
        {
            Currency c = new Currency()
            {
                CurrencyId = 1,
                IsoCode = "XX",
                Name = "Curr1",
                Countries = null
            };

            // Act
            CurrencyDTO dto = c.AsCurrencyDTO(null, details: true);

            Assert.AreEqual(dto.IsoCode, c.IsoCode);
            Assert.AreEqual(dto.Name, c.Name);
            Assert.IsInstanceOfType(dto, typeof(CurrencyDetailsDTO));
            Assert.IsNotNull((dto as CurrencyDetailsDTO).Countries);
            Assert.IsTrue((dto as CurrencyDetailsDTO).Countries.Count() == 0);
        }
    }
}
