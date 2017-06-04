// Copyright (c) Rollinghours.com owners. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;

namespace Rh.DateRange.Picker.Common {

    public class RangeMonth : IDateRange {

        public DateTime From { get; }

        public DateTime To { get; }

        public RangeMonth() : this(DateTime.Now) { }

        public RangeMonth(DateTime dateTime) {
            From = dateTime.GetMonthFirstDate();
            To = dateTime.GetMonthLastDate();
        }

        public DateRangeKind Kind => DateRangeKind.Month;

        public IDateRange Refresh() => new RangeMonth();

        public IDateRange GetNextRange() => new RangeMonth(To.AddDays(1));

        public IDateRange GetPreviousRange() => new RangeMonth(From.AddDays(-1));

    }

}
