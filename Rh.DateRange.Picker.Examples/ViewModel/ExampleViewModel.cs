// Copyright (c) Rollinghours.com owners. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using Rh.DateRange.Picker.Common;
using System;
using System.ComponentModel;

namespace Rh.DateRange.Picker.Examples.ViewModel {

    public class ExampleViewModel : INotifyPropertyChanged {

        private DateTime _rangeFrom;
        public DateTime RangeFrom {
            get { return _rangeFrom; }
            set {
                _rangeFrom = value;
                RaisePropertyChanged(nameof(RangeFrom));
            }
        }

        private DateTime _rangeTo;
        public DateTime RangeTo {
            get { return _rangeTo; }
            set {
                _rangeTo = value;
                RaisePropertyChanged(nameof(RangeTo));
            }
        }

        private DateRangeKind _kindOfRange;
        public DateRangeKind KindOfRange {
            get { return _kindOfRange; }
            set {
                _kindOfRange = value;
                RaisePropertyChanged(nameof(KindOfRange));
            }

        }


        private string _status;
        public string Status {
            get { return _status; }
            set {
                _status = value;
                RaisePropertyChanged(nameof(Status));
            }
        }

        private int _commandCount;
        private RelayCommand _command;
        public RelayCommand DateRangeCommand => _command ?? 
            (_command = new RelayCommand(r => { Status = $"Command fired {_commandCount++} times."; }));

        #region INPC-re

        private void RaisePropertyChanged(string v) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

    }


}
