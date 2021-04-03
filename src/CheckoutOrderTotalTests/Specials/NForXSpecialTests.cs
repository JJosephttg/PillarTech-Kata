using CheckoutOrderTotalLib;
using NUnit.Framework;

namespace CheckoutOrderTotalTests {
    public class NForXSpecialTests : GroceryPOSSystemTestBase {
        [Test]
        [TestCase(3, 3, 5, 5)]
        [TestCase(5, 4, 7.5, 57.5)]
        public void SpecialAppliesToTotalWhenMet(double orderQty, double qualifiedQty, double discountPrice, double expectedPrice) => SetupSpecialAndExpectPrice(orderQty, qualifiedQty, discountPrice, 1, expectedPrice);

        [Test]
        [TestCase(6, 3, 5, 1, 155)]
        [TestCase(10, 2.5, 5, 3, 140)]
        [TestCase(5.5, 2, 10, 10, 95)]
        public void SpecialAppliesUpToLimit(double orderQty, double qualifiedQty, double discountPrice, int limit, double expectedPrice) => SetupSpecialAndExpectPrice(orderQty, qualifiedQty, discountPrice, limit, expectedPrice);

        [Test]
        [TestCaseSource(nameof(InvalidNumbers))]
        public void SpecialWithInvalidQualifiedQtyThrowsException(double invalidQty) => SetupAndValidateSpecial(() => new NForXSpecial(invalidQty, 1), "qualifiedQty");

        [Test]
        [TestCaseSource(nameof(InvalidNumbers))]
        public void SpecialWithInvalidDiscountPriceThrowsException(double invalidPrice) => SetupAndValidateSpecial(() => new NForXSpecial(1, invalidPrice), "discountPrice");

        [Test]
        [TestCaseSource(nameof(InvalidLimits))]
        public void SpecialWithInvalidLimitThrowsException(int invalidLimit) => SetupAndValidateSpecial(() => new NForXSpecial(1, 1, invalidLimit), "limit");

        private void SetupSpecialAndExpectPrice(double orderQty, double qualifiedQty, double discountPrice, int limit, double expectedPrice) {
            var checkoutManager = SetupAndScan(C_DefaultItem, C_DefaultUnitPrice, orderQty);
            checkoutManager.SetSpecial(C_DefaultItem, new NForXSpecial(qualifiedQty, discountPrice, limit));

            Assert.AreEqual(expectedPrice, checkoutManager.GetTotalPrice());
        } 
    }
}
