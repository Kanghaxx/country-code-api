using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Moq;
using System.Threading.Tasks;

using Data.Common.Model;
using Data.Common.Abstract;
using Web.API.Controllers;
using System.Web.Http.Results;
using Web.API.Models;
using System.Web.Http.Routing;

namespace Testing.Web.API.Controller
{
    [TestCategory("Web.Controllers.CountryController")]
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
            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(m => m.Link("PostCountry", null))
                .Returns("api/country");
            urlHelper.Setup(m => m.Link("Country", It.IsAny<object>()))
                .Returns("api/country/1");

            var c = new CountryController(factoryMock.Object);
            c.Url = urlHelper.Object;

            var result = await c.GetCountries();
            var contentResult = result as OkNegotiatedContentResult<GetCountriesResult>;

            Assert.IsNotNull(contentResult);
            Assert.IsTrue(contentResult.Content.PostURL == "api/country");
            Assert.IsTrue(contentResult.Content.Countries.All(x => x.GetUrl == "api/country/1"));
            Assert.IsTrue(contentResult.Content.Countries.Count() == 3);
        }

        [TestMethod]
        public async Task GetCountry_Returns_FoundItem()
        {
            var c1 = new Country()
            {
                CountryId = 2,
                IsoCode = "BB",
                Name = "C2"
            };
            var data = new List<Country>()
            {
                new Country()
                {
                    CountryId = 1, IsoCode = "AA", Name = "C1"
                },
                c1
                ,
                new Country()
                {
                    CountryId = 3, IsoCode = "CC", Name = "C3"
                },
            };

            var repMock = new Mock<ICountryRepository>();
            repMock.Setup(m => m.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(c1));
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(m => m.CountryRepository).Returns(repMock.Object);
            var factoryMock = new Mock<IStoreFactory>();
            factoryMock.Setup(m => m.CreateUnitOfWork()).Returns(uowMock.Object);
            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(m => m.Link("Country", It.IsAny<object>()))
                .Returns($"api/country/{c1.Name}");
            urlHelper.Setup(m => m.Link("PutCountry", It.IsAny<object>()))
                .Returns($"api/country/{c1.Name}");
            urlHelper.Setup(m => m.Link("DeleteCountry", It.IsAny<object>()))
                .Returns($"api/country/{c1.Name}");

            var c = new CountryController(factoryMock.Object);
            c.Url = urlHelper.Object;

            var result = await c.GetCountry("BB");
            var contentResult = result as OkNegotiatedContentResult<CountryDTO>;

            Assert.IsNotNull(contentResult);
            Assert.IsTrue(contentResult.Content.IsoCode == c1.IsoCode);
            Assert.IsTrue(contentResult.Content.Name == c1.Name);
            var dto = (CountryDetailsDTO)contentResult.Content;
            Assert.IsTrue(dto.GetUrl == $"api/country/{c1.Name}");
            Assert.IsTrue(dto.PutUrl == $"api/country/{c1.Name}");
            Assert.IsTrue(dto.DeleteUrl == $"api/country/{c1.Name}");
        }

        [TestMethod]
        public async Task GetCountry_Returns_404_IfNotFound()
        {
            var c1 = new Country()
            {
                CountryId = 2,
                IsoCode = "BB",
                Name = "C2"
            };
            var data = new List<Country>()
            {
                new Country()
                {
                    CountryId = 1, IsoCode = "AA", Name = "C1"
                },
                c1
                ,
                new Country()
                {
                    CountryId = 3, IsoCode = "CC", Name = "C3"
                },
            };

            var repMock = new Mock<ICountryRepository>();
            repMock.Setup(m => m.GetAsync("BB")).Returns(Task.FromResult<Country>(null));
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(m => m.CountryRepository).Returns(repMock.Object);
            var factoryMock = new Mock<IStoreFactory>();
            factoryMock.Setup(m => m.CreateUnitOfWork()).Returns(uowMock.Object);

            var c = new CountryController(factoryMock.Object);

            var result = await c.GetCountry("BB");

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }


        [TestMethod]
        public async Task Find_Returns_FoundItems()
        {
            var data = new List<Country>()
            {
                new Country()
                {
                    CountryId = 2,
                    IsoCode = "BB",
                    Name = "C2"
                },
                new Country()
                {
                    CountryId = 3, IsoCode = "CC", Name = "C3"
                },
            };

            var repMock = new Mock<ICountryRepository>();
            repMock.Setup(m => m.FindAsync(new string[] { "BB", "CC"}))
                .Returns(Task.FromResult(data.AsEnumerable()));
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(m => m.CountryRepository).Returns(repMock.Object);
            var factoryMock = new Mock<IStoreFactory>();
            factoryMock.Setup(m => m.CreateUnitOfWork()).Returns(uowMock.Object);

            var c = new CountryController(factoryMock.Object);

            var result = await c.FindCountries(new SearchDTO()
            {
                IsoCodes = new string[] { "BB", "CC" }
            });
            var contentResult = result as OkNegotiatedContentResult<IEnumerable<CountryDTO>>;

            Assert.IsNotNull(contentResult);
            Assert.IsTrue(contentResult.Content.Count() == 2);
            Assert.IsNotNull(contentResult.Content.Where(x => x.IsoCode == "BB"));
            Assert.IsNotNull(contentResult.Content.Where(x => x.IsoCode == "CC"));
        }


        [TestMethod]
        public async Task Find_Returns_404_IfNotFound()
        {
            var data = new List<Country>()
            {
                new Country()
                {
                    CountryId = 2,
                    IsoCode = "BB",
                    Name = "C2"
                },
                new Country()
                {
                    CountryId = 3, IsoCode = "CC", Name = "C3"
                },
            };

            var repMock = new Mock<ICountryRepository>();
            repMock.Setup(m => m.FindAsync(new string[] { "BB", "CC" }))
                .Returns(Task.FromResult<IEnumerable<Country>>(null));
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(m => m.CountryRepository).Returns(repMock.Object);
            var factoryMock = new Mock<IStoreFactory>();
            factoryMock.Setup(m => m.CreateUnitOfWork()).Returns(uowMock.Object);

            var c = new CountryController(factoryMock.Object);

            var result = await c.FindCountries(new SearchDTO()
            {
                IsoCodes = new string[] { "BB", "CC" }
            });

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            
        }
    }
}
