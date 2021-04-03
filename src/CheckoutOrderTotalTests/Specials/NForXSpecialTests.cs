using CheckoutOrderTotalLib;
using NUnit.Framework;

namespace CheckoutOrderTotalTests {
    public class NForXSpecialTests : GroceryPOSSystemTestBase {
        [Test]
        [TestCase(3, 3, 5, 5)]
        [TestCase(5, 4, 7.5, 57.5)]
        public void SpecialAppliesToTotalWhenMet(double orderQty, double qualifiedQty, double discountPrice, double expectedPrice) {
            var checkoutManager = SetupAndScan(C_DefaultItem, C_DefaultUnitPrice, orderQty);
            checkoutManager.SetSpecial(C_DefaultItem, new NForXSpecial(qualifiedQty, discountPrice));

            Assert.AreEqual(expectedPrice, checkoutManager.GetTotalPrice());
        }
    }
}
