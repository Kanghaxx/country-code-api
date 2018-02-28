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
    [TestCategory("Web.Controllers.OrganizationController")]
    [TestClass]
    public class OrganizationControllerTests
    {
        [TestMethod]
        public async Task GetOrganizations_Returns_AllItems()
        {
            var data = new List<Organization>()
            {
                new Organization()
                {
                    OrganizationId = 1, Name = "AA", Description = "C1"
                },                                   
                new Organization()                   
                {                                    
                    OrganizationId = 2, Name = "BB", Description = "C2"
                },                                   
                new Organization()                   
                {                                    
                    OrganizationId = 3, Name = "CC", Description = "C3"
                },
            };
            var asyncData = Task.FromResult(data.AsEnumerable());

            var repMock = new Mock<IOrganizationRepository>();
            repMock.Setup(m => m.GetAsync()).Returns(asyncData);
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(m => m.OrganizationRepository).Returns(repMock.Object);
            var factoryMock = new Mock<IStoreFactory>();
            factoryMock.Setup(m => m.CreateUnitOfWork()).Returns(uowMock.Object);

            var c = new OrganizationController(factoryMock.Object);

            var result = await c.GetOrganizations();
            var contentResult = result as OkNegotiatedContentResult<IEnumerable<OrganizationDTO>>;

            Assert.IsNotNull(contentResult);
            Assert.IsTrue(contentResult.Content.Count() == 3);
        }

        [TestMethod]
        public async Task GetOrganization_Returns_FoundItem()
        {
            var c1 = new Organization()
            {
                OrganizationId = 2, Name = "BB", Description = "C2"
            };
            var data = new List<Organization>()
            {
                new Organization()
                {
                    OrganizationId = 1, Name = "AA", Description = "C1"
                },
                c1
                ,
                new Organization()
                {
                    OrganizationId = 3, Name = "CC", Description = "C3"
                },
            };

            var repMock = new Mock<IOrganizationRepository>();
            repMock.Setup(m => m.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(c1));
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(m => m.OrganizationRepository).Returns(repMock.Object);
            var factoryMock = new Mock<IStoreFactory>();
            factoryMock.Setup(m => m.CreateUnitOfWork()).Returns(uowMock.Object);

            var c = new OrganizationController(factoryMock.Object);

            var result = await c.GetOrganization("BB");
            var contentResult = result as OkNegotiatedContentResult<OrganizationDTO>;

            Assert.IsNotNull(contentResult);
            Assert.IsTrue(contentResult.Content.Name == c1.Name);
        }

        [TestMethod]
        public async Task GetOrganization_Returns_404_IfNotFound()
        {
            var c1 = new Organization()
            {
                OrganizationId = 2,
                Name = "BB",
                Description = "C2"
            };
            var data = new List<Organization>()
            {
                new Organization()
                {
                    OrganizationId = 1, Name = "AA", Description = "C1"
                },
                c1
                ,
                new Organization()
                {
                    OrganizationId = 3, Name = "CC", Description = "C3"
                },
            };

            var repMock = new Mock<IOrganizationRepository>();
            repMock.Setup(m => m.GetAsync("BB")).Returns(Task.FromResult<Organization>(null));
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(m => m.OrganizationRepository).Returns(repMock.Object);
            var factoryMock = new Mock<IStoreFactory>();
            factoryMock.Setup(m => m.CreateUnitOfWork()).Returns(uowMock.Object);

            var c = new OrganizationController(factoryMock.Object);

            var result = await c.GetOrganization("BB");

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        
    }
}
