using System;
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

				if (CheckBudgetEmpty(diffMonth.Substring(0, 4), diffMonth.Substring(4, 2)))
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

		private Budget GetBudget(DateTime date)
		{
			return _budgets.FirstOrDefault(x => int.Parse(x.YearMonth.Substring(4, 2)) == int.Parse(date.Month.ToString()));
		}

		private int GetTotalBudgetByMonth(int starTimeSpan, DateTime date)
		{
			if (!CheckBudgetEmpty(date.Year.ToString(), date.Month.ToString()))
			{
				return 0;
			}
			var budget = GetBudget(date);

			var budgetAmount = 0;

			if (budget != null)
			{
				budgetAmount = budget.Amount;
			}
			return starTimeSpan * budgetAmount / GetBudgetMonthDays(date);
		}

		private int GetBudgetMonthDays(DateTime date)
		{
			return DateTime.DaysInMonth(int.Parse(date.Year.ToString()), int.Parse(date.Month.ToString()));
		}

		private bool CheckBudgetEmpty(string year, string month)
		{
			var checkBudgetEmpty = _budgets.Any(x => int.Parse(x.YearMonth.Substring(4, 2)) == int.Parse(month)
													 && int.Parse(x.YearMonth.Substring(0, 4)) == int.Parse(year));
			return checkBudgetEmpty;
		}
	}
}