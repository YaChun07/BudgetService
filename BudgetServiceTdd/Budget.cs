using System;

namespace BudgetServiceTdd
{
	public class Budget
	{
		public string YearMonth { get; set; }
		public int Amount { get; set; }

		public DateTime CurrentDateTime => DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);

		public DateTime FirstDay => CurrentDateTime;

		public DateTime LastDay => DateTime.ParseExact(YearMonth + GetDaysInMonth(), "yyyyMMdd", null);

		public int GetDaysInMonth() => DateTime.DaysInMonth(CurrentDateTime.Year, CurrentDateTime.Month);

		public int DailyAmount() => Amount / GetDaysInMonth();
	}
}