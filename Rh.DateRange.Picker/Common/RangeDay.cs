// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;

namespace Rh.DateRange.Picker.Common {

    public class RangeDay : IDateRange {
        public DateTime From { get; }

        public DateTime To { get; }

        public RangeDay() : this(DateTime.Now) { }

        public RangeDay(DateTime dateTime) {
            From = To = dateTime.Date;
        }

        public DateRangeKind Kind => DateRangeKind.Today;

        public IDateRange Refresh() => new RangeDay();

        public IDateRange GetNextRange() => new RangeDay(To.AddDays(1));

        public IDateRange GetPreviousRange() => new RangeDay(From.AddDays(-1));

    }

}
