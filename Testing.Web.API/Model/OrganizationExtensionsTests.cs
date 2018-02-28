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
    public class OrganizationExtensionsTests
    {
        [TestMethod]
        public void AsOrganizationDTO_Returns_DtoWithDetails()
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

            Organization item = new Organization()
            {
                Name = "XX",
                Description = "DD",
                Countries = new List<Country>() { c1, c2}
            };
            
            c1.Organizations = new List<Organization>() { item };
            c2.Organizations = new List<Organization>() { item };

            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(m => m.Link("Country", It.IsAny<object>()))
                .Returns("api/country/1");
            urlHelper.Setup(m => m.Link("Organization", It.IsAny<object>()))
                .Returns("api/organization/1");

            // Act
            var dto = item.AsOrganizationDTO(urlHelper.Object, details: true);
            
            Assert.AreEqual(dto.Name, item.Name);
            Assert.IsInstanceOfType(dto, typeof(OrganizationDetailsDTO));
            Assert.IsNotNull((dto as OrganizationDetailsDTO).Countries);
            Assert.AreEqual((dto as OrganizationDetailsDTO).Description, item.Description);
            Assert.IsTrue((dto as OrganizationDetailsDTO).Countries.Count() == 2);
            Assert.IsTrue(dto.GetUrl == "api/organization/1");

            var cDto1 = (dto as OrganizationDetailsDTO)
                .Countries
                .Where(x => x.IsoCode == c1.IsoCode)
                .FirstOrDefault();
            Assert.IsNotNull(cDto1);
            Assert.IsNotInstanceOfType(cDto1, typeof(CountryDetailsDTO));
            Assert.IsTrue(cDto1.GetUrl == "api/country/1");

            var cDto2 = (dto as OrganizationDetailsDTO)
                .Countries
                .Where(x => x.IsoCode == c2.IsoCode)
                .FirstOrDefault();
            Assert.IsNotNull(cDto2);
            Assert.IsNotInstanceOfType(cDto2, typeof(CountryDetailsDTO));
            Assert.IsTrue(cDto2.GetUrl == "api/country/1");
        }

        [TestMethod]
        public void AsOrganizationDTO_Returns_DtoWithNoDetailsIfNull()
        {
            Organization item = new Organization()
            {
                OrganizationId = 1,
                Description = "XX",
                Name = "DD",
                Countries = null
            };

            // Act
            var dto = item.AsOrganizationDTO(null, details: true);

            Assert.AreEqual((dto as OrganizationDetailsDTO).Description, item.Description);
            Assert.AreEqual(dto.Name, item.Name);
            Assert.IsInstanceOfType(dto, typeof(OrganizationDetailsDTO));
            Assert.IsNotNull((dto as OrganizationDetailsDTO).Countries);
            Assert.IsTrue((dto as OrganizationDetailsDTO).Countries.Count() == 0);
        }
    }
}
