﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetServiceTdd
{
	public class BudgetService
	{
		private readonly List<Budget> _budgets;

		public BudgetService(IBudgetRepository budgetRepository)
		{
			_budgets = budgetRepository.GetAll();
		}

		public double TotalAmount(DateTime start, DateTime end)
		{
			if (start > end) return 0;

			// code smell, a little primitive obsession for datetime.
			// code smell, data clump for start, end always occur together.
			return GetBudgetTotalAmount(start, end);
		}

		private int GetBudgetTotalAmount(DateTime start, DateTime end)
		{
			var period = new Period(start, end);
			var totalAmount = 0;
			foreach (var diffMonth in period.GetMidMonths())
			{
				var budget = _budgets.FirstOrDefault(x => x.YearMonth == diffMonth);
				if (budget == null) continue;
				totalAmount += period.IntervalDays(budget) * budget.DailyAmount();
			}

			return totalAmount;
		}
	}
}