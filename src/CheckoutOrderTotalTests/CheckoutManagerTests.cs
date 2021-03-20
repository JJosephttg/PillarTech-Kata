using CheckoutOrderTotalLib;
using NUnit.Framework;

namespace CheckoutOrderTotalTests {
    public class CheckoutManagerTests {

        #region AddItem Tests
        [Test]
        [TestCase("beef", 5.87)]
        public void ConfiguringItemProvidesCorrectTotalWhenAdded(string groceryItem, double unitPrice) {
            var checkoutManager = SetupAndScan(groceryItem, unitPrice);

            Assert.AreEqual(unitPrice, checkoutManager.GetTotalPrice());
        }

        [Test]
        [TestCase("rice", 8.3, 2.5)]
        //For weighted items, seems like it is similar to per-unit because it is per-unit of the item, but represented as a different standard like pounds or ounces
        public void ConfiguringWeightedItemProvidesCorrectTotalWhenAdded(string groceryItem, double unitPrice, double weight) {
            var checkoutManager = SetupAndScan(groceryItem, unitPrice, weight);

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

        #region RemoveScannedItem Tests
        [Test]
        [TestCase("Yummy Pretzels", 200)]
        public void RemovingItemUpdatesCorrectTotal(string groceryItem, double unitPrice) {
            var checkoutManager = SetupAndScan(groceryItem, unitPrice, 2);
            checkoutManager.RemoveScannedItem(groceryItem);

            Assert.AreEqual(unitPrice, checkoutManager.GetTotalPrice());
        }

        [Test]
        [TestCase("Yum yum yuummoooo")]
        public void RemovingNonExistentItemDoesNotInvalidateTotal(string groceryItem) {
            var checkoutManager = new CheckoutOrderManager();

            checkoutManager.RemoveScannedItem(groceryItem);

            Assert.AreEqual(0, checkoutManager.GetTotalPrice());
        }
        #endregion

        #region SetMarkdown Tests
        [Test]
        [TestCase("Candy", 7.49, .15)]
        public void SettingMarkdownReflectsCorrectTotalWhenAdded(string groceryItem, double unitPrice, double markdown) {
            var checkoutManager = SetupAndScan(groceryItem, unitPrice);
            checkoutManager.SetMarkdown(groceryItem, markdown);

            Assert.AreEqual(unitPrice - markdown, checkoutManager.GetTotalPrice());
        }
        #endregion

        private CheckoutOrderManager SetupCheckoutManager(string groceryItem, double unitPrice) {
            var checkoutManager = new CheckoutOrderManager();
            checkoutManager.AddItem(groceryItem, unitPrice);
            return checkoutManager;
        }

        private CheckoutOrderManager SetupAndScan(string groceryItem, double unitPrice, double quantity = 1) {
            var checkoutManager = SetupCheckoutManager(groceryItem, unitPrice);
            checkoutManager.ScanItem(groceryItem, quantity);
            return checkoutManager;
        }
    }
}