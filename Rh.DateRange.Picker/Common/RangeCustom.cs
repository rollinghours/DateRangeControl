// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;


namespace Rh.DateRange.Picker.Common {

    public class RangeCustom : IDateRange {
        private readonly int _dayCount;

        public DateTime From { get; }

        public DateTime To { get; }

        public RangeCustom() : this(DateTime.Now.AddDays(-1).Date, DateTime.Now.Date) { }

        public RangeCustom(DateTime from, DateTime to, int dayCount = 0) {
            if (from > to) { throw new ArgumentException($"{nameof(from)} is later than {nameof(to)}"); }

            From = from.Date;
            To = to.Date;
            _dayCount = dayCount == 0 ? (int)(To - From).TotalDays + 1 : dayCount;
        }

        public virtual DateRangeKind Kind => DateRangeKind.Custom;

        public virtual IDateRange Refresh() => new RangeCustom(From, To);

        public virtual IDateRange GetNextRange() {
            DateTime from = To.AddDays(1);
            DateTime to = _dayCount > 0 ? from.AddDays(_dayCount - 1) : from.Add(To - From);
            return new RangeCustom(from, to, _dayCount);
        }

        public virtual IDateRange GetPreviousRange() {
            DateTime to = From.AddDays(-1);
            DateTime from = _dayCount > 0 ? to.AddDays(-_dayCount + 1) : to.AddDays(-(To - From).TotalDays + 1);
            return new RangeCustom(from, to, _dayCount);
        }

    }

}



