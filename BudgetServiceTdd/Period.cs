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

		public int GetEndTimeSpan()
		{
			var endTimeSpan = 0;
			if (Start.Month != End.Month)
			{
				endTimeSpan = End.Day;
			}

			return endTimeSpan;
		}
	}
}