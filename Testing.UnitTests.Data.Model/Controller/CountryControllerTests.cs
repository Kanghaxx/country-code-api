using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Moq;
using System.Threading.Tasks;

using Data.Common.Model;
using Data.Common.Abstract;
using Web.API.Controllers;

namespace Testing.UnitTests.Data.Model.Controller
{
    [TestCategory("Web.Controller.CountryController")]
    [TestClass]
    public class CountryControllerTests
    {
        [TestMethod]
        public async Task GetCountries_Returns_AllItems()
        {
            var data = new List<Country>()
            {
                new Country()
                {
                    CountryId = 1, IsoCode = "AA", Name = "C1"
                },
                new Country()
                {
                    CountryId = 2, IsoCode = "BB", Name = "C2"
                },
                new Country()
                {
                    CountryId = 3, IsoCode = "CC", Name = "C3"
                },
            };
            var asyncData = Task.FromResult(data.AsEnumerable());

            var repMock = new Mock<ICountryRepository>();
            repMock.Setup(m => m.GetAsync()).Returns(asyncData);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(m => m.CountryRepository).Returns(repMock.Object);

            var factoryMock = new Mock<IStoreFactory>();
            factoryMock.Setup(m => m.CreateUnitOfWork()).Returns(uowMock.Object);

            var c = new CountryController(factoryMock.Object);

            var result = await c.GetCountries();

            Assert.IsNotNull(result);
        }
    }
}
