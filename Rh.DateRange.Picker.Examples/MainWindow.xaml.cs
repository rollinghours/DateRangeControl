// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using System.Windows;
using System.Windows.Data;

namespace Rh.DateRange.Picker.Examples {

    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e) {
            var enumerator = Picker.GetLocalValueEnumerator();
            while (enumerator.MoveNext()) {
                if (!BindingOperations.IsDataBound(Picker, enumerator.Current.Property)) {
                    Picker.ClearValue(enumerator.Current.Property);
                }
            }

            Picker.KindOfRange = 0;
        }
        
    }

}
