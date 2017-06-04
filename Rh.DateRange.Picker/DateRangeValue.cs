// Copyright (c) Rollinghours.com owners. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;

namespace Rh.DateRange.Picker {
    public struct DateRangeValue {
        public DateTime From { get; }
        public DateTime To { get; }

        public DateRangeValue(DateTime from, DateTime to) {
            From = from;
            To = to;
        }

        public override string ToString() =>
            string.Concat(From.ToShortDateString(), " - ", To.ToShortDateString());
    }
}
