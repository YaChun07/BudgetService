using System;

namespace BudgetServiceTdd
{
	public class Budget
	{
		public string YearMonth { get; set; }
		public int Amount { get; set; }

		public DateTime CurrentDateTime => DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);

		public int DaysInMonth()
		{
			return DateTime.DaysInMonth(CurrentDateTime.Year, CurrentDateTime.Month);
		}

		public int GetAmountByIntervalDays(int intervalDays)
		{
			return intervalDays * (Amount / DaysInMonth());
		}
	}
}