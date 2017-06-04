// Copyright (c) Rollinghours.com owners. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;

namespace Rh.DateRange.Picker.Common {
    public class RangeLastYear : IDateRange {

        public DateTime From { get; }

        public DateTime To { get; }

        public RangeLastYear() : this(new RangeYear().GetPreviousRange()) { }

        private RangeLastYear(IDateRange previousYearRange) {
            From = previousYearRange.From;
            To = previousYearRange.To;
        }

        public DateRangeKind Kind => DateRangeKind.LastYear;

        public IDateRange Refresh() => new RangeLastYear();

        public IDateRange GetNextRange() => new RangeLastYear(new RangeYear(To.AddDays(1)));

        public IDateRange GetPreviousRange() => new RangeLastYear(new RangeYear(From.AddDays(-1)));

    }
}
