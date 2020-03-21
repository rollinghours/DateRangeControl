// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;

namespace Rh.DateRange.Picker.Common {
    public class RangeTheLastYear : IDateRange {

        public DateTime From { get; }

        public DateTime To { get; }

        public RangeTheLastYear() : this(DateTime.Now.AddYears(-1).AddDays(1), DateTime.Now) { }

        private RangeTheLastYear(DateTime from, DateTime to) {
            From = from.Date;
            To = to.Date;
        }

        public DateRangeKind Kind => DateRangeKind.TheLastYear;

        public IDateRange Refresh() => new RangeTheLastYear();

        public IDateRange GetNextRange() => new RangeTheLastYear(To.AddDays(1), To.AddYears(1));

        public IDateRange GetPreviousRange() => new RangeTheLastYear(From.AddYears(-1), From.AddDays(-1));
    }
}
