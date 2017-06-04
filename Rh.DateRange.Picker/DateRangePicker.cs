// Copyright (c) Rollinghours.com owners. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using Rh.DateRange.Picker.Resources;
using Rh.DateRange.Picker.Common;
using System;
using System.Globalization;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;

namespace Rh.DateRange.Picker {
    public class DateRangePicker : Control, ICommandSource {

        private const string PrevRangeButtonName = "PART_PrevRangeButton";
        private const string NextRangeButtonName = "PART_NextRangeButton";
        private const string FromTextBlockName = "PART_FromTextBlock";
        private const string ToTextBlockName = "PART_ToTextBlock";
        private const string RangeComboBoxName = "PART_RangeComboBox";

        private const string FromTextBlockResourceKey = "From";
        private const string ToTextBlockResourceKey = "To";

        private const double DefaultSpacerValue = 5.0;

        private static readonly DateRangeKind DefaultKindOfRange;
        private readonly ResourceManager _resourceManager;
        private readonly FirstDayOfWeekProvider _firstDayOfWeekProvider;

        private DateRangeList _dateRanges;
        public ReadOnlyCollection<RangePresenter> DateRanges {
            get {
                //public collection property should not be null
                //return (_dateRanges ?? new List<RangePresenter>()).AsReadOnly();

                return _dateRanges?.AsReadOnly();
            }
        }

        static DateRangePicker() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateRangePicker), new FrameworkPropertyMetadata(typeof(DateRangePicker)));
            DefaultKindOfRange = (DateRangeKind)Enum.ToObject(typeof(DateRangeKind), 0);
        }

        public DateRangePicker() {
            _resourceManager = new ResourceManager(typeof(Resource));
            _firstDayOfWeekProvider = new FirstDayOfWeekProvider();
        }

        public override void OnApplyTemplate() {
            // base.OnApplyTemplate(); //not needed if deriving from Control

            PrevRangeButton = GetTemplateChild(PrevRangeButtonName) as Button;
            NextRangeButton = GetTemplateChild(NextRangeButtonName) as Button;

            SetTextBlockLocalizedText(FromTextBlockName, FromTextBlockResourceKey);
            SetTextBlockLocalizedText(ToTextBlockName, ToTextBlockResourceKey);

            if (null == _dateRanges) {
                _dateRanges = DateRangeHelper.BuildDateRangeList(_firstDayOfWeekProvider, _resourceManager);
                _dateRanges.CurrentChanged += OnCurrentChanged;
            }

            InitDatePropertiesSetByUser();

            var rangeComboBox = GetTemplateChild(RangeComboBoxName) as ItemsControl;
            var bindingExpression = rangeComboBox?.GetBindingExpression(ItemsControl.ItemsSourceProperty);
            bindingExpression?.UpdateTarget();
        }


        /*
         * The basic rules for initializing the KindOfRange, From and To properties within the OnApplyTemplate: 
         * 1) A non-Custom KindOfRange setting has the top priority: if user sets Range, then the user settings of From and To don't matter.
         * 2) If user doesn't set the Range property but does set the From or To, the range kind gets automatically set to Custom.
         * 3) If the User sets the From/To values and the range to Custom, the From/To properties accept user settings.
         * 4) If both the From and To are set by user and From > To, the To := From.
         * 5) Default values for the Custom range are From=[yesterday], To=[today]. 
         */
        private void InitDatePropertiesSetByUser() {

            var toSetByUser = To;
            var toIsSetByUser = new DateTime(0) != toSetByUser;

            var fromSetByUser = From;
            var fromIsSetByUser = new DateTime(0) != fromSetByUser;

            var rangeSetByUser = KindOfRange;
            var rangeIsSetByUser = null != rangeSetByUser;

            if (toIsSetByUser) {
                _dateRanges.SetCustomWithNewToDate(toSetByUser);
            }
            if (fromIsSetByUser) {
                _dateRanges.SetCustomWithNewFromDate(fromSetByUser);
            }
            if (rangeIsSetByUser) {
                if (DateRangeKind.Custom != rangeSetByUser) {
                    _dateRanges.CurrentKindOfRange = rangeSetByUser;
                    return;
                }

                if (!toIsSetByUser) {
                    To = _dateRanges.Current.To;
                }

                if (!fromIsSetByUser) {
                    From = _dateRanges.Current.From;
                }

                return;
            }

            KindOfRange = !toIsSetByUser && !fromIsSetByUser ? DefaultKindOfRange : DateRangeKind.Custom;

        }


        private void OnCurrentChanged(object sender, DateRangeChangedEventArgs e) {
            if (null == e.NewValue) { return; }

            To = e.NewValue.To;
            From = e.NewValue.From;
            KindOfRange = e.NewValue.Kind;

            var oldRangeValue = (e.OldValue ?? new RangeCustom()).GetDateRangeValue();
            var newRangeValue = e.NewValue.GetDateRangeValue();
            var args = new RoutedPropertyChangedEventArgs<DateRangeValue>(oldRangeValue, newRangeValue)
            {
                RoutedEvent = DateRangeChangedEvent
            };
            RaiseEvent(args);
            RaiseCommand();
        }

        public static readonly RoutedEvent DateRangeChangedEvent = EventManager.RegisterRoutedEvent(nameof(DateRangeChanged), RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateRangeValue>), typeof(DateRangeValue));

        public event RoutedPropertyChangedEventHandler<DateRangeValue> DateRangeChanged {
            add { AddHandler(DateRangeChangedEvent, value); }
            remove { RemoveHandler(DateRangeChangedEvent, value); }
        }

        private void SetPreviousRange(object sender, RoutedEventArgs eventArgs) {
            _dateRanges.SetPreviousRange();
        }

        private Button _prevRangeButton;
        protected Button PrevRangeButton {
            get { return _prevRangeButton; }
            set {
                if (null != _prevRangeButton) {
                    _prevRangeButton.Click -= SetPreviousRange;
                }
                _prevRangeButton = value;
                if (null != _prevRangeButton) {
                    _prevRangeButton.Click += SetPreviousRange;
                }
            }
        }

        private void SetNextRange(object sender, RoutedEventArgs eventArgs) {
            _dateRanges.SetNextRange();
        }

        private Button _nextRangeButton;
        protected Button NextRangeButton {
            get { return _nextRangeButton; }
            set {
                if (null != _nextRangeButton) {
                    _nextRangeButton.Click -= SetNextRange;
                }
                _nextRangeButton = value;
                if (null != _nextRangeButton) {
                    _nextRangeButton.Click += SetNextRange;
                }
            }
        }

        private void SetTextBlockLocalizedText(string textBlockName, string resourceKey) {
            var textBlock = GetTemplateChild(textBlockName) as TextBlock;
            if (null != textBlock) {
                var s = _resourceManager.GetString(resourceKey);
                textBlock.Text = string.IsNullOrWhiteSpace(s) ? $"{resourceKey}" : s;
            }
        }


        public DateTime To {

            get { return (DateTime)GetValue(ToProperty); }
            set {
                if (To != value.Date) {
                    SetValue(ToProperty, value.Date);
                }
            }
        }

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register(nameof(To), typeof(DateTime), typeof(DateRangePicker)
                , new FrameworkPropertyMetadata(
                    new DateTime(0)
                    , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                    , OnToChangedCallBack)
            );

        private static void OnToChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            var picker = sender as DateRangePicker;
            picker?._dateRanges?.SetCustomWithNewToDate((DateTime)e.NewValue);
        }

        public DateTime From {

            get { return (DateTime)GetValue(FromProperty); }
            set {
                if (From != value.Date) {
                    SetValue(FromProperty, value.Date);
                }
            }
        }

        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register(nameof(From), typeof(DateTime), typeof(DateRangePicker)
                , new FrameworkPropertyMetadata(
                    new DateTime(0)
                    , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                    , OnFromChangedCallBack)
            );

        private static void OnFromChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            var picker = sender as DateRangePicker;
            picker?._dateRanges?.SetCustomWithNewFromDate((DateTime)e.NewValue);
        }
        
        public DateRangeKind? KindOfRange {
            get { return (DateRangeKind?)GetValue(KindOfRangeProperty); }
            set {
                //Users cannot set KindOfRange to null. It is reserved for the initial unassigned state. 
                if (null == value) {
                    value = DefaultKindOfRange;
                }
                if (KindOfRange != value) {
                    SetValue(KindOfRangeProperty, value);
                }
            }
        }

        public static readonly DependencyProperty KindOfRangeProperty =
            DependencyProperty.Register(nameof(KindOfRange), typeof(DateRangeKind?), typeof(DateRangePicker)
                , new FrameworkPropertyMetadata(
                    null
                    , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                    , OnKindOfRangeChangedCallBack)
            );

        private static void OnKindOfRangeChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            var picker = sender as DateRangePicker;

            //considering e.NewValue can be null after ClearValue(KindOfRangeProperty)
            var newValue = (DateRangeKind?)e.NewValue ?? DefaultKindOfRange;

            if (picker?._dateRanges != null) {
                picker._dateRanges.CurrentKindOfRange = newValue;
            }
        }


        public DayOfWeek FirstDayOfWeek {
            get { return (DayOfWeek)GetValue(FirstDayOfWeekProperty); }
            set {
                if (FirstDayOfWeek != value) {
                    SetValue(FirstDayOfWeekProperty, value);
                }
            }
        }

        public static readonly DependencyProperty FirstDayOfWeekProperty =
            DependencyProperty.Register(nameof(FirstDayOfWeek), typeof(DayOfWeek), typeof(DateRangePicker)
                , new PropertyMetadata(CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek, OnFirstDayOfWeekChangedCallBack));

        private static void OnFirstDayOfWeekChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            var picker = sender as DateRangePicker;
            if (picker?._firstDayOfWeekProvider != null) {
                picker._firstDayOfWeekProvider.Value = (DayOfWeek)e.NewValue;
            }
        }

        #region Trivial properties

        public Orientation Orientation {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(DateRangePicker)
                , new PropertyMetadata(default(Orientation)));


        public double SpacerWidth {
            get { return (double)GetValue(SpacerWidthProperty); }
            set { SetValue(SpacerWidthProperty, value); }
        }

        public static readonly DependencyProperty SpacerWidthProperty =
            DependencyProperty.Register(nameof(SpacerWidth), typeof(double), typeof(DateRangePicker)
                , new PropertyMetadata(DefaultSpacerValue));

        public double SpacerHeight {
            get { return (double)GetValue(SpacerHeightProperty); }
            set { SetValue(SpacerHeightProperty, value); }
        }

        public static readonly DependencyProperty SpacerHeightProperty =
            DependencyProperty.Register(nameof(SpacerHeight), typeof(double), typeof(DateRangePicker)
                , new PropertyMetadata(DefaultSpacerValue));


        public Style DatePickerCaptionTextBlockStyle {
            get { return (Style)GetValue(DatePickerCaptionTextBlockStyleProperty); }
            set { SetValue(DatePickerCaptionTextBlockStyleProperty, value); }
        }

        public static readonly DependencyProperty DatePickerCaptionTextBlockStyleProperty = DependencyProperty.Register(nameof(DatePickerCaptionTextBlockStyle), typeof(Style), typeof(DateRangePicker));


        public Style NavigationButtonTextBlockStyle {
            get { return (Style)GetValue(NavigationButtonTextBlockStyleProperty); }
            set { SetValue(NavigationButtonTextBlockStyleProperty, value); }
        }

        public static readonly DependencyProperty NavigationButtonTextBlockStyleProperty = DependencyProperty.Register(nameof(NavigationButtonTextBlockStyle), typeof(Style), typeof(DateRangePicker));


        public string PreviousRangeButtonText {
            get { return (string)GetValue(PreviousRangeButtonTextProperty); }
            set { SetValue(PreviousRangeButtonTextProperty, value); }
        }

        public static readonly DependencyProperty PreviousRangeButtonTextProperty =
            DependencyProperty.Register(nameof(PreviousRangeButtonText), typeof(string), typeof(DateRangePicker));


        public string NextRangeButtonText {
            get { return (string)GetValue(NextRangeButtonTextProperty); }
            set { SetValue(NextRangeButtonTextProperty, value); }
        }

        public static readonly DependencyProperty NextRangeButtonTextProperty =
            DependencyProperty.Register(nameof(NextRangeButtonText), typeof(string), typeof(DateRangePicker));



        public DatePickerFormat SelectedDateFormat {
            get { return (DatePickerFormat)GetValue(SelectedDateFormatProperty); }
            set { SetValue(SelectedDateFormatProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateFormatProperty =
            DependencyProperty.Register(nameof(SelectedDateFormat), typeof(DatePickerFormat), typeof(DateRangePicker));




        public double DatePickerMinWidth {
            get { return (double)GetValue(DatePickerMinWidthProperty); }
            set { SetValue(DatePickerMinWidthProperty, value); }
        }

        public static readonly DependencyProperty DatePickerMinWidthProperty =
            DependencyProperty.Register(nameof(DatePickerMinWidth), typeof(double), typeof(DateRangePicker));



        public double DatePickerToCaptionMinWidth {
            get { return (double)GetValue(DatePickerToCaptionMinWidthProperty); }
            set { SetValue(DatePickerToCaptionMinWidthProperty, value); }
        }

        public static readonly DependencyProperty DatePickerToCaptionMinWidthProperty =
            DependencyProperty.Register(nameof(DatePickerToCaptionMinWidth), typeof(double), typeof(DateRangePicker));



        public double NavigationButtonMinWidth {
            get { return (double)GetValue(NavigationButtonMinWidthProperty); }
            set { SetValue(NavigationButtonMinWidthProperty, value); }
        }

        public static readonly DependencyProperty NavigationButtonMinWidthProperty =
            DependencyProperty.Register(nameof(NavigationButtonMinWidth), typeof(double), typeof(DateRangePicker));



        public double RangeComboBoxMinWidth {
            get { return (double)GetValue(RangeComboBoxMinWidthProperty); }
            set { SetValue(RangeComboBoxMinWidthProperty, value); }
        }

        public static readonly DependencyProperty RangeComboBoxMinWidthProperty =
            DependencyProperty.Register(nameof(RangeComboBoxMinWidth), typeof(double), typeof(DateRangePicker));

        #endregion

        #region Command

        private void RaiseCommand() {
            if (Command != null) {
                RoutedCommand rc = Command as RoutedCommand;
                if (rc != null)
                    rc.Execute(CommandParameter, CommandTarget);
                else
                    Command.Execute(CommandParameter);
            }
        }


        // Keeps a copy of the CanExecuteChnaged handler so it doesn't get garbage collected.
        private EventHandler _canExecuteChangedHandler;

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(DateRangePicker),
            new PropertyMetadata(null, new PropertyChangedCallback(OnCommandChanged)));

        [TypeConverter(typeof(CommandConverter))]
        public ICommand Command {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        private static void OnCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
            var picker = sender as DateRangePicker;
            if (picker != null) {
                picker.OnCommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue);
            }
        }

        protected virtual void OnCommandChanged(ICommand oldValue, ICommand newValue) {
            if (oldValue != null) {
                UnhookCommand(oldValue, newValue);
            }

            HookupCommand(oldValue, newValue);

            CanExecuteChanged(null, null);
        }

        private void UnhookCommand(ICommand oldCommand, ICommand newCommand) {
            EventHandler handler = CanExecuteChanged;
            oldCommand.CanExecuteChanged -= CanExecuteChanged;
        }

        private void HookupCommand(ICommand oldCommand, ICommand newCommand) {
            EventHandler handler = new EventHandler(CanExecuteChanged);
            _canExecuteChangedHandler = handler;
            if (newCommand != null) {
                newCommand.CanExecuteChanged += _canExecuteChangedHandler;
            }
        }

        private void CanExecuteChanged(object sender, EventArgs e) {
            if (Command != null) {
                RoutedCommand rc = Command as RoutedCommand;

                IsEnabled = rc?.CanExecute(CommandParameter, CommandTarget) ?? Command.CanExecute(CommandParameter);
            }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(DateRangePicker),
            new PropertyMetadata(null));

        public object CommandParameter {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandTargetProperty =
            DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(DateRangePicker),
            new PropertyMetadata(null));

        public IInputElement CommandTarget {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }
        #endregion
    }
}

