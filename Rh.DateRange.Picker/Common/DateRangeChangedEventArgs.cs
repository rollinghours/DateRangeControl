// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;

namespace Rh.DateRange.Picker.Common {
    internal class DateRangeChangedEventArgs: EventArgs {
        public IDateRange NewValue { get; }
        public IDateRange OldValue { get; }

        public DateRangeChangedEventArgs(IDateRange oldValue, IDateRange newValue) {
            OldValue = oldValue ?? new RangeCustom();
            NewValue = newValue;
        }
    }

}
