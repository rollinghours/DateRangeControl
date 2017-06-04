// Copyright (c) Rollinghours.com owners. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;

namespace Rh.DateRange.Picker.Common {
    public class RangeLast7Days : RangeCustom {
        public RangeLast7Days() : base(DateTime.Now.AddDays(- 6), DateTime.Now) { }

        public override DateRangeKind Kind => DateRangeKind.Last7Days;

        public override IDateRange Refresh() => new RangeLast7Days();

    }
}
