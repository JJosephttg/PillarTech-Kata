using CheckoutOrderTotalLib;
using NUnit.Framework;

namespace CheckoutOrderTotalTests {
    public abstract class GroceryPOSSystemTestBase {

        protected const string C_DefaultItem = "Can of Hello World Beans";

        #region Misc Methods
        protected GroceryPOSSystem SetupCheckoutManager(string groceryItem, double unitPrice) {
            var checkoutManager = new GroceryPOSSystem();
            checkoutManager.AddScannableItem(groceryItem, unitPrice);
            return checkoutManager;
        }

        protected GroceryPOSSystem SetupAndScan(string groceryItem, double unitPrice, double quantity = 1) {
            var checkoutManager = SetupCheckoutManager(groceryItem, unitPrice);
            checkoutManager.ScanItem(groceryItem, quantity);
            return checkoutManager;
        }
        #endregion
    }
}