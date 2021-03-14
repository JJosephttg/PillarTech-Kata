using CheckoutOrderTotalLib;
using NUnit.Framework;

namespace CheckoutOrderTotalTests {
    public class CheckoutManagerTests {

        #region ConfigureItemPrice Tests
        [Test]
        [TestCase("beef", 5.87)]
        public void ConfiguringItemProvidesCorrectTotalWhenAdded(string groceryItem, double unitPrice) {
            var checkoutManager = SetupCheckoutManager(groceryItem, unitPrice);
            
            Assert.True(checkoutManager.ScanItem(groceryItem));
            Assert.AreEqual(unitPrice, checkoutManager.GetTotalPrice());
        }

        [Test]
        [TestCase("rice", 8.3, 2.5)]
        //For weighted items, seems like it is similar to per-unit because it is per-unit of the item, but represented as a different standard like pounds or ounces
        public void ConfiguringWeightedItemProvidesCorrectTotalWhenAdded(string groceryItem, double unitPrice, double weight) {
            var checkoutManager = SetupCheckoutManager(groceryItem, unitPrice);
            
            Assert.True(checkoutManager.ScanItem(groceryItem, weight));
            Assert.AreEqual(unitPrice * weight, checkoutManager.GetTotalPrice());
        }
        #endregion

        #region ScanItem Tests
        [Test]
        public void AddingItemNotConfiguredDoesNotScanItem() {
            var checkoutManager = SetupCheckoutManager("milk", 5.27);

            Assert.False(checkoutManager.ScanItem("yogurt"));
            Assert.AreEqual(0, checkoutManager.GetTotalPrice());
        }
        #endregion

        private CheckoutOrderManager SetupCheckoutManager(string groceryItem, double unitPrice) {
            var checkoutManager = new CheckoutOrderManager();
            checkoutManager.SetProductUnitPrice(groceryItem, unitPrice);
            return checkoutManager;
        }
    }
}