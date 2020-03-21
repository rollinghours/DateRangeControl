// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;

namespace Rh.DateRange.Picker.Common {

    public class RangeWeek : RangeFirstDayOfWeekAware {

        public RangeWeek(IFirstDayOfWeekProvider firstDayOfWeekProvider) : this(DateTime.Now, firstDayOfWeekProvider) { }

        public RangeWeek(DateTime date, IFirstDayOfWeekProvider firstDayOfWeekProvider) : base(firstDayOfWeekProvider) {
            From = date.GetWeekFirstDate(firstDayOfWeekProvider.Value);
            To = date.GetWeekLastDate(firstDayOfWeekProvider.Value);
            Kind = DateRangeKind.Week;
        }


        public override IDateRange Refresh() {
            CleanUp();
            return new RangeWeek(FirstDayOfWeekProvider);
        }

        public override IDateRange GetNextRange() => new RangeWeek(To.AddDays(1), FirstDayOfWeekProvider);

        public override IDateRange GetPreviousRange() => new RangeWeek(From.AddDays(-1), FirstDayOfWeekProvider);

    }

}
