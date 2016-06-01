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
    public class SignalGeneratorTests
    {

        [TestMethod()]
        public void GenerateSineSignalTest()
        {
            SignalGenerator generator = new SineWaveGenerator(10, 1, 0);

            TimeDomainSignal tested = generator.GenerateSignal(0, 1000);

            Assert.IsTrue(Math.Abs(tested.Duration - 1.0) < 0.001);
            Assert.IsTrue(Math.Abs(tested.PeakToPeak - 1.0) < 0.001);
        }

        [TestMethod()]
        public void GenerateFlatTest()
        {
            SignalGenerator generator = new FlatGenerator(10, 1, 0, 0);

            TimeDomainSignal tested = generator.GenerateSignal(0, 1000);

            Assert.IsTrue(Math.Abs(tested.Duration - 1.0) < 0.001);
            Assert.IsTrue(Math.Abs(tested.PeakToPeak - 0.0) < 0.001);
        }
    }
}