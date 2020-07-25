// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using NUnit.Framework;
using Rh.DateRange.Picker.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace Rh.DateRange.Picker.Tests {
    /// <summary>
    /// This tests the proper DateRangePicker initialization (OnApplyTemplate) against different 
    /// To vs. From vs. KindOfDateRange property value combinations provided 
    /// in XAML by the user.
    /// </summary>
    [TestFixture, RequiresThread(ApartmentState.STA)]
    public class DateRangePickerTests {

        //nullables are for the case user has not set the paramater
        public struct PickerDataProps {
            public DateTime? From { get; }
            public DateTime? To { get; }
            public DateRangeKind? Kind { get; }

            public PickerDataProps(DateTime? from = null, DateTime? to = null, DateRangeKind? kind = null) {
                From = from;
                To = to;
                Kind = kind;
            }
        }

        private DateRangePicker _picker;
        private static readonly DateTime PastDate1 = new DateTime(2001, 01, 01);
        private static readonly DateTime PastDate2 = new DateTime(2002, 02, 02);
        private static readonly DateTime FutureDate = new DateTime(2100, 01, 01);
        private static readonly DateTime Today = DateTime.Now.Date;
        private static readonly DateTime Yesterday = DateTime.Now.AddDays(-1).Date;


        private static IEnumerable<TestCaseData> OnApplyTemplateCases() {

            var testName = nameof(OnApplyTemplateCases);

            yield return new TestCaseData(
                new PickerDataProps(), new PickerDataProps(Today, Today, DateRangeKind.Today))
                .SetName($"{testName}: 010");

            yield return new TestCaseData(
                new PickerDataProps(null, null, DateRangeKind.Custom), new PickerDataProps(Yesterday, Today, DateRangeKind.Custom))
                .SetName($"{testName}: 020");


            foreach (var kind in new DateRangeKind?[] { null, DateRangeKind.Custom }) {

                yield return new TestCaseData(
                    new PickerDataProps(null, PastDate1, kind), new PickerDataProps(PastDate1, PastDate1, DateRangeKind.Custom))
                    .SetName($"{testName}: 030 {kind}");

                yield return new TestCaseData(
                    new PickerDataProps(null, FutureDate, kind), new PickerDataProps(Yesterday, FutureDate, DateRangeKind.Custom))
                    .SetName($"{testName}: 040 {kind}");

                yield return new TestCaseData(
                    new PickerDataProps(PastDate1, null, kind), new PickerDataProps(PastDate1, Today, DateRangeKind.Custom))
                    .SetName($"{testName}: 050 {kind}");

                yield return new TestCaseData(
                    new PickerDataProps(FutureDate, null, kind), new PickerDataProps(FutureDate, FutureDate, DateRangeKind.Custom))
                    .SetName($"{testName}: 060 {kind}");

                yield return new TestCaseData(
                    new PickerDataProps(PastDate1, PastDate2, kind), new PickerDataProps(PastDate1, PastDate2, DateRangeKind.Custom))
                    .SetName($"{testName}: 070 {kind}");

                yield return new TestCaseData(
                    new PickerDataProps(PastDate2, PastDate1, kind), new PickerDataProps(PastDate2, PastDate2, DateRangeKind.Custom))
                    .SetName($"{testName}: 080 {kind}");

            }


            foreach (DateRangeKind kind in Enum.GetValues(typeof(DateRangeKind))) {

                var kindsToPassBy = new DateRangeKind[] { DateRangeKind.Custom, DateRangeKind.Week, DateRangeKind.Month, DateRangeKind.Year };

                if (kindsToPassBy.Contains(kind)) {
                    continue;
                }

                //for the case when DateRangeKind enum has more kinds than are actually contained in the Picker's DateRangeList.
                var dateRange = DateRangeHelper.GetDateRangeOfKind(kind);
                if (null == dateRange) {
                    continue;
                }

                yield return new TestCaseData(
                    new PickerDataProps(null, null, kind),
                    new PickerDataProps(dateRange.From, dateRange.To, kind))
                    .SetName($"{testName}: 110 {kind}");

                yield return new TestCaseData(
                    new PickerDataProps(PastDate1, null, kind),
                    new PickerDataProps(dateRange.From, dateRange.To, kind))
                    .SetName($"{testName}: 120 {kind}");

                yield return new TestCaseData(
                    new PickerDataProps(null, PastDate1, kind),
                    new PickerDataProps(dateRange.From, dateRange.To, kind))
                    .SetName($"{testName}: 130 {kind}");

                yield return new TestCaseData(
                    new PickerDataProps(PastDate1, PastDate2, kind),
                    new PickerDataProps(dateRange.From, dateRange.To, kind))
                    .SetName($"{testName}: 140 {kind}");

                yield return new TestCaseData(
                    new PickerDataProps(PastDate2, PastDate1, kind),
                    new PickerDataProps(dateRange.From, dateRange.To, kind))
                    .SetName($"{testName}: 150 {kind}");

            }

        }


        [SetUp]
        public void SetUp() {
            _picker = new DateRangePicker();
        }

        [Test()]
        public void DateRangePickerTest() {
            Assert.That(_picker.KindOfRange, Is.Null);
            Assert.That(_picker.From, Is.EqualTo(new DateTime(0)));
            Assert.That(_picker.To, Is.EqualTo(new DateTime(0)));
        }

        [Test(), TestCaseSource(nameof(OnApplyTemplateCases))]
        public void OnApplyTemplateTest(PickerDataProps userSets, PickerDataProps result) {
            if (userSets.From != null) {
                _picker.From = (DateTime)userSets.From;
            }
            if (userSets.To != null) {
                _picker.To = (DateTime)userSets.To;
            }
            if (userSets.Kind != null) {
                _picker.KindOfRange = (DateRangeKind)userSets.Kind;
            }

            _picker.OnApplyTemplate();

            Assert.That(_picker.KindOfRange, Is.EqualTo(result.Kind));
            Assert.That(_picker.From, Is.EqualTo(result.From));
            Assert.That(_picker.To, Is.EqualTo(result.To));
        }

    }
}