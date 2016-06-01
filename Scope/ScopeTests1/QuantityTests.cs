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
    public class QuantityTests
    {
        [TestMethod()]
        public void QuantityTest()
        {
            Assert.IsTrue(true); //not much to test here
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Quantity testedQuantity = new Quantity(0.001, "u");

            Assert.AreEqual(testedQuantity.ToString(), "1.0mu");

            testedQuantity = new Quantity(0.000001, "u");

            Assert.AreEqual(testedQuantity.ToString(), "1.0µu");

            testedQuantity = new Quantity(100000, "u");

            Assert.AreEqual(testedQuantity.ToString(), "100.0ku");
        }
    }
}