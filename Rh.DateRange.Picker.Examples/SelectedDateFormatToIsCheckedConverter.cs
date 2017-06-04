// Copyright (c) Rollinghours.com owners. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Rh.DateRange.Picker.Examples {
    class SelectedDateFormatToIsCheckedConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return (DatePickerFormat)value == DatePickerFormat.Short;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return (bool?)value == true ? DatePickerFormat.Short : DatePickerFormat.Long;
        }
    }

}
