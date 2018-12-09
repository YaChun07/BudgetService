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

			var firstMonthAmount = FirstMonthAmount(period);
			var lastMonthAmount = LastMonthAmount(period);
			return GetMidMonthBudget(period)
				   + firstMonthAmount
				   + lastMonthAmount;
		}

		private int LastMonthAmount(Period period)
		{
			var lastMonthAmount = 0;
			var lastMonthBudget = _budgets.FirstOrDefault(x => x.YearMonth == period.End.ToString("yyyyMM"));
			if (lastMonthBudget != null)
			{
				lastMonthAmount = lastMonthBudget.GetTotalBudgetByMonth(period.GetEndTimeSpan());
			}

			return lastMonthAmount;
		}

		private int FirstMonthAmount(Period period)
		{
			var firstMonthBudget = _budgets.FirstOrDefault(x => x.YearMonth == period.Start.ToString("yyyyMM"));
			var firstMonthAmount = 0;
			if (firstMonthBudget != null)
			{
				firstMonthAmount = firstMonthBudget.GetTotalBudgetByMonth(period.GetStarTimeSpan());
			}

			return firstMonthAmount;
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
					var currentBudget = _budgets.FirstOrDefault(x => x.YearMonth == midMonthTime.ToString("yyyyMM"));
					if (currentBudget != null)
					{
						budgetTotalAmount += currentBudget.GetTotalBudgetByMonth(GetBudgetMonthDays(midMonthTime));
					}
				}
			}
			return budgetTotalAmount;
		}

		private IEnumerable<string> RemoveFirstAndLast(IEnumerable<string> diffMonths)
		{
			return diffMonths.Skip(1).Take(diffMonths.Count() - 2);
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