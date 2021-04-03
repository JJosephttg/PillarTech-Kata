using NUnit.Framework;
using CheckoutOrderTotalLib;
using System;

namespace CheckoutOrderTotalTests {
    public class BuyXGetYAtZPercentOffTests : GroceryPOSSystemTestBase {

        [Test]
        [TestCase(5, 5, 1, 1, 250)]
        [TestCase(4, 2, 1, 1, 175)]
        [TestCase(9, 2, 1, 2, 400)]
        public void SpecialDoesNotApplyAfterLimitIsHitOrBeforeLimitCanBeApplied(double orderQty, double qualifiedQty, double discountQty, int limit, double expectedPrice) {
            var checkoutManager = SetupAndScan(C_DefaultItem, C_DefaultUnitPrice, orderQty);
            checkoutManager.SetSpecial(C_DefaultItem, new BuyXGetYAtZPercentOffSpecial(qualifiedQty, discountQty, 50, limit));
            Assert.AreEqual(expectedPrice, checkoutManager.GetTotalPrice());
        }

        [Test]
        [TestCaseSource(nameof(InvalidNumbers))]
        public void SpecialWithInvalidQualifiedQtyThrowsException(double invalidQty) => SetupAndValidateSpecial(invalidQty, 1, 50, 1, "qualifiedQty");

        [Test]
        [TestCaseSource(nameof(InvalidNumbers))]
        public void SpecialWithInvalidDiscountQtyThrowsException(double invalidQty) => SetupAndValidateSpecial(1, invalidQty, 50, 1, "discountedQty");

        [Test]
        [TestCaseSource(nameof(InvalidNumbers))]
        [TestCase(100.001)]
        [TestCase(150)]
        public void SpecialWithInvalidPercentageThrowsException(double invalidPct) => SetupAndValidateSpecial(1, 1, invalidPct, 1, "percentOff");

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-83)]
        [TestCase(int.MinValue)]
        public void SpecialWithInvalidLimitThrowsException(int limit) => SetupAndValidateSpecial(1, 1, 50, limit, "limit");

        private void SetupAndValidateSpecial(double qualifiedQty, double discountQty, double percentOff, int limit, string paramName) {
            var checkoutManager = SetupAndScan(C_DefaultItem, C_DefaultUnitPrice);
            AssertExceptionParam<ArgumentOutOfRangeException>(() => checkoutManager.SetSpecial(C_DefaultItem, new BuyXGetYAtZPercentOffSpecial(qualifiedQty, discountQty, percentOff, limit)), paramName);
        }            
    }
}
