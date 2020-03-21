// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;
using System.Resources;
using System.Globalization;

namespace Rh.DateRange.Picker.Common {
    public static class DateRangeHelper {

        internal static DateRangeList BuildDateRangeList(IFirstDayOfWeekProvider firstDayOfWeekProvider, ResourceManager resourceManager = null) {
            return new DateRangeList(new[] {
                new RangePresenter(resourceManager, new RangeDay()),
                new RangePresenter(resourceManager, new RangeYesterday()),
                new RangePresenter(resourceManager, new RangeWeekToDate(firstDayOfWeekProvider)),
                new RangePresenter(resourceManager, new RangeMonthToDate()),
                new RangePresenter(resourceManager, new RangeYearToDate()),
                new RangePresenter(resourceManager, new RangeLastWeek(firstDayOfWeekProvider)),
                new RangePresenter(resourceManager, new RangeLastMonth()),
                new RangePresenter(resourceManager, new RangeLastYear()),
                new RangePresenter(resourceManager, new RangeLast7Days()),
                new RangePresenter(resourceManager, new RangeLast30Days()),
                new RangePresenter(resourceManager, new RangeTheLastYear()),
                new RangePresenter(resourceManager, new RangeCustom())
            });
        }


        /// <summary>
        /// Returns recalculated date range as DateRangeValue for the given input parameters. 
        /// </summary>
        /// <param name="rangeKind">The kind of range to calculate.</param>
        /// <param name="from">Start DateTime of the range. Has sense only if the rangeKind parameter is set to Custom.</param>
        /// <param name="to">End DateTime of the range. Has sense only if the rangeKind parameter is set to Custom.</param>
        public static DateRangeValue GetDateRangeDates(DateRangeKind rangeKind, DateTime from, DateTime to) {
            return GetDateRangeDates(rangeKind, from, to, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
        }

        public static DateRangeValue GetDateRangeDates(DateRangeKind rangeKind, DateTime from, DateTime to, DayOfWeek firstDayOfWeek) {
            if (DateRangeKind.Custom == rangeKind) {
                return new DateRangeValue(from, to);
            }

            var range = GetDateRangeOfKind(rangeKind, firstDayOfWeek);
            return new DateRangeValue(range.From, range.To);
        }


        /// <summary>
        /// Returns DateRange with date values calculated for the given input rangeKind parameter. 
        /// </summary>
        internal static IDateRange GetDateRangeOfKind(DateRangeKind rangeKind) {
            return GetDateRangeOfKind(rangeKind, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
        }

        private static IDateRange GetDateRangeOfKind(DateRangeKind rangeKind, DayOfWeek firstDayOfWeek) {
            var firstDayOfWeekProvider = new FirstDayOfWeekProvider() { Value = firstDayOfWeek };
            var list = BuildDateRangeList(firstDayOfWeekProvider);
            list.CurrentKindOfRange = rangeKind;
            return list.Current;
        }
    }
}
