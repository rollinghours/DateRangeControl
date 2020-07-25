// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using NUnit.Framework;
using Rh.DateRange.Picker.Common;
using System;
using System.Collections.Generic;

namespace Rh.DateRange.Picker.Tests {
    [TestFixture()]
    public class DateRangeHelperTests {

        private static DateTime _fromIn;
        private static DateTime _toIn;
        private static DateTime _today;
        private static DateTime _yesterday;
        private static DateTime _daysBack30;
        private static DateTime _daysBack7;
        private static DateTime _weekFirstDay;
        private static DateTime _weekLastDay;
        private static DateTime _lastWeekFirstDay;
        private static DateTime _lastWeekLastDay;
        private static DateTime _monthFirstDay;
        private static DateTime _monthLastDay;
        private static DateTime _lastMonthFirstDay;
        private static DateTime _lastMonthLastDay;
        private static DateTime _yearFirstDay;
        private static DateTime _yearLastDay;
        private static DateTime _yearBack;
        private static DateTime _lastYearFirstDay;
        private static DateTime _lastYearLastDay;

        //TestCaseSource'd test runs before SetUp() and SetUpFixture(), thus initialising in the static constructor
        static DateRangeHelperTests() {
            _fromIn = new DateTime(2000, 1, 5, 0, 0, 0);
            _toIn = new DateTime(2000, 2, 2, 0, 0, 0);

            var firstDayOfWeek = new FirstDayOfWeekProvider().Value;

            _today = DateTime.Now.Date;
            _yesterday = _today.AddDays(-1);
            _daysBack30 = _today.AddDays(-29);
            _daysBack7 = _today.AddDays(-6);
            _weekFirstDay = _today.GetWeekFirstDate(firstDayOfWeek).Date;
            _weekLastDay = _weekFirstDay.AddDays(6);
            _lastWeekFirstDay = _today.GetWeekFirstDate(firstDayOfWeek).Date.AddDays(-7);
            _lastWeekLastDay = _today.GetWeekFirstDate(firstDayOfWeek).Date.AddDays(-1);
            _monthFirstDay = _today.GetMonthFirstDate();
            _monthLastDay = _today.GetMonthLastDate();
            _lastMonthFirstDay = _today.AddMonths(-1).GetMonthFirstDate();
            _lastMonthLastDay = _today.AddMonths(-1).GetMonthLastDate();
            _yearFirstDay = _today.GetYearFirstDate();
            _yearLastDay = _today.GetYearLastDate();
            _yearBack = _today.AddYears(-1).AddDays(1);
            _lastYearFirstDay = _today.GetYearFirstDate().AddYears(-1);
            _lastYearLastDay = _today.GetYearLastDate().AddYears(-1);
        }

        private static IEnumerable<TestCaseData> GetSpanDatesTestCases() {
            yield return new TestCaseData(DateRangeKind.Custom, new DateRangeValue(_fromIn, _toIn));
            yield return new TestCaseData(DateRangeKind.Today, new DateRangeValue(_today, _today));
            yield return new TestCaseData(DateRangeKind.Yesterday, new DateRangeValue(_yesterday, _yesterday));
            yield return new TestCaseData(DateRangeKind.Last30Days, new DateRangeValue(_daysBack30, _today));
            yield return new TestCaseData(DateRangeKind.Last7Days, new DateRangeValue(_daysBack7, _today));
            yield return new TestCaseData(DateRangeKind.TheLastYear, new DateRangeValue(_yearBack, _today));
            yield return new TestCaseData(DateRangeKind.MonthToDate, new DateRangeValue(_monthFirstDay, _today));
            yield return new TestCaseData(DateRangeKind.LastMonth, new DateRangeValue(_lastMonthFirstDay, _lastMonthLastDay));
            yield return new TestCaseData(DateRangeKind.LastWeek, new DateRangeValue(_lastWeekFirstDay, _lastWeekLastDay));
            yield return new TestCaseData(DateRangeKind.LastYear, new DateRangeValue(_lastYearFirstDay, _lastYearLastDay));
            yield return new TestCaseData(DateRangeKind.WeekToDate, new DateRangeValue(_weekFirstDay, _today));
            yield return new TestCaseData(DateRangeKind.YearToDate, new DateRangeValue(_yearFirstDay, _today));
        }

        [Test(), TestCaseSource(nameof(GetSpanDatesTestCases))]
        public void GetSpanDatesTest(DateRangeKind kind, DateRangeValue expected) {
            var result = DateRangeHelper.GetDateRangeDates(kind, _fromIn, _toIn);
            Assert.AreEqual(expected, result);
        }

    }
}
