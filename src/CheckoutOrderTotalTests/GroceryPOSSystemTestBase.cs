using CheckoutOrderTotalLib;
using NUnit.Framework;
using System;

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

        protected void SetupAndValidateSpecial(Func<SpecialBase> specGen, string paramName) {
            var checkoutManager = SetupAndScan(C_DefaultItem, C_DefaultUnitPrice);
            AssertExceptionParam<ArgumentOutOfRangeException>(() => checkoutManager.SetSpecial(C_DefaultItem, specGen()), paramName);
        }

        protected void AssertExceptionParam<T>(TestDelegate method, string paramName) where T : ArgumentException {
            var exception = Assert.Throws<T>(method);
            Assert.AreEqual(exception.ParamName, paramName);
        }
        #endregion

        #region TestCaseSources
        protected static double[] InvalidNumbers = new double[] { 0, -1, -3.23, -.0001, double.NegativeInfinity, double.PositiveInfinity };
        protected static int[] InvalidLimits = new int[] { 0, -1, -25, -150, int.MinValue };
        #endregion
    }
}