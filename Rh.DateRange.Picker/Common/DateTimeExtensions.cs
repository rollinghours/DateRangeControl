// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;

namespace Rh.DateRange.Picker.Common {
    internal static class DateTimeExtensions {

        internal static DateTime GetWeekFirstDate(this DateTime date, DayOfWeek firstDayOfWeek) {
            if (firstDayOfWeek == date.DayOfWeek) {
                return date.Date;
            }
            int num = firstDayOfWeek < date.DayOfWeek ? date.DayOfWeek - firstDayOfWeek : 7 - (firstDayOfWeek - date.DayOfWeek);
            return date.AddDays(-num).Date;
        }

        internal static DateTime GetWeekLastDate(this DateTime date, DayOfWeek firstDayOfWeek) {
            return date.GetWeekFirstDate(firstDayOfWeek).AddDays(6).Date;
        }

        internal static DateTime GetMonthFirstDate(this DateTime date)
            => new DateTime(date.Year, date.Month, 1).Date;

        internal static DateTime GetMonthLastDate(this DateTime date) {
            return date.GetMonthFirstDate().AddMonths(1).AddDays(-1).Date;
        }

        internal static DateTime GetYearFirstDate(this DateTime date) {
            return new DateTime(date.Year, 1, 1).Date;
        }

        internal static DateTime GetYearLastDate(this DateTime date) {
            return new DateTime(date.Year, 12, 31).Date;
        }

    }
}
