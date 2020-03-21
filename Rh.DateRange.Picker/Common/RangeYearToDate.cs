// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;

namespace Rh.DateRange.Picker.Common {
    public class RangeYearToDate : IDateRange {
        private readonly DateTime _initialTo;

        public DateTime From { get; }

        public DateTime To { get; }

        //public int Length => To - From + 1;

        public RangeYearToDate() {
            From = DateTime.Now.GetYearFirstDate();
            _initialTo = To = DateTime.Now.Date;
        }

        private RangeYearToDate(DateTime from, DateTime initialTo) {
            From = from.Date;
            _initialTo = initialTo.Date;

            var initialToDayOfMonth =
             (2 == _initialTo.Month) && (29 == _initialTo.Day) && !DateTime.IsLeapYear(From.Year)
             ? 28
             : _initialTo.Day;

            To = new DateTime(From.Year, _initialTo.Month, initialToDayOfMonth);

        }

        public DateRangeKind Kind => DateRangeKind.YearToDate;


        public IDateRange Refresh() => new RangeYearToDate();

        public IDateRange GetNextRange() {
            var nextFirstDate = From.GetYearLastDate().AddDays(1);
            return new RangeYearToDate(nextFirstDate, _initialTo);
        }

        public IDateRange GetPreviousRange() {
            var prevFirstDate = From.AddDays(-1).GetYearFirstDate();
            return new RangeYearToDate(prevFirstDate, _initialTo);
        }

    }
}
