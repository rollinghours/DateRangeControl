// Copyright (c) Rollinghours.com owners. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;
using System.ComponentModel;
using System.Globalization;

namespace Rh.DateRange.Picker {
    internal class FirstDayOfWeekProvider : IFirstDayOfWeekProvider {
        private DayOfWeek _firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

        public DayOfWeek Value {
            get {
                return _firstDayOfWeek;
            }
            set {
                _firstDayOfWeek = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
