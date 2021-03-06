﻿using System;
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

            return GetBudgetTotalAmount(start, end);
        }

        private int GetBudgetTotalAmount(DateTime start, DateTime end)
        {
            int starTimeSpan, endTimeSpan;

            if (start.Month == end.Month)
            {
                starTimeSpan = end.Day - start.Day + 1;
                endTimeSpan = 0;
            }
            else
            {
                starTimeSpan = GetBudgetMonthDays(start) - start.Day + 1;
                endTimeSpan = end.Day;
            }

            return GetMinMonthBudget(start, end) 
                   + GetTotalBudgetByMonth(starTimeSpan, start)
                   + GetTotalBudgetByMonth(endTimeSpan, end);
        }

        private int GetMinMonthBudget(DateTime start, DateTime end)
        {
            var budgetTotalAmount = 0;
            var diffMonths = GetMidMonths(start, end);

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

        private IEnumerable<string> GetMidMonths(DateTime start, DateTime end)
        {
            var startTime = new DateTime(start.Year, start.Month, 1);

            while (startTime <= end)
            {
                yield return startTime.Year + startTime.Month.ToString("D2");
                startTime = startTime.AddMonths(1);
            }
        }
    }
}