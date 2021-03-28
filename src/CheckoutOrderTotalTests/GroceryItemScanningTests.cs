using CheckoutOrderTotalLib;
using NUnit.Framework;

namespace CheckoutOrderTotalTests {
    public class GroceryItemScanningTests : GroceryPOSSystemTestBase {
        [TestFixture]
        public class ScanItemTests : GroceryPOSSystemTestBase {
            [Test]
            public void AddingItemNotConfiguredDoesNotScanItem() {
                var checkoutManager = SetupCheckoutManager("milk", 5.27);

                Assert.False(checkoutManager.ScanItem("yogurt"));
                Assert.AreEqual(0, checkoutManager.GetTotalPrice());
            }
        }

        [TestFixture]
        public class RemoveScannedItemTests : GroceryPOSSystemTestBase {
            [Test]
            [TestCase("Yummy Pretzels", 200)]
            [TestCase("helloworld beans", 20)]
            [TestCase("groceries", 72.32)]
            public void RemovingItemUpdatesCorrectTotal(string groceryItem, double unitPrice) {
                var checkoutManager = SetupAndScan(groceryItem, unitPrice, 2);
                checkoutManager.AddScannableItem(C_DefaultItem, 50);
                checkoutManager.ScanItem(C_DefaultItem);
                checkoutManager.RemoveScannedItem(groceryItem);

                Assert.AreEqual(50, checkoutManager.GetTotalPrice());
            }

            [Test]
            public void RemovingNonExistentItemDoesNotInvalidateTotal() {
                var checkoutManager = new GroceryPOSSystem();

                checkoutManager.RemoveScannedItem(C_DefaultItem);

                Assert.AreEqual(0, checkoutManager.GetTotalPrice());
            }
        }
    }
}
