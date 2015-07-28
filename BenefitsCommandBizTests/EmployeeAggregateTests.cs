using Benefits.CommandBiz;
using Benefits.CommandData;
using Benefits.Infrastructure.Commands;
using Benefits.Infrastructure.Events;
using Benefits.Infrastructure.Models;
using BenefitsCommandBizTests.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace BenefitsCommandBizTests
{
    [TestClass]
    public class EmployeeAggregateTests
    {
        [TestMethod]
        public void TestBasicCreateCommandHandling()
        {
            var target = new EmployeeAggregate(CreateDataModelMock());

            var cmd = new EmployeeCreateCommand() { Arg = CreateNewEmployeeModel() };

            var results = target.Handle(cmd).ToArray();

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());

            var empCreatedEvent = results.First() as EmployeeCreatedEvent;

            Assert.IsNotNull(empCreatedEvent);
        }

        [TestMethod]
        public void TestBasicPayCalculation()
        {
            var target = new EmployeeAggregate(CreateDataModelMock());

            var cmd = new EmployeeCreateCommand() { Arg = CreateNewEmployeeModel() };

            var results = target.Handle(cmd).ToArray();

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());

            var empCreatedEvent = results.First() as EmployeeCreatedEvent;

            Assert.IsNotNull(empCreatedEvent);

            Assert.AreEqual(2000M, empCreatedEvent.Data.GrossPay);
            Assert.AreEqual(38M, empCreatedEvent.Data.Benefits);
            Assert.AreEqual(1962M, empCreatedEvent.Data.NetPay);
        }

        [TestMethod]
        public void TestBasicPayWithDependentCalculation()
        {
            var target = new EmployeeAggregate(CreateDataModelMock());

            var arg = CreateNewEmployeeModel();

            arg.Dependents = new DependentModel[] { new DependentModel() { Name = "OtherGuy" } };

            var cmd = new EmployeeCreateCommand() { Arg = arg };

            var results = target.Handle(cmd).ToArray();

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());

            var empCreatedEvent = results.First() as EmployeeCreatedEvent;

            Assert.IsNotNull(empCreatedEvent);

            Assert.AreEqual(2000M, empCreatedEvent.Data.GrossPay);
            Assert.AreEqual(57M, empCreatedEvent.Data.Benefits);
            Assert.AreEqual(1943M, empCreatedEvent.Data.NetPay);
        }

        [TestMethod]
        public void TestDiscountPayCalculation()
        {
            var target = new EmployeeAggregate(CreateDataModelMock());

            var arg = CreateNewEmployeeModel();

            arg.Name = "A" + arg.Name;

            var cmd = new EmployeeCreateCommand() { Arg = arg };

            var results = target.Handle(cmd).ToArray();

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());

            var empCreatedEvent = results.First() as EmployeeCreatedEvent;

            Assert.IsNotNull(empCreatedEvent);

            Assert.AreEqual(2000M, empCreatedEvent.Data.GrossPay);
            Assert.AreEqual(34.2M, empCreatedEvent.Data.Benefits);
            Assert.AreEqual(1965.8M, empCreatedEvent.Data.NetPay);
        }

        [TestMethod]
        public void TestBasicEditCommandHandling()
        {
            var target = new EmployeeAggregate(CreateDataModelMock());

            var cmd = new EmployeeEditCommand() { Arg = CreateExistingEmployeeModel() };

            var results = target.Handle(cmd).ToArray();

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());

            var empEditedEvent = results.First() as EmployeeEditedEvent;

            Assert.IsNotNull(empEditedEvent);
        }

        [TestMethod]
        public void TestBasicDeleteCommandHandling()
        {
            var target = new EmployeeAggregate(CreateDataModelMock());

            var cmd = new EmployeeDeleteCommand() { Arg = CreateExistingEmployeeModel() };

            var results = target.Handle(cmd).ToArray();

            Assert.IsNotNull(results);
            Assert.AreEqual(1, results.Count());

            var empDeletedEvent = results.First() as EmployeeDeletedEvent;

            Assert.IsNotNull(empDeletedEvent);
        }

        static ICommandDataModel CreateDataModelMock()
        {
            var emp = new Employee { Id = "1", Name = "ThatGuy", IsDeleted = false };
            var empCreatedEvent = new EmployeeEvent() { Employee = emp, EmployeeId = "1", Id = "1", Name = "EmployeeCreated", Timestamp = DateTime.Now, Version = 1, Data = "{\"Id\":\"1\",\"Name\":\"ThatGuy\",\"Version\":1,\"IsDeleted\":false,\"GrossPay\":2000.0,\"Benefits\":38.0,\"NetPay\":1962.0,\"Dependents\":[]}" };
            
            var empData = new List<Employee> { emp }.AsQueryable();
            var empEvtData = new List<EmployeeEvent> { empCreatedEvent }.AsQueryable();

            var empMockSet = new Mock<DbSet<Employee>>();
            var empEvtMockSet = new Mock<DbSet<EmployeeEvent>>();

            empMockSet.As<IDbAsyncEnumerable<Employee>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Employee>(empData.GetEnumerator()));

            empEvtMockSet.As<IDbAsyncEnumerable<EmployeeEvent>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<EmployeeEvent>(empEvtData.GetEnumerator()));

            empMockSet.As<IQueryable<Employee>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Employee>(empData.Provider));

            empEvtMockSet.As<IQueryable<EmployeeEvent>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<EmployeeEvent>(empEvtData.Provider));

            empMockSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(empData.Expression);
            empMockSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(empData.ElementType);
            empMockSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(empData.GetEnumerator());

            empEvtMockSet.As<IQueryable<EmployeeEvent>>().Setup(m => m.Expression).Returns(empEvtData.Expression);
            empEvtMockSet.As<IQueryable<EmployeeEvent>>().Setup(m => m.ElementType).Returns(empEvtData.ElementType);
            empEvtMockSet.As<IQueryable<EmployeeEvent>>().Setup(m => m.GetEnumerator()).Returns(empEvtData.GetEnumerator());

            var mockContext = new Mock<ICommandDataModel>();

            mockContext.Setup(c => c.Employees).Returns(empMockSet.Object);
            mockContext.Setup(c => c.EmployeeEvents).Returns(empEvtMockSet.Object);

            return mockContext.Object;
        }

        static EmployeeModel CreateNewEmployeeModel()
        {
            return new EmployeeModel() 
            {
                 Name = "ThisGuy"
            };
        }

        static EmployeeModel CreateExistingEmployeeModel()
        {
            return new EmployeeModel()
            {
                Name = "ThatGuy",
                Version = 1,
                Id = "1"
            };
        }
    }
}
