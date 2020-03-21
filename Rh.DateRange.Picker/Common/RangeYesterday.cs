// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;

namespace Rh.DateRange.Picker.Common {

    public class RangeYesterday : IDateRange {
        public DateTime From { get; }

        public DateTime To { get; }

        public RangeYesterday() : this(DateTime.Now.AddDays(-1)) { }

        private RangeYesterday(DateTime dateTime) {
            From = To = dateTime.Date;
        }

        public DateRangeKind Kind => DateRangeKind.Yesterday;

        public IDateRange Refresh() => new RangeYesterday();

        public IDateRange GetNextRange() => new RangeYesterday(To.AddDays(1));

        public IDateRange GetPreviousRange() => new RangeYesterday(From.AddDays(-1));

    }
}
