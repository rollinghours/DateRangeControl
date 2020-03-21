// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
namespace Rh.DateRange.Picker.Common {
    public static class DateRangeExtensions {
        public static DateRangeValue GetDateRangeValue(this IDateRange dateRange) {
            return new DateRangeValue(dateRange.From, dateRange.To);
        }

    }
}
