using CheckoutOrderTotalLib;
using NUnit.Framework;

namespace CheckoutOrderTotalTests {
    public class CheckoutManagerTests {

        #region ConfigureItemPrice Tests
        [Test]
        public void AddingConfiguredItemProvidesCorrectTotal() {
            CheckoutOrderManager checkoutManager = new CheckoutOrderManager();

            // Configure item
            string groceryItem = "beef";
            checkoutManager.ConfigureItemPrice(groceryItem, 5.87);
            
            //Test that scanning an item goes through and reads in our price we set
            Assert.True(checkoutManager.ScanItem(groceryItem));
            Assert.AreEqual(5.87, checkoutManager.GetCurrentTotal());
        }
        #endregion

        #region ScanItem Tests
        [Test]
        public void AddingItemNotConfiguredDoesNotScanItem() {
            CheckoutOrderManager checkoutManager = new CheckoutOrderManager();

            checkoutManager.ConfigureItemPrice("milk", 5.27);

            Assert.False(checkoutManager.ScanItem("yogurt"));
            Assert.AreEqual(0, checkoutManager.GetCurrentTotal());
        }
        #endregion
    }
}