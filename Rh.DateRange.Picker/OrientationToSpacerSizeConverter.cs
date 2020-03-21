// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Rh.DateRange.Picker {

    public class OrientationToSpacerSizeConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object targetOrientation, CultureInfo culture) {
            if (2 != values.Length) { throw new ArgumentException("values[]"); }

            if (!(values[0] is Orientation)) { throw new ArgumentException("values[0]"); }
            var orientation = (Orientation)values[0];

            if (!(targetOrientation is Orientation)) { throw new ArgumentException(nameof(targetOrientation)); }

            if ((Orientation)targetOrientation == orientation) {
                if (!(values[1] is double)) { throw new ArgumentException("values[1] must be a double"); }
                return new GridLength((double)values[1]);
            }

            return new GridLength(0.0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            return value as object[];
        }

    }

}
