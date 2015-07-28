using Benefits.Infrastructure;
using Benefits.Infrastructure.Commands;
using Benefits.QueryBiz;
using Benefits.QueryData;
using BenefitsWeb.Controllers;
using BenefitsWeb.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace BenefitsWebTests.Controllers
{
    [TestClass]
    public class EmployeesApiControllerTests
    {
        [TestMethod]
        public void GetTest()
        {
            var controller = new EmployeesApiController(CreateIQueriesMock(), CreateICommandBusMock());
            var result = controller.Get();

            Assert.IsNotNull(result);

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetByIdTest()
        {
            var controller = new EmployeesApiController(CreateIQueriesMock(), CreateICommandBusMock());
            var result = controller.Get("1");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PostTest()
        {
            var cmdBus = new Mock<ICommandBus>();
            var controller = new EmployeesApiController(CreateIQueriesMock(), cmdBus.Object);
            var newEmp = CreateNewEmployeeDetailViewModel();

            var result = controller.Post(newEmp);

            Assert.IsNotNull(result);

            cmdBus.Verify(i => i.Execute(It.IsAny<MessageBase>()));
        }

        [TestMethod]
        public void PutTest()
        {
            var cmdBus = new Mock<ICommandBus>();
            var controller = new EmployeesApiController(CreateIQueriesMock(), cmdBus.Object);
            var oldEmp = CreateExistingEmployeeDetailViewModel();

            controller.Put("1", oldEmp);

            cmdBus.Verify(i => i.Execute(It.IsAny<MessageBase>()));
        }

        [TestMethod]
        public void DeleteTest()
        {
            var cmdBus = new Mock<ICommandBus>();
            var controller = new EmployeesApiController(CreateIQueriesMock(), cmdBus.Object);
           
            controller.Delete("1");

            cmdBus.Verify(i => i.Execute(It.IsAny<MessageBase>()));
        }

        static EmployeeDetailViewModel CreateNewEmployeeDetailViewModel()
        {
            return new EmployeeDetailViewModel()
            {
                Name = "NewGuy"
            };
        }

        static EmployeeDetailViewModel CreateExistingEmployeeDetailViewModel()
        {
            return new EmployeeDetailViewModel()
            {
                Benefits = 38M,
                GrossPay = 2000M,
                Id = "1",
                Name = "OldGuy",
                NetPay = 1962M,
                Version = 1
            };
        }

        static IQueries CreateIQueriesMock()
        {
            var employee1 = new EmployeeDetail()
            {
                Benefits = 38M,
                GrossPay = 2000M,
                Id = "1",
                IsDeleted = false,
                Name = "ThisGuy",
                NetPay = 1962M,
                Version = 1,
                Dependents = 0
            };

            var employee2 = new EmployeeDetail()
            {
                Benefits = 57M,
                GrossPay = 2000M,
                Id = "2",
                IsDeleted = false,
                Name = "ThisGuy",
                NetPay = 1943M,
                Version = 1,
                Dependents = 1
            };

            employee2.DependentDetails.Add(new DependentDetail() { EmployeeDetail = employee2, EmployeeId = "2", Id = "1", Name = "OtherGuy" });

            var employees = new List<EmployeeDetail>()
            {
                 employee1,
                 employee2
            };

            var mock = new Mock<IQueries>();

            mock.Setup(i => i.GetAllEmployees()).Returns(employees);
            mock.Setup(i => i.GetEmployeeById("1")).Returns(employee1);
            mock.Setup(i => i.GetEmployeeById("2")).Returns(employee2);
          
            return mock.Object;
        }

        static ICommandBus CreateICommandBusMock()
        {
            var mock = new Mock<ICommandBus>();

            return mock.Object;
        }
    }
}
