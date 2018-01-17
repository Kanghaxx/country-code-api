using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using System.Data.Entity;

using Data.Common.Model;
using Data.Repository.Context;
using Data.Repository;
using System.Threading.Tasks;

namespace Testing.Data.Repository
{
    [TestCategory("Data.CountryRepository")]
    [TestClass]
    public class CountryRepositoryTests
    {
        [TestMethod]
        public async Task GetAsync_Returns_Sorted_Items()
        {
            var data = new List<Country>
            {
                new Country { IsoCode = "BB", Name = "YYY" },
                new Country { IsoCode = "AA", Name = "XXX" },
                new Country { IsoCode = "CC", Name = "UUU" },
            };
            
            var mockSet = new Mock<DbSet<Country>>().SetupData(data);
            var mockContext = new Mock<CountryContext>();
            mockContext.Setup(c => c.Countries).Returns(mockSet.Object);

            var service = new CountryRepository(mockContext.Object);

            // Act
            var items = (await service.GetAsync()).ToList();

            Assert.IsNotNull(items);
            Assert.AreEqual(items.Count, data.Count);
            Assert.AreEqual(items[0].IsoCode, "AA");
            Assert.AreEqual(items[1].IsoCode, "BB");
            Assert.AreEqual(items[2].IsoCode, "CC");
        }

        [TestMethod]
        public void Get_Returns_Sorted_Items()
        {
            var data = new List<Country>
            {
                new Country { IsoCode = "BB", Name = "YYY" },
                new Country { IsoCode = "AA", Name = "XXX" },
                new Country { IsoCode = "CC", Name = "UUU" },
            };

            var mockSet = new Mock<DbSet<Country>>().SetupData(data);
            var mockContext = new Mock<CountryContext>();
            mockContext.Setup(c => c.Countries).Returns(mockSet.Object);

            var service = new CountryRepository(mockContext.Object);

            // Act
            var items = service.Get().ToList();

            Assert.IsNotNull(items);
            Assert.AreEqual(items.Count, data.Count);
            Assert.AreEqual(items[0].IsoCode, "AA");
            Assert.AreEqual(items[1].IsoCode, "BB");
            Assert.AreEqual(items[2].IsoCode, "CC");
        }

        [TestMethod]
        public void Get_ByIsoCode_Returns_FoundItem()
        {
            var data = new List<Country>
            {
                new Country { IsoCode = "BB", Name = "YYY" },
                new Country { IsoCode = "AA", Name = "XXX" },
                new Country { IsoCode = "CC", Name = "UUU" },
            };

            var mockSet = new Mock<DbSet<Country>>().SetupData(data);
            var mockContext = new Mock<CountryContext>();
            mockContext.Setup(c => c.Countries).Returns(mockSet.Object);

            var service = new CountryRepository(mockContext.Object);

            // Act
            var item = service.Get("BB");

            Assert.IsNotNull(item);
            Assert.AreEqual(item.IsoCode, "BB");
        }

        [TestMethod]
        public async Task GetAsync_ByIsoCode_Returns_FoundItem()
        {
            var data = new List<Country>
            {
                new Country { IsoCode = "BB", Name = "YYY" },
                new Country { IsoCode = "AA", Name = "XXX" },
                new Country { IsoCode = "CC", Name = "UUU" },
            };

            var mockSet = new Mock<DbSet<Country>>().SetupData(data);
            var mockContext = new Mock<CountryContext>();
            mockContext.Setup(c => c.Countries).Returns(mockSet.Object);

            var service = new CountryRepository(mockContext.Object);

            // Act
            var item = await service.GetAsync("BB");

            Assert.IsNotNull(item);
            Assert.AreEqual(item.IsoCode, "BB");
        }

        [TestMethod]
        public void Get_ById_Returns_FoundItem()
        {
            var data = new List<Country>
            {
                new Country { CountryId = 2, IsoCode = "BB", Name = "YYY" },
                new Country { CountryId = 1, IsoCode = "AA", Name = "XXX" },
                new Country { CountryId = 3, IsoCode = "CC", Name = "UUU" },
            };
            
            var mockSet = new Mock<DbSet<Country>>().SetupData(data, 
                objects => data.SingleOrDefault(d => d.CountryId == (long)objects.First()));
            var mockContext = new Mock<CountryContext>();
            mockContext.Setup(c => c.Countries).Returns(mockSet.Object);
            mockContext.Setup(c => c.Set<Country>()).Returns(mockSet.Object);

            var service = new CountryRepository(mockContext.Object);

            // Act
            var item = service.Get(2);

            Assert.IsNotNull(item);
            Assert.AreEqual(item.IsoCode, "BB");
        }

