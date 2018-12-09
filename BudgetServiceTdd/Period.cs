using System;
using System.Collections.Generic;

namespace BudgetServiceTdd
{
	public class Period
	{
		public Period(DateTime start, DateTime end)
		{
			Start = start;
			End = end;
		}

		public DateTime Start { get; private set; }
		public DateTime End { get; private set; }

		public int GetEndTimeSpan()
		{
			return Start.Month != End.Month ? End.Day : 0;
		}

		public int GetStarTimeSpan()
		{
			return Start.Month == End.Month
				? End.Day - Start.Day + 1
				: DateTime.DaysInMonth(Start.Year, Start.Month) - Start.Day + 1;
		}

		public IEnumerable<string> GetMidMonths()
		{
			var currentMonth = new DateTime(Start.Year, Start.Month, 1);

			while (currentMonth <= End)
			{
				yield return currentMonth.ToString("yyyyMM");
				currentMonth = currentMonth.AddMonths(1);
			}
		}

		public int IntervalDays(Budget currentBudget)
		{
			int intervalDays;
			if (currentBudget.YearMonth == Start.ToString("yyyyMM"))
			{
				intervalDays = GetStarTimeSpan();
			}
			else if (currentBudget.YearMonth == End.ToString("yyyyMM"))
			{
				intervalDays = GetEndTimeSpan();
			}
			else
			{
				intervalDays = currentBudget.GetDaysInMonth();
			}

			return intervalDays;
		}
	}
}