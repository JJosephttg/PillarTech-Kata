using NUnit.Framework;
using CheckoutOrderTotalLib;

namespace CheckoutOrderTotalTests {
    public class BuyXGetYAtZPercentOffTests : GroceryPOSSystemTestBase {

        [Test]
        [TestCase(5, 2, 1, 0, 250)]
        [TestCase(4, 2, 1, 1, 175)]
        [TestCase(9, 2, 1, 2, 400)]
        public void SpecialDoesNotApplyAfterLimitIsHit(double orderQty, double qualifiedQty, double discountQty, int limit, double expectedPrice) {
            var checkoutManager = SetupAndScan(C_DefaultItem, C_DefaultUnitPrice, orderQty);
            checkoutManager.SetSpecial(C_DefaultItem, new BuyXGetYAtZPercentOffSpecial(qualifiedQty, discountQty, 50, limit));
            Assert.AreEqual(expectedPrice, checkoutManager.GetTotalPrice());
        }
    }
}
