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
using Data.Common.Abstract;

namespace Testing.Data.Repository
{
    [TestCategory("Data.UnitOfWork")]
    [TestClass]
    public class UnitOfWorkTests
    {

        [TestMethod]
        public void Complete_SavesChanges()
        {
            var mockContext = new Mock<CountryContext>();

            var service = new TestUnitofWork(mockContext.Object);

            // Act
            service.Complete();

            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [TestMethod]
        public async Task CompleteAsync_SavesChanges()
        {
            var mockContext = new Mock<CountryContext>();

            var service = new TestUnitofWork(mockContext.Object);

            // Act
            await service.CompleteAsync();

            mockContext.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce);
        }

        [TestMethod]
        public void CountryRepository_Returns_InstanceOf_ICountryRepository()
        {
            var mockContext = new Mock<CountryContext>();

            var service = new TestUnitofWork(mockContext.Object);

            // Act
            var r = service.CountryRepository;
            var r2 = service.CountryRepository;

            Assert.IsNotNull(r);
            Assert.IsInstanceOfType(r, typeof(ICountryRepository));
            Assert.AreSame(r, r2);
        }

        [TestMethod]
        public void CurrencyRepository_Returns_InstanceOf_ICurrencyRepository()
        {
            var mockContext = new Mock<CountryContext>();

            var service = new TestUnitofWork(mockContext.Object);

            // Act
            var r = service.CurrencyRepository;
            var r2 = service.CurrencyRepository;

            Assert.IsNotNull(r);
            Assert.IsInstanceOfType(r, typeof(ICurrencyRepository));
            Assert.AreSame(r, r2);
        }


        [TestMethod]
        public void OrganizationRepository_Returns_InstanceOf_IOrganizationRepository()
        {
            var mockContext = new Mock<CountryContext>();

            var service = new TestUnitofWork(mockContext.Object);

            // Act
            var r = service.OrganizationRepository;
            var r2 = service.OrganizationRepository;

            Assert.IsNotNull(r);
            Assert.IsInstanceOfType(r, typeof(IOrganizationRepository));
            Assert.AreSame(r, r2);
        }

        [TestMethod]
        public void Propagates_Same_Context()
        {
            var mockContext = new Mock<CountryContext>();

            var service = new TestUnitofWork(mockContext.Object);

            // Act
            CountryRepository countryRep = service.CountryRepository as CountryRepository;
            CurrencyRepository currencyRep = service.CurrencyRepository as CurrencyRepository;
            OrganizationRepository orgRep = service.OrganizationRepository as OrganizationRepository;

            Assert.IsNotNull(countryRep);
            Assert.IsNotNull(currencyRep);
            Assert.IsNotNull(orgRep);
            Assert.AreSame(countryRep.Context, currencyRep.Context);
            Assert.AreSame(countryRep.Context, orgRep.Context);
        }
    }

    class TestUnitofWork : UnitOfWork
    {
        public TestUnitofWork(CountryContext contextMock): base(contextMock)
        {
        }
    }
}


