// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;
using System.ComponentModel;

namespace Rh.DateRange.Picker.Common {
    public abstract class RangeFirstDayOfWeekAware : IDateRange {

        protected readonly IFirstDayOfWeekProvider FirstDayOfWeekProvider;

        public DateTime From { get; protected set; }

        public DateTime To { get; protected set; }

        public DateRangeKind Kind { get; protected set; }

        protected RangeFirstDayOfWeekAware(IFirstDayOfWeekProvider firstDayOfWeekProvider) {
            FirstDayOfWeekProvider = firstDayOfWeekProvider;
            FirstDayOfWeekProvider.PropertyChanged += ReFirstDayOfWeekChanged;
        }

        protected void CleanUp() {
            FirstDayOfWeekProvider.PropertyChanged -= ReFirstDayOfWeekChanged;
        }


        public abstract IDateRange GetNextRange();

        public abstract IDateRange GetPreviousRange();

        public abstract IDateRange Refresh();

        private void ReFirstDayOfWeekChanged(object sender, PropertyChangedEventArgs e) {
            var provider = sender as IFirstDayOfWeekProvider;
            if ((null != provider) && nameof(provider.Value).Equals(e.PropertyName)) {
                Refresh();
            }

        }

    }
}
