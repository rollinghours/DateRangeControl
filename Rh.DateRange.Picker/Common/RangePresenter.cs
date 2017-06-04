// Copyright (c) Rollinghours.com owners. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System;
using System.Resources;

namespace Rh.DateRange.Picker.Common {

    public class RangePresenter : IDateRange {

        private IDateRange _dateRange;

        private readonly bool _forceCustom;

        public DateTime From => _dateRange.From;

        public DateTime To => _dateRange.To;

        public ResourceManager ResourceManager { get; }

        public RangePresenter(ResourceManager resourceManager, IDateRange range, bool forceCustom = false) {
            _dateRange = range;
            _forceCustom = forceCustom;
            ResourceManager = resourceManager;
        }

        //SelectedValuePath for the kind of range combobox
        public DateRangeKind Kind => _forceCustom ? DateRangeKind.Custom : _dateRange.Kind;

        //DisplayMemberPath for the kind of range combobox
        public string DisplayName {
            get {
                var s = ResourceManager?.GetString(Kind.ToString());
                return string.IsNullOrWhiteSpace(s) ? $"{Kind.ToString()}" : s;
            }
        }

        //the underlying type of the decorated range remains the same to keep the correct steps for prev/next range getting
        public IDateRange GetPreviousRange() => _dateRange.GetPreviousRange();

        public IDateRange GetNextRange() => _dateRange.GetNextRange();

        public IDateRange Refresh() {
            _dateRange = _dateRange.Refresh();
            return this;
        }
    }

}
