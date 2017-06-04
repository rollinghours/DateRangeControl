// Copyright (c) Rollinghours.com owners. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Rh.DateRange.Picker.Examples {

    public class StyleToIsCheckedConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return parameter.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return true == (bool?)value ? parameter as Style : null;
        }
    }
}
