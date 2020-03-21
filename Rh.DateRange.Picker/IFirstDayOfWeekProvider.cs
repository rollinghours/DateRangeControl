// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;
using System.ComponentModel;

namespace Rh.DateRange.Picker {
    public interface IFirstDayOfWeekProvider : INotifyPropertyChanged {
        DayOfWeek Value { get; }
    }

}
