// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rh.DateRange.Picker.Common {

    public class DateRangeList : List<RangePresenter> {

        private RangePresenter _current;

        internal event EventHandler<DateRangeChangedEventArgs> CurrentChanged;

        internal DateRangeList(RangePresenter[] ranges) {
            if (0 == ranges.Length) {
                throw new ArgumentException($"{nameof(ranges)} can not be empty.");
            }

            if (null == ranges.FirstOrDefault(dr => DateRangeKind.Custom == dr.Kind)) {
                throw new ArgumentException($"{nameof(ranges)} must contain an instance of the Custom date range.");
            }

            //making sure the ranges argument has no multiple ranges of the same kind 
            var r = ranges.Select(ds => ds.Kind).Count() == ranges.GroupBy(ds => ds.Kind).Count();
            if (!r) {
                throw new ArgumentException($"{nameof(ranges)} has multiple ranges of the same kind.");
            }

            AddRange(ranges);
            _current = GetItemByKind(DateRangeKind.Custom);
        }

        protected RangePresenter GetItemByKind(DateRangeKind? kind) 
            => this.First(rp => kind == rp.Kind);

        internal DateRangeKind? CurrentKindOfRange {
            get { return _current.Kind; }
            set {
                if ((null != Current) && Current.Kind.Equals(value)) {
                    return;
                }
                Current = (RangePresenter)GetItemByKind(value)?.Refresh();
            }
        }

        internal RangePresenter Current {
            get { return _current; }
            private set {
                if ((null != value) && value.Equals(_current)) {
                    return;
                }
                var previous = _current;
                _current = value;
                CurrentChanged?.Invoke(this, new DateRangeChangedEventArgs(previous, _current));
            }
        }

        internal void SetPreviousRange() {
            SetCustomRange(_current.GetPreviousRange());
        }

        internal void SetNextRange() {
            SetCustomRange(_current.GetNextRange());
        }

        private void SetCustomRange(IDateRange dateRange) {
            if (null == dateRange) { throw new ArgumentNullException(nameof(dateRange)); }

            var custom = GetItemByKind(DateRangeKind.Custom);
            var customIndex = IndexOf(custom);
            var forceCustom = true;
            this[customIndex] = new RangePresenter(custom.ResourceManager, dateRange, forceCustom);

            Current = this[customIndex];
        }

        internal void SetCustomWithNewFromDate(DateTime from) {
            if (Current.From != from) {
                var to = from <= Current.To ? Current.To : from;
                SetCustomRange(new RangeCustom(from, to));
            }
        }

        internal void SetCustomWithNewToDate(DateTime to) {
            if (Current.To != to) {
                var from = to >= Current.From ? Current.From : to;
                SetCustomRange(new RangeCustom(from, to));
            }
        }

    }
}
