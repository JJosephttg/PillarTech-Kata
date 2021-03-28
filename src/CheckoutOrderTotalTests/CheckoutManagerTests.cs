using CheckoutOrderTotalLib;
using NUnit.Framework;
using System;

namespace CheckoutOrderTotalTests {
    public class CheckoutManagerTests {

        private const string C_DefaultItem = "Can of Hello World Beans";

        [TestFixture]
        public class AddItemTests : CheckoutManagerTests {            
            [Test]
            [TestCase("beef", 5.87)]
            public void ConfiguringItemProvidesCorrectTotalWhenAdded(string groceryItem, double unitPrice) {
                var checkoutManager = SetupAndScan(groceryItem, unitPrice);

                Assert.AreEqual(unitPrice, checkoutManager.GetTotalPrice());
            }

            [Test]
            [TestCase("rice", 8.3, 2.5)]
            public void ConfiguringWeightedItemProvidesCorrectTotalWhenAdded(string groceryItem, double unitPrice, double weight) {
                var checkoutManager = SetupAndScan(groceryItem, unitPrice, weight);

                Assert.AreEqual(unitPrice * weight, checkoutManager.GetTotalPrice());
            }
        }
        

        [TestFixture]
        public class ScanItemTests : CheckoutManagerTests {
            [Test]
            public void AddingItemNotConfiguredDoesNotScanItem() {
                var checkoutManager = SetupCheckoutManager("milk", 5.27);

                Assert.False(checkoutManager.ScanItem("yogurt"));
                Assert.AreEqual(0, checkoutManager.GetTotalPrice());
            }
        }
        
        [TestFixture]
        public class RemoveScannedItemTests : CheckoutManagerTests {
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

        [TestFixture]
        public class SetMarkdownTests : CheckoutManagerTests {
            [Test]
            [TestCase("Candy", 7.49, .15)]
            public void SettingMarkdownReflectsCorrectTotalWhenAdded(string groceryItem, double unitPrice, double markdown) {
                var checkoutManager = SetupAndScan(groceryItem, unitPrice);
                checkoutManager.SetMarkdown(groceryItem, markdown);

                Assert.AreEqual(unitPrice - markdown, checkoutManager.GetTotalPrice());
            }

            [Test]
            [TestCase(5, -1)]
            [TestCase(6, -5.23)]
            [TestCase(2.8, -.01)]
            [TestCase(1, -1928.84)]
            [TestCase(9.43, double.NegativeInfinity)]
            [TestCase(1, 20)]
            [TestCase(7.34, 8.12)]
            [TestCase(0.01, 0.02)]
            [TestCase(19283.23, 99999.43)]
            public void SettingInvalidMarkdownThrowsException(double originPrice, double markdownPrice) {
                var checkoutManager = SetupAndScan(C_DefaultItem, originPrice);
                AssertExceptionParam<ArgumentOutOfRangeException>(() => checkoutManager.SetMarkdown(C_DefaultItem, markdownPrice), "markdown");
            }

            [Test]
            [TestCase("Hello")]
            [TestCase("Green Beans")]
            [TestCase("")]
            public void SettingMarkdownOnNonConfiguredItemThrowsException(string itemId) {
                AssertExceptionParam<ArgumentException>(() => new GroceryPOSSystem().SetMarkdown(itemId, 5.23), "itemId");
            }

            private void AssertExceptionParam<T>(TestDelegate method, string paramName) where T : ArgumentException {
                var exception = Assert.Throws<T>(method);
                Assert.AreEqual(exception.ParamName, paramName);
            }
        }

        [TestFixture]
        public class SetSpecialTests : CheckoutManagerTests {
            private const double C_DefaultUnitPrice = 50;
            [Test]
            [TestCase(1, 1, 100, 50)]
            [TestCase(2, 1, 50, 125)]
            public void BuyXGetYAtZPercentOffSpecialReflectsCorrectTotal(double qualifiedQty, double discountedQty, double percentOff, double expectedTotal) {
                var checkoutManager = SetupAndScan(C_DefaultItem, C_DefaultUnitPrice, qualifiedQty + discountedQty);
                checkoutManager.SetSpecial(C_DefaultItem, new BuyXGetYAtZPercentOffSpecial(qualifiedQty, discountedQty, percentOff));
                Assert.AreEqual(expectedTotal, checkoutManager.GetTotalPrice());
            }
        }

        #region Misc Methods
        private GroceryPOSSystem SetupCheckoutManager(string groceryItem, double unitPrice) {
            var checkoutManager = new GroceryPOSSystem();
            checkoutManager.AddScannableItem(groceryItem, unitPrice);
            return checkoutManager;
        }

        private GroceryPOSSystem SetupAndScan(string groceryItem, double unitPrice, double quantity = 1) {
            var checkoutManager = SetupCheckoutManager(groceryItem, unitPrice);
            checkoutManager.ScanItem(groceryItem, quantity);
            return checkoutManager;
        }
        #endregion
    }
}