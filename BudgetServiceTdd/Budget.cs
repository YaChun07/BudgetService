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
			var daysInMonth = DateTime.DaysInMonth(CurrentDateTime.Year, CurrentDateTime.Month);
			return daysInMonth;
		}

		public int DailyAmount()
		{
			var dailyAmount = Amount / DaysInMonth();
			return dailyAmount;
		}

		public int GetTotalBudgetByMonth(int starTimeSpan)
		{
			if (this != null)
			{
				var dailyAmount = DailyAmount();
				return starTimeSpan * dailyAmount;
			}

			return 0;
		}
	}
}