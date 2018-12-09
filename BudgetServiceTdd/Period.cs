using System;

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

		public int IntervalDays(Budget budget)
		{
			if (InvalidDate() || NoOverlappingDays(budget))
			{
				return 0;
			}

			var intervalStart = Start > budget.FirstDay ? Start : budget.FirstDay;
			var intervalEnd = End < budget.LastDay ? End : budget.LastDay;

			return intervalEnd.Subtract(intervalStart).Days + 1;
		}

		private bool InvalidDate()
		{
			return Start > End;
		}

		private bool NoOverlappingDays(Budget budget)
		{
			return budget.FirstDay > End || budget.LastDay < Start;
		}
	}
}