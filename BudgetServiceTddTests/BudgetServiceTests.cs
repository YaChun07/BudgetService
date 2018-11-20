using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace BudgetServiceTdd.Tests
{
    [TestClass]
    public class BudgetServiceTests
    {
        private BudgetService _budgetService;

        [TestInitialize]
        public void FakeBudgetRepository()
        {
            var budgetRepository = Substitute.For<IBudgetRepository>();
            var budgetList = new List<Budget>
            {
                new Budget {YearMonth = "201804", Amount = 300},
                new Budget {YearMonth = "201805", Amount = 31}
            };
            budgetRepository.GetAll().Returns(budgetList);
            _budgetService = Substitute.For<BudgetService>(budgetRepository);
        }

        [TestMethod]
        public void startDate_GreaterThan_EndDate_ShouldReturn_0()
        {
            var actual = _budgetService.TotalAmount(new DateTime(2018, 04, 30), new DateTime(2018, 04, 01));
            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void NoBudget_ShouldReturn_0()
        {
            var actual = _budgetService.TotalAmount(new DateTime(2018, 12, 01), new DateTime(2018, 12, 05));
            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void Budget_On_SameMonth_ShouldReturn_20()
        {
            var actual = _budgetService.TotalAmount(new DateTime(2018, 04, 01), new DateTime(2018, 04, 02));
            Assert.AreEqual(20, actual);
        }

        [TestMethod]
        public void Budget_On_DiffMonth_ShouldReturn_12()
        {
            var actual = _budgetService.TotalAmount(new DateTime(2018, 04, 30), new DateTime(2018, 05, 02));
            Assert.AreEqual(12, actual);
        }
    }
}