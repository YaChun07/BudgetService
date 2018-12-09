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
			var starTimeSpan = period.GetStarTimeSpan();
			var endTimeSpan = period.GetEndTimeSpan();

			return GetMidMonthBudget(period)
				   + GetTotalBudgetByMonth(starTimeSpan, period.Start)
				   + GetTotalBudgetByMonth(endTimeSpan, period.End);
		}

		private int GetMidMonthBudget(Period period)
		{
			var budgetTotalAmount = 0;
			var diffMonths = period.GetMidMonths();

			foreach (var diffMonth in RemoveFirstAndLast(diffMonths))
			{
				var midMonthTime = new DateTime(int.Parse(diffMonth.Substring(0, 4)), int.Parse(diffMonth.Substring(4, 2)), 1);

				if (CheckBudgetEmpty(diffMonth))
				{
					budgetTotalAmount += GetTotalBudgetByMonth(GetBudgetMonthDays(midMonthTime), midMonthTime);
				}
			}
			return budgetTotalAmount;
		}

		private IEnumerable<string> RemoveFirstAndLast(IEnumerable<string> diffMonths)
		{
			return diffMonths.Skip(1).Take(diffMonths.Count() - 2);
		}

		private int GetTotalBudgetByMonth(int starTimeSpan, DateTime date)
		{
			if (!CheckBudgetEmpty(date.ToString("yyyyMM")))
			{
				return 0;
			}
			var budget = _budgets.FirstOrDefault(x => x.YearMonth == date.ToString("yyyyMM"));

			var budgetAmount = 0;

			if (budget != null)
			{
				budgetAmount = budget.Amount;
			}

			var daysInMonth = budget.DaysInMonth();
			return starTimeSpan * budgetAmount / daysInMonth;
		}

		private int GetBudgetMonthDays(DateTime date)
		{
			return DateTime.DaysInMonth(int.Parse(date.Year.ToString()), int.Parse(date.Month.ToString()));
		}

		private bool CheckBudgetEmpty(string yearMonth)
		{
			return _budgets.Any(x => x.YearMonth == yearMonth);
		}
	}
}