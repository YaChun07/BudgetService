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

			var firstMonthBudget = _budgets.FirstOrDefault(x => x.YearMonth == period.Start.ToString("yyyyMM"));
			int firstMonthAmount = 0;
			if (firstMonthBudget != null)
			{
				firstMonthAmount = firstMonthBudget.GetAmountByIntervalDays(period.GetStarTimeSpan());
			}

			var lastMonthBudget = _budgets.FirstOrDefault(x => x.YearMonth == period.End.ToString("yyyyMM"));
			int lastMonthAmount = 0;
			if (lastMonthBudget != null)
			{
				lastMonthAmount = lastMonthBudget.GetAmountByIntervalDays(period.GetEndTimeSpan());
			}

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
						budgetTotalAmount += currentBudget.GetAmountByIntervalDays(currentBudget.DaysInMonth());
					}
				}
			}

			return budgetTotalAmount + firstMonthAmount + lastMonthAmount;
		}

		private IEnumerable<string> RemoveFirstAndLast(IEnumerable<string> diffMonths)
		{
			return diffMonths.Skip(1).Take(diffMonths.Count() - 2);
		}

		private bool CheckBudgetEmpty(string yearMonth)
		{
			return _budgets.Any(x => x.YearMonth == yearMonth);
		}
	}
}