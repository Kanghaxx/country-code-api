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

namespace Testing.Web.API.Controller
{
    [TestCategory("Web.Controllers.CurrencyController")]
    [TestClass]
    public class CurrencyControllerTests
    {
        [TestMethod]
        public async Task GetCurrencies_Returns_AllItems()
        {
            var data = new List<Currency>()
            {
                new Currency()
                {
                    CurrencyId = 1, IsoCode = "AA", Name = "C1"
                },
                new Currency()
                {
                    CurrencyId = 2, IsoCode = "BB", Name = "C2"
                },
                new Currency()
                {
                    CurrencyId = 3, IsoCode = "CC", Name = "C3"
                },
            };
            var asyncData = Task.FromResult(data.AsEnumerable());

            var repMock = new Mock<ICurrencyRepository>();
            repMock.Setup(m => m.GetAsync()).Returns(asyncData);
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(m => m.CurrencyRepository).Returns(repMock.Object);
            var factoryMock = new Mock<IStoreFactory>();
            factoryMock.Setup(m => m.CreateUnitOfWork()).Returns(uowMock.Object);

            var c = new CurrencyController(factoryMock.Object);

            var result = await c.GetCurrencies();
            var contentResult = result as OkNegotiatedContentResult<IEnumerable<CurrencyDTO>>;

            Assert.IsNotNull(contentResult);
            Assert.IsTrue(contentResult.Content.Count() == 3);
        }

        [TestMethod]
        public async Task GetCurrency_Returns_FoundItem()
        {
            var c1 = new Currency()
            {
                CurrencyId = 2,
                IsoCode = "BB",
                Name = "C2"
            };
            var data = new List<Currency>()
            {
                new Currency()
                {
                    CurrencyId = 1, IsoCode = "AA", Name = "C1"
                },
                c1
                ,
                new Currency()
                {
                    CurrencyId = 3, IsoCode = "CC", Name = "C3"
                },
            };

            var repMock = new Mock<ICurrencyRepository>();
            repMock.Setup(m => m.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(c1));
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(m => m.CurrencyRepository).Returns(repMock.Object);
            var factoryMock = new Mock<IStoreFactory>();
            factoryMock.Setup(m => m.CreateUnitOfWork()).Returns(uowMock.Object);

            var c = new CurrencyController(factoryMock.Object);

            var result = await c.GetCurrency("BB");
            var contentResult = result as OkNegotiatedContentResult<CurrencyDTO>;

            Assert.IsNotNull(contentResult);
            Assert.IsTrue(contentResult.Content.IsoCode == c1.IsoCode);
            Assert.IsTrue(contentResult.Content.Name == c1.Name);
        }

        [TestMethod]
        public async Task GetCurrency_Returns_404_IfNotFound()
        {
            var repMock = new Mock<ICurrencyRepository>();
            repMock.Setup(m => m.GetAsync("BB")).Returns(Task.FromResult<Currency>(null));
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(m => m.CurrencyRepository).Returns(repMock.Object);
            var factoryMock = new Mock<IStoreFactory>();
            factoryMock.Setup(m => m.CreateUnitOfWork()).Returns(uowMock.Object);

            var c = new CurrencyController(factoryMock.Object);

            var result = await c.GetCurrency("BB");

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }


        [TestMethod]
        public async Task Find_Returns_FoundItems()
        {
            var data = new List<Currency>()
            {
                new Currency()
                {
                    CurrencyId = 2, IsoCode = "BB", Name = "C2"
                },
                new Currency()
                {
                    CurrencyId = 3, IsoCode = "CC", Name = "C3"
                },
            };

            var repMock = new Mock<ICurrencyRepository>();
            repMock.Setup(m => m.FindAsync(new string[] { "BB", "CC"}))
                .Returns(Task.FromResult(data.AsEnumerable()));
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(m => m.CurrencyRepository).Returns(repMock.Object);
            var factoryMock = new Mock<IStoreFactory>();
            factoryMock.Setup(m => m.CreateUnitOfWork()).Returns(uowMock.Object);

            var c = new CurrencyController(factoryMock.Object);

            var result = await c.FindCurrencies(new SearchDTO()
            {
                IsoCodes = new string[] { "BB", "CC" }
            });
            var contentResult = result as OkNegotiatedContentResult<IEnumerable<CurrencyDTO>>;

            Assert.IsNotNull(contentResult);
            Assert.IsTrue(contentResult.Content.Count() == 2);
            Assert.IsNotNull(contentResult.Content.Where(x => x.IsoCode == "BB"));
            Assert.IsNotNull(contentResult.Content.Where(x => x.IsoCode == "CC"));
        }


        [TestMethod]
        public async Task Find_Returns_404_IfNotFound()
        {
            var repMock = new Mock<ICurrencyRepository>();
            repMock.Setup(m => m.FindAsync(new string[] { "BB", "CC" }))
                .Returns(Task.FromResult<IEnumerable<Currency>>(null));
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(m => m.CurrencyRepository).Returns(repMock.Object);
            var factoryMock = new Mock<IStoreFactory>();
            factoryMock.Setup(m => m.CreateUnitOfWork()).Returns(uowMock.Object);

            var c = new CurrencyController(factoryMock.Object);

            var result = await c.FindCurrencies(new SearchDTO()
            {
                IsoCodes = new string[] { "BB", "CC" }
            });

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            
        }
    }
}
