using System;
//using LABA;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LABA_Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Analyze_1sum2_3_return()
        {
            Calculator calculator = new Calculator();
            string exp = "1+2";
            double expected = 3;
            double result = calculator.SetExp(exp);

            Assert.AreEqual(expected, result);
        }
    }
}
