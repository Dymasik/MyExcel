using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LABA.Tests
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

        [TestMethod]
        public void Analyze_1ab_exeption_return()
        {
            Calculator calculator = new Calculator();
            string exp = "1ab";

            Exception expectedExcetpion = null;

            // Act
            try
            {
                double result = calculator.SetExp(exp);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.IsNotNull(expectedExcetpion);

        }

        [TestMethod]
        public void Analyze_5div0_exep_return()
        {
            Calculator calculator = new Calculator();
            string exp = "5/0";

            Exception expectedExcetpion = null;

            // Act
            try
            {
                double result = calculator.SetExp(exp);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.IsNotNull(expectedExcetpion);
        }

        [TestMethod]
        public void Analyze_lot_of_brackets_exep_return()
        {
            Calculator calculator = new Calculator();
            string exp = "(((((((((55/88)))))";

            Exception expectedExcetpion = null;

            // Act
            try
            {
                double result = calculator.SetExp(exp);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.IsNotNull(expectedExcetpion);
        }

        [TestMethod]
        public void Analyze_hard_expr_double_return()
        {
            Calculator calculator = new Calculator();
            string exp = "((11+8)/9-(7-9/2)/2)+3^2^0";
            double expected = (31 / 36.0) + 3.0;

            double actual = calculator.SetExp(exp);
            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
