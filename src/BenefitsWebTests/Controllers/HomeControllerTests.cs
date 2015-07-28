using Benefits.QueryBiz;
using Benefits.QueryData;
using BenefitsWeb.Controllers;
using BenefitsWeb.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BenefitsWebTests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void IndexTest()
        {
            var controller = new HomeController(CreateIQueriesMock());
            var result = controller.Index() as ViewResult;

            Assert.IsNotNull(result);

            var model = result.Model as KpisViewModel;

            Assert.IsNotNull(model);

            Assert.AreEqual(2, model.Employees);
            Assert.AreEqual(5000, model.GrossPay);
            Assert.AreEqual(1000, model.Benefits);
            Assert.AreEqual(4000, model.NetPay);
        }

        static IQueries CreateIQueriesMock()
        {
            var kpis = new List<Kpi>()
            {
                 new Kpi() { Id = "Employees", Value = 2 },
                 new Kpi() { Id = "GrossPay", Value = 5000 },
                 new Kpi() { Id = "Benefits", Value = 1000 },
                 new Kpi() { Id = "NetPay", Value = 4000 },
            };

            var mock = new Mock<IQueries>();

            mock.Setup(i => i.GetAllKpis()).Returns(kpis);

            return mock.Object;
        }
    }
}
