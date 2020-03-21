// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;

namespace Rh.DateRange.Picker.Common {
    public class RangeLast30Days : RangeCustom {

        public RangeLast30Days() : base(DateTime.Now.AddDays(-29), DateTime.Now) { }

        public override DateRangeKind Kind => DateRangeKind.Last30Days;

        public override IDateRange Refresh() => new RangeLast30Days();

    }
}
