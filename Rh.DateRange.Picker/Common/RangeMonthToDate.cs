// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;

namespace Rh.DateRange.Picker.Common {
    public class RangeMonthToDate : IDateRange {

        public DateTime From { get; }

        public DateTime To { get; }

        private readonly int _dayCount;

        public RangeMonthToDate() {
            From = DateTime.Now.GetMonthFirstDate();
            To = DateTime.Now.Date;
            _dayCount = (int)(To - From).TotalDays + 1;
        }

        private RangeMonthToDate(DateTime from, int dayCount) {
            From = from;
            _dayCount = dayCount;

            var fromDate = from;
            var daysInMonth = DateTime.DaysInMonth(fromDate.Year, fromDate.Month);

            //align to the end of month if exceeded
            To = _dayCount > daysInMonth ? from.AddDays(daysInMonth - 1) : from.AddDays(_dayCount - 1);

        }

        public DateRangeKind Kind => DateRangeKind.MonthToDate;

        public IDateRange Refresh() => new RangeMonthToDate();

        public IDateRange GetNextRange() {
            var nextFirstDate = From.GetMonthLastDate().AddDays(1);
            return new RangeMonthToDate(nextFirstDate, _dayCount);
        }

        public IDateRange GetPreviousRange() {
            var prevFirstDate = From.AddDays(-1).GetMonthFirstDate();
            return new RangeMonthToDate(prevFirstDate, _dayCount);
        }
    }
}
