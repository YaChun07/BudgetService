using System;

namespace BudgetServiceTdd
{
	public class Budget
	{
		public string YearMonth { get; set; }
		public int Amount { get; set; }

		public DateTime FirstDay => DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);

		public DateTime LastDay => DateTime.ParseExact(YearMonth + GetDaysInMonth(), "yyyyMMdd", null);

		private int GetDaysInMonth() => DateTime.DaysInMonth(FirstDay.Year, FirstDay.Month);

		public int DailyAmount() => Amount / GetDaysInMonth();
	}
}