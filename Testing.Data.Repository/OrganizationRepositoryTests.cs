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
    [TestCategory("Data.OrganizationRepository")]
    [TestClass]
    public class OrganizationRepositoryTests
    {
        [TestMethod]
        public async Task GetAsync_Returns_Sorted_Items()
        {
            var data = new List<Organization>
            {
                new Organization { Name = "BB", Description = "YYY" },
                new Organization { Name = "AA", Description = "XXX" },
                new Organization { Name = "CC", Description = "UUU" },
            };
            
            var mockSet = new Mock<DbSet<Organization>>().SetupData(data);
            var mockContext = new Mock<CountryContext>();
            mockContext.Setup(c => c.Organizations).Returns(mockSet.Object);

            var service = new OrganizationRepository(mockContext.Object);

            // Act
            var items = (await service.GetAsync()).ToList();

            Assert.IsNotNull(items);
            Assert.AreEqual(items.Count, data.Count);
            Assert.AreEqual(items[0].Name, "AA");
            Assert.AreEqual(items[1].Name, "BB");
            Assert.AreEqual(items[2].Name, "CC");
        }

        [TestMethod]
        public void Get_Returns_Sorted_Items()
        {
            var data = new List<Organization>
            {
                new Organization { Name = "BB", Description = "YYY" },
                new Organization { Name = "AA", Description = "XXX" },
                new Organization { Name = "CC", Description = "UUU" },
            };

            var mockSet = new Mock<DbSet<Organization>>().SetupData(data);
            var mockContext = new Mock<CountryContext>();
            mockContext.Setup(c => c.Organizations).Returns(mockSet.Object);

            var service = new OrganizationRepository(mockContext.Object);

            // Act
            var items = service.Get().ToList();

            Assert.IsNotNull(items);
            Assert.AreEqual(items.Count, data.Count);
            Assert.AreEqual(items[0].Name, "AA");
            Assert.AreEqual(items[1].Name, "BB");
            Assert.AreEqual(items[2].Name, "CC");
        }

        [TestMethod]
        public void Get_ByIsoCode_Returns_FoundItem()
        {
            var data = new List<Organization>
            {
                new Organization { Name = "BB", Description = "YYY" },
                new Organization { Name = "AA", Description = "XXX" },
                new Organization { Name = "CC", Description = "UUU" },
            };

            var mockSet = new Mock<DbSet<Organization>>().SetupData(data);
            var mockContext = new Mock<CountryContext>();
            mockContext.Setup(c => c.Organizations).Returns(mockSet.Object);

            var service = new OrganizationRepository(mockContext.Object);

            // Act
            var item = service.Get("BB");

            Assert.IsNotNull(item);
            Assert.AreEqual(item.Name, "BB");
        }

        [TestMethod]
        public async Task GetAsync_ByIsoCode_Returns_FoundItem()
        {
            var data = new List<Organization>
            {
                new Organization { Name = "BB", Description = "YYY" },
                new Organization { Name = "AA", Description = "XXX" },
                new Organization { Name = "CC", Description = "UUU" },
            };

            var mockSet = new Mock<DbSet<Organization>>().SetupData(data);
            var mockContext = new Mock<CountryContext>();
            mockContext.Setup(c => c.Organizations).Returns(mockSet.Object);

            var service = new OrganizationRepository(mockContext.Object);

            // Act
            var item = await service.GetAsync("BB");

            Assert.IsNotNull(item);
            Assert.AreEqual(item.Name, "BB");
        }

        [TestMethod]
        public void Get_ById_Returns_FoundItem()
        {
            var data = new List<Organization>
            {
                new Organization { Name = "BB", Description = "YYY", OrganizationId = 2 },
                new Organization { Name = "AA", Description = "XXX", OrganizationId = 1 },
                new Organization { Name = "CC", Description = "UUU", OrganizationId = 3 },
            };

            var mockSet = new Mock<DbSet<Organization>>().SetupData(data, 
                objects => data.SingleOrDefault(d => d.OrganizationId == (long)objects.First()));
            var mockContext = new Mock<CountryContext>();
            mockContext.Setup(c => c.Organizations).Returns(mockSet.Object);
            mockContext.Setup(c => c.Set<Organization>()).Returns(mockSet.Object);

            var service = new OrganizationRepository(mockContext.Object);

            // Act
            var item = service.Get(2);

            Assert.IsNotNull(item);
            Assert.AreEqual(item.Name, "BB");
        }

        [TestMethod]
        public async Task GetAsync_ById_Returns_FoundItem()
        {
            var data = new List<Organization>
            {
                new Organization { Name = "BB", Description = "YYY", OrganizationId = 2 },
                new Organization { Name = "AA", Description = "XXX", OrganizationId = 1 },
                new Organization { Name = "CC", Description = "UUU", OrganizationId = 3 },
            };

            var mockSet = new Mock<DbSet<Organization>>().SetupData(data,
                objects => data.SingleOrDefault(d => d.OrganizationId == (long)objects.First()));
            var mockContext = new Mock<CountryContext>();
            mockContext.Setup(c => c.Organizations).Returns(mockSet.Object);
            mockContext.Setup(c => c.Set<Organization>()).Returns(mockSet.Object);

            var service = new OrganizationRepository(mockContext.Object);

            // Act
            var item = await service.GetAsync(2);

            Assert.IsNotNull(item);
            Assert.AreEqual(item.Name, "BB");
        }


        [TestMethod]
        public void Add_AddsItemToDbSet()
        {
            var newItem = new Organization()
            {
                OrganizationId = 4,
                Description = "DD",
                Name = "TestName"
            };

            var mockSet = new Mock<DbSet<Organization>>().SetupData(new List<Organization>());
            mockSet.Setup(m => m.Add(It.IsAny<Organization>())).Returns(newItem);

            var mockContext = new Mock<CountryContext>();
            mockContext.Setup(c => c.Set<Organization>()).Returns(mockSet.Object);

            var service = new OrganizationRepository(mockContext.Object);
            
            // Act
            var item = service.Add(newItem);
            
            mockSet.Verify(m => m.Add(newItem), Times.Once);
            Assert.IsNotNull(item);
            Assert.AreEqual(item.Description, newItem.Description);
            Assert.AreEqual(item.Name, newItem.Name);
        }
        
    }
}
