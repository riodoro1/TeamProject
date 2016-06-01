using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scope;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scope.Tests
{
    [TestClass()]
    public class SignalTests
    {
        SineWaveGenerator generator = new SineWaveGenerator(50, 2, 0);
        

        [TestMethod()]
        public void SignalTest()
        {
            TimeDomainSignal testSignal = generator.GenerateSignal(0, 1000);
            Assert.IsTrue(Math.Abs(testSignal.FirstXValue - 0.0) < 0.001);
            Assert.IsTrue(Math.Abs(testSignal.Duration - 1.0) < 0.001);
            Assert.IsTrue(Math.Abs(testSignal.PeakToPeak - 2.0) < 0.001);
        }

        [TestMethod()]
        public void StartIndexInsideIntervalTest()
        {
            TimeDomainSignal testSignal = generator.GenerateSignal(0, 1000);
            int start = testSignal.StartIndexInsideInterval(0, 1.0);

            Assert.AreEqual(start, 0);

            start = testSignal.StartIndexInsideInterval(0.1, 1.0);

            Assert.AreNotEqual(start, -1);
            Assert.IsTrue(start > 0);

            start = testSignal.StartIndexInsideInterval(1.01, 2.0);

            Assert.AreEqual(start, -1);
        }

        [TestMethod()]
        public void FirstIndexBeforeTest()
        {
            TimeDomainSignal testSignal = generator.GenerateSignal(0, 1000);
            int start = testSignal.FirstIndexBefore(0.1);

            Assert.AreNotEqual(start, -1);
        }

        [TestMethod()]
        public void InterpolatedValueForXTest()
        {
            TimeDomainSignal testSignal = generator.GenerateSignal(0, 1000);

            double val = testSignal.InterpolatedValueForX(testSignal.Points[0].Y).Value;
            Assert.AreEqual(val, testSignal.Points[0].Y);

            val = testSignal.InterpolatedValueForX(0.00005).Value;
            Assert.IsTrue(val > testSignal.Points[0].X);
        }
    }
}