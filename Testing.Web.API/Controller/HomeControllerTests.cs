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
    [TestCategory("Web.Controllers.HomeController")]
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Get_Returns_Endpoints()
        {
            var urlHelper = new Mock<UrlHelper>();
            urlHelper.Setup(m => m.Link("GetCountries", null)).Returns("api/GetCountries");
            urlHelper.Setup(m => m.Link("GetCurrencies", null)).Returns("api/GetCurrencies");
            urlHelper.Setup(m => m.Link("GetOrganizations", null)).Returns("api/GetOrganizations");
            urlHelper.Setup(m => m.Content("~/api/help/index")).Returns("api/help/index");

            var c = new HomeController();
            c.Url = urlHelper.Object;

            // Act
            var result = c.GetEndpoints();
            var contentResult = result as OkNegotiatedContentResult<GetEndpointsResult>;

            Assert.IsNotNull(contentResult);
            Assert.IsTrue(contentResult.Content.Endpoints.Count() == 4);
            Assert.IsTrue(contentResult.Content.Endpoints["GetCountries"] == "api/GetCountries");
            Assert.IsTrue(contentResult.Content.Endpoints["GetCurrencies"] == "api/GetCurrencies");
            Assert.IsTrue(contentResult.Content.Endpoints["GetOrganizations"] == "api/GetOrganizations");
            Assert.IsTrue(contentResult.Content.Endpoints["GetHelpHtml"] == "api/help/index");
        }
        
    }
}