        [TestMethod]
        public async Task GetAsync_ById_Returns_FoundItem()
        {
            var data = new List<Country>
            {
                new Country { CountryId = 2, IsoCode = "BB", Name = "YYY" },
                new Country { CountryId = 1, IsoCode = "AA", Name = "XXX" },
                new Country { CountryId = 3, IsoCode = "CC", Name = "UUU" },
            };

            var mockSet = new Mock<DbSet<Country>>().SetupData(data,
                objects => data.SingleOrDefault(d => d.CountryId == (long)objects.First()));
            var mockContext = new Mock<CountryContext>();
            mockContext.Setup(c => c.Countries).Returns(mockSet.Object);
            mockContext.Setup(c => c.Set<Country>()).Returns(mockSet.Object);

            var service = new CountryRepository(mockContext.Object);

            // Act
            var item = await service.GetAsync(2);

            Assert.IsNotNull(item);
            Assert.AreEqual(item.IsoCode, "BB");
        }


        [TestMethod]
        public void Add_AddsItemToDbSet()
        {
            var newCountry = new Country()
            {
                CountryId = 4,
                IsoCode = "DD",
                Name = "TestName"
            };

            var mockSet = new Mock<DbSet<Country>>().SetupData(new List<Country>());
            mockSet.Setup(m => m.Add(It.IsAny<Country>())).Returns(newCountry);

            var mockContext = new Mock<CountryContext>();
            mockContext.Setup(c => c.Set<Country>()).Returns(mockSet.Object);

            var service = new CountryRepository(mockContext.Object);
            
            // Act
            var item = service.Add(newCountry);
            
            mockSet.Verify(m => m.Add(newCountry), Times.Once);
            Assert.IsNotNull(item);
            Assert.AreEqual(item.IsoCode, newCountry.IsoCode);
            Assert.AreEqual(item.Name, newCountry.Name);
        }


        [TestMethod]
        public async Task FindAsync_Returns_Sorted_FoundByIsoCode()
        {
            var data = new List<Country>
            {
                new Country { IsoCode = "CC", Name = "UUU" },
                new Country { IsoCode = "BB", Name = "XXX" },
                new Country { IsoCode = "DD", Name = "YYY" },
                new Country { IsoCode = "AA", Name = "YYY" },
            };

            var mockSet = new Mock<DbSet<Country>>().SetupData(data);
            var mockContext = new Mock<CountryContext>();
            mockContext.Setup(c => c.Countries).Returns(mockSet.Object);

            var service = new CountryRepository(mockContext.Object);

            // Act
            var items = (await service.FindAsync(new string[] { "BB", "CC" })).ToList();

            Assert.IsNotNull(items);
            Assert.AreEqual(items.Count, 2);
            Assert.AreEqual(items[0].IsoCode, "BB");
            Assert.AreEqual(items[1].IsoCode, "CC");
        }

        [TestMethod]
        public void Find_Returns_Sorted_FoundByIsoCode()
        {
            var data = new List<Country>
            {
                new Country { IsoCode = "CC", Name = "UUU" },
                new Country { IsoCode = "BB", Name = "XXX" },
                new Country { IsoCode = "DD", Name = "YYY" },
                new Country { IsoCode = "AA", Name = "YYY" },
            };

            var mockSet = new Mock<DbSet<Country>>().SetupData(data);
            var mockContext = new Mock<CountryContext>();
            mockContext.Setup(c => c.Countries).Returns(mockSet.Object);

            var service = new CountryRepository(mockContext.Object);

            // Act
            var items = service.Find(new string[] { "BB", "CC" }).ToList();

            Assert.IsNotNull(items);
            Assert.AreEqual(items.Count, 2);
            Assert.AreEqual(items[0].IsoCode, "BB");
            Assert.AreEqual(items[1].IsoCode, "CC");
        }
    }
}
