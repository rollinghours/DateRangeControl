// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;

namespace Rh.DateRange.Picker.Common {

    public class RangeWeekToDate : RangeFirstDayOfWeekAware {

        public RangeWeekToDate(IFirstDayOfWeekProvider firstDayOfWeekProvider) :
            this(DateTime.Now.GetWeekFirstDate(firstDayOfWeekProvider.Value),
                DateTime.Now,
                firstDayOfWeekProvider) {
        }

        private RangeWeekToDate(DateTime from, DateTime to, IFirstDayOfWeekProvider firstDayOfWeekProvider) : base(firstDayOfWeekProvider) {
            From = from.Date;
            To = to.Date;
            Kind = DateRangeKind.WeekToDate;
        }

        public override IDateRange Refresh() {
            CleanUp();
            return new RangeWeekToDate(FirstDayOfWeekProvider);
        }

        public override IDateRange GetNextRange() {
            var nextFirstDate = From.GetWeekLastDate(FirstDayOfWeekProvider.Value).AddDays(1);
            return new RangeWeekToDate(nextFirstDate, nextFirstDate.AddDays((To - From).TotalDays), FirstDayOfWeekProvider);
        }

        public override IDateRange GetPreviousRange() {
            var prevFirstDate = From.AddDays(-1).GetWeekFirstDate(FirstDayOfWeekProvider.Value);
            return new RangeWeekToDate(prevFirstDate, prevFirstDate.AddDays((To - From).TotalDays), FirstDayOfWeekProvider);
        }

    }
}
