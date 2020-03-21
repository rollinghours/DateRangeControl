// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;

namespace Rh.DateRange.Picker.Common {
    public class RangeLastMonth : IDateRange {

        public DateTime From { get; }

        public DateTime To { get; }

        public RangeLastMonth() : this(new RangeMonth().GetPreviousRange()) { }

        private RangeLastMonth(IDateRange previousMonthRange) {
            From = previousMonthRange.From;
            To = previousMonthRange.To;
        }

        public DateRangeKind Kind => DateRangeKind.LastMonth;

        public IDateRange Refresh() => new RangeLastMonth();

        public IDateRange GetNextRange() => new RangeLastMonth(new RangeMonth(To.AddDays(1)));

        public IDateRange GetPreviousRange() => new RangeLastMonth(new RangeMonth(From.AddDays(-1)));

    }

}
