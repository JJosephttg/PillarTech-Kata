using CheckoutOrderTotalLib;

namespace CheckoutOrderTotalTests {
    public abstract class GroceryPOSSystemTestBase {

        protected const string C_DefaultItem = "Can of Hello World Beans";
        protected const double C_DefaultUnitPrice = 50;

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