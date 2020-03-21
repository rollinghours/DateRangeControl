// Copyright (c) Alexander Zhmerik. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
using NUnit.Framework;
using Rh.DateRange.Picker.Common;
using System;

namespace Rh.DateRange.Picker.Tests {
    [TestFixture()]
    public class RangeCustomTests {

        private DateTime _start;
        private DateTime _end;

        [SetUp()]
        public void SetUp() {
            _start = new DateTime(2015, 2, 15);
            _end = new DateTime(2015, 2, 20);
        }

        [Test()]
        public void RangeCustomTest() {
            var result = new RangeCustom();
            Assert.AreEqual(result.To, DateTime.Now.Date);
            Assert.AreEqual(result.From, DateTime.Now.AddDays(-1).Date);
        }

        [Test(), Sequential]
        public void RangeCustomTestWithInput([Values(42050/*20150215*/, 42055, 42055.5)] double end) {
            var dateEnd = DateTime.FromOADate(end);
            var result = new RangeCustom(_start, dateEnd);
            Assert.AreEqual(result.From, _start.Date);
            Assert.AreEqual(result.To, dateEnd.Date);
        }

        [Test()]
        public void RangeCustomTestWithInputWrong() {
            TestDelegate d = () => new RangeCustom(_end, _start);
            Assert.That(d, Throws.ArgumentException);
        }


        [Test()]
        public void RefreshTest() {
            var range = new RangeCustom(_start, _end);
            var result = range.Refresh();
            Assert.AreNotSame(range, result);
            Assert.AreEqual(result.From, _start);
            Assert.AreEqual(result.To, _end);
        }

        [Test(), Sequential]
        public void GetNextRangeTest(
            [Values(15, 15, 15.9)] double start, 
            [Values(20, 15, 16.1)] double end ) {

            var from = DateTime.FromOADate(start);
            var to = DateTime.FromOADate(end);
            int span = (int)(to.Date - from.Date).TotalDays;

            IDateRange range = new RangeCustom(from, to);

            for (int i = 1; i < 10; i++) {
                range = range.GetNextRange();
                Assert.AreEqual(range.From, from.AddDays(span * i + i).Date, $"From, i={i}");
                Assert.AreEqual(range.To, to.AddDays(span * i + i).Date, $"To, i={i}");
            }
        }

        [Test(), Sequential]
        public void GetPreviousRangeTest(
            [Values(15, 15, 15.9)] double start,
            [Values(20, 15, 16.1)] double end) {

            var from = DateTime.FromOADate(start);
            var to = DateTime.FromOADate(end);
            int span = (int)(to.Date - from.Date).TotalDays;

            IDateRange range = new RangeCustom(from, to);

            for (int i = 1; i < 10; i++) {
                range = range.GetPreviousRange();
                Assert.AreEqual(range.From, from.AddDays(-span * i - i).Date, $"From, i={i}");
                Assert.AreEqual(range.To, to.AddDays(-span * i - i).Date, $"To, i={i}");
            }
        }
    }
}