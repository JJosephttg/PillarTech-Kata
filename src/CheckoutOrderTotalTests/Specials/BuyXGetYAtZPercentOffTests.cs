using NUnit.Framework;
using CheckoutOrderTotalLib;

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
        public void SpecialWithInvalidQualifiedQtyThrowsException(double invalidQty) => SetupAndValidateSpecial(() => new BuyXGetYAtZPercentOffSpecial(invalidQty, 1, 50), "qualifiedQty");

        [Test]
        [TestCaseSource(nameof(InvalidNumbers))]
        public void SpecialWithInvalidDiscountQtyThrowsException(double invalidQty) => SetupAndValidateSpecial(() => new BuyXGetYAtZPercentOffSpecial(1, invalidQty, 50), "discountedQty");

        [Test]
        [TestCaseSource(nameof(InvalidNumbers))]
        [TestCase(100.001)]
        [TestCase(150)]
        public void SpecialWithInvalidPercentageThrowsException(double invalidPct) => SetupAndValidateSpecial(() => new BuyXGetYAtZPercentOffSpecial(1, 1, invalidPct), "percentOff");

        [Test]
        [TestCaseSource(nameof(InvalidLimits))]
        public void SpecialWithInvalidLimitThrowsException(int limit) => SetupAndValidateSpecial(() => new BuyXGetYAtZPercentOffSpecial(1, 1, 50, limit), "limit");         
    }
}
