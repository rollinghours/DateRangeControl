// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
namespace Rh.DateRange.Picker.Common {
    public class RangeLastWeek : RangeFirstDayOfWeekAware {

        public RangeLastWeek(IFirstDayOfWeekProvider firstDayOfWeekProvider) :
            this((RangeWeek)new RangeWeek(firstDayOfWeekProvider).GetPreviousRange(), firstDayOfWeekProvider) { }

        private RangeLastWeek(RangeWeek previousWeekRange, IFirstDayOfWeekProvider firstDayOfWeekProvider) : base(firstDayOfWeekProvider) {
            From = previousWeekRange.From;
            To = previousWeekRange.To;
            Kind = DateRangeKind.LastWeek;
        }

        public override IDateRange Refresh() {
            CleanUp();
            return new RangeLastWeek(FirstDayOfWeekProvider);
        }

        public override IDateRange GetNextRange() => new RangeLastWeek(new RangeWeek(To.AddDays(1), FirstDayOfWeekProvider), FirstDayOfWeekProvider);

        public override IDateRange GetPreviousRange() => new RangeLastWeek(new RangeWeek(From.AddDays(-1), FirstDayOfWeekProvider), FirstDayOfWeekProvider);

    }

}
