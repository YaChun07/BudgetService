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
            if (start.Ticks > end.Ticks) return 0;

            if (!CheckBudgetEmpty(start.Month.ToString()) && !CheckBudgetEmpty(end.Month.ToString()))
            {
                return 0;
            }
            return GetBudgetTotalAmount(start, end);
        }

        private int GetBudgetTotalAmount(DateTime start, DateTime end)
        {
            var startBudget = _budgets.FirstOrDefault(x => int.Parse(x.YearMonth.Substring(4, 2)) == int.Parse(start.Month.ToString()));

            var endBudget = _budgets.FirstOrDefault(x => int.Parse(x.YearMonth.Substring(4, 2)) == int.Parse(end.Month.ToString()));

            int starTimeSpan, endTimeSpan;

            if (start.Month == end.Month)
            {
                starTimeSpan = end.Day - start.Day + 1;
                endTimeSpan = 0;
            }
            else
            {
                starTimeSpan = GetBudgetMonthDays(startBudget) - start.Day + 1;
                endTimeSpan = end.Day;
            }

            return GetTotalBudgetByMonth(starTimeSpan, startBudget)
                   + GetTotalBudgetByMonth(endTimeSpan, endBudget);
        }

        private int GetTotalBudgetByMonth(int starTimeSpan, Budget startBudget)
        {
            return starTimeSpan * startBudget.Amount / GetBudgetMonthDays(startBudget);
        }

        private int GetBudgetMonthDays(Budget budget)
        {
            return DateTime.DaysInMonth(int.Parse(budget.YearMonth.Substring(0, 4)), int.Parse(budget.YearMonth.Substring(4, 2)));
        }

        private bool CheckBudgetEmpty(string month)
        {
            return _budgets.Any(x => int.Parse(x.YearMonth.Substring(4, 2)) == int.Parse(month));
        }
    }
}