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

            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(m => m.Link("Country", It.IsAny<object>()))
                .Returns("api/country/1");
            urlHelper.Setup(m => m.Link("Currency", It.IsAny<object>()))
                .Returns("api/currency/1");

            // Act
            CountryDTO dto = c.AsCountryDTO(urlHelper.Object, details: true);
            
            Assert.AreEqual(dto.IsoCode, c.IsoCode);
            Assert.AreEqual(dto.Name, c.Name);
            Assert.IsInstanceOfType(dto, typeof(CountryDetailsDTO));
            Assert.IsNotNull((dto as CountryDetailsDTO).Currencies);
            Assert.IsTrue((dto as CountryDetailsDTO).Currencies.Count() == 2);
            //urlHelper.Verify(m => m.)
            Assert.IsTrue(dto.GetUrl == "api/country/1");

            var curDto1 = (dto as CountryDetailsDTO)
                .Currencies
                .Where(x => x.IsoCode == cur1.IsoCode)
                .FirstOrDefault();
            Assert.IsNotNull(curDto1);
            Assert.IsNotInstanceOfType(curDto1, typeof(CurrencyDetailsDTO));
            Assert.IsTrue(curDto1.GetUrl == "api/currency/1");

            var curDto2 = (dto as CountryDetailsDTO)
                .Currencies
                .Where(x => x.IsoCode == cur2.IsoCode)
                .FirstOrDefault();
            Assert.IsNotNull(curDto2);
            Assert.IsNotInstanceOfType(curDto2, typeof(CurrencyDetailsDTO));
            Assert.IsTrue(curDto2.GetUrl == "api/currency/1");
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
            CountryDTO dto = c.AsCountryDTO(null,details: true);

            Assert.AreEqual(dto.IsoCode, c.IsoCode);
            Assert.AreEqual(dto.Name, c.Name);
            Assert.IsInstanceOfType(dto, typeof(CountryDetailsDTO));
            Assert.IsNotNull((dto as CountryDetailsDTO).Currencies);
            Assert.IsTrue((dto as CountryDetailsDTO).Currencies.Count() == 0);
        }

    }
}
