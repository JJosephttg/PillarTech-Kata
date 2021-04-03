using CheckoutOrderTotalLib;
using NUnit.Framework;
using System;

namespace CheckoutOrderTotalTests {
    public class GroceryItemConfigTests : GroceryPOSSystemTestBase {
        [TestFixture]
        public class AddItemTests : GroceryPOSSystemTestBase {
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

            [Test]
            [TestCaseSource(nameof(InvalidNumbers))]
            public void ConfiguringItemWithInvalidQuantityThrowsException(double qty) {
                var checkoutManager = SetupCheckoutManager(C_DefaultItem, 5);
                AssertExceptionParam<ArgumentOutOfRangeException>(() => checkoutManager.ScanItem(C_DefaultItem, qty), "weightOrQty");
                Assert.AreEqual(0, checkoutManager.GetTotalPrice());
            }

            [Test]
            [TestCaseSource(nameof(InvalidNumbers))]
            public void ConfiguringItemWithInvalidPricingThrowsException(double price) {
                var checkoutManager = new GroceryPOSSystem();
                AssertExceptionParam<ArgumentOutOfRangeException>(() => checkoutManager.AddScannableItem(C_DefaultItem, price), "unitPrice");
                Assert.AreEqual(0, checkoutManager.GetTotalPrice());
            }
        }

        [TestFixture]
        public class SetMarkdownTests : GroceryPOSSystemTestBase {
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
        }

        [TestFixture]
        public class SetSpecialTests : GroceryPOSSystemTestBase {
            [Test]
            [TestCase(1, 1, 100, 50)]
            [TestCase(2, 1, 50, 125)]
            public void BuyXGetYAtZPercentOffSpecialReflectsCorrectTotal(double qualifiedQty, double discountedQty, double percentOff, double expectedTotal) {
                var checkoutManager = SetupAndScan(C_DefaultItem, C_DefaultUnitPrice, qualifiedQty + discountedQty);
                checkoutManager.SetSpecial(C_DefaultItem, new BuyXGetYAtZPercentOffSpecial(qualifiedQty, discountedQty, percentOff));
                Assert.AreEqual(expectedTotal, checkoutManager.GetTotalPrice());
            }
        }
    }
}
