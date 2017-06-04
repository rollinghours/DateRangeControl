// Copyright (c) Rollinghours.com owners. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;

namespace Rh.DateRange.Picker.Common {

    public class RangeYear : IDateRange {

        public DateTime From { get; }

        public DateTime To { get; }

        public RangeYear() : this(DateTime.Now) { }

        public RangeYear(DateTime date) {
            From = date.GetYearFirstDate();
            To = date.GetYearLastDate();
        }

        public DateRangeKind Kind => DateRangeKind.Year;

        public IDateRange Refresh() => new RangeYear();

        public IDateRange GetNextRange() => new RangeYear(To.AddDays( 1));

        public IDateRange GetPreviousRange() => new RangeYear(From.AddDays( - 1));

    }

}
