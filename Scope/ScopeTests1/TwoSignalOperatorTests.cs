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
    public class SignalOperatorTests
    {
        [TestMethod()]
        public void TwoOperandsResultTest()
        {
            TwoSignalOperator oper = new SignalSummator();

            SineWaveGenerator generator = new SineWaveGenerator(10, 1, 0);
            TimeDomainSignal s1 = generator.GenerateSignal(0, 1000);
            TimeDomainSignal s2 = generator.GenerateSignal(500, 1000);

            Signal[] operands = { s1, s2 };
            Signal res = oper.Result(operands);

            Assert.IsTrue(Math.Abs(res.Duration - 1.5) < 0.001);
        }

        public void OneOperandResultTest()
        {
            SignalOperator oper = new SignalDifferentiator();

            SineWaveGenerator generator = new SineWaveGenerator(10, 1, 0);
            TimeDomainSignal s1 = generator.GenerateSignal(0, 1000);

            Signal[] operands = { s1 };
            Signal res = oper.Result(operands);

            Assert.IsTrue(Math.Abs(res.Duration - s1.Duration) < 0.001);

            oper = new SignalIntegrator();
            res = oper.Result(operands);

            Assert.IsTrue(Math.Abs(res.Duration - s1.Duration) < 0.001);
        }
    }
}