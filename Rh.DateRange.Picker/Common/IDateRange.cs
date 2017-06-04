// Copyright (c) Rollinghours.com owners. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;

namespace Rh.DateRange.Picker.Common {
    public interface IDateRange {
        DateTime From { get; }
        DateTime To { get; }
        DateRangeKind Kind { get; }
        IDateRange Refresh();
        IDateRange GetPreviousRange();
        IDateRange GetNextRange();
    }
}
