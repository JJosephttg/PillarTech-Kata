using System.Linq;
using System.Collections.Generic;
using System;

namespace CheckoutOrderTotalLib {
   /* Expected API Calls:
    * SetProductUnitPrice(item, price)
    * SetMarkDown(item, unitpriceAmtTakenOff)
    * SetSpecial(item, specialType, limitAmt)
    * ScanItem(item, weight = 1)
    * GetTotalPrice()
    * RemoveProduct(item)
    */
    public class CheckoutOrderManager {
        // Separate checkout from price config for efficiency purposes when calculating total (Think about the 1000's of inventory a store has but how little a customer actually orders)
        Dictionary<string, GroceryItem> _groceryPriceMap = new Dictionary<string, GroceryItem>();
        HashSet<GroceryItem> _checkoutOrder = new HashSet<GroceryItem>();

        /// <summary>
        /// Adds specified identifier to scannable items using the base price
        /// </summary>
        /// <param name="itemId">Grocery item identifier</param>
        /// <param name="price">Base unit price</param>
        public void AddScannableItem(string itemId, double price) {
            if (!_groceryPriceMap.TryGetValue(itemId, out GroceryItem groceryItem)) _groceryPriceMap.Add(itemId, groceryItem = new GroceryItem());
            groceryItem.UnitPrice = price;
        }

        /// <summary>
        /// Sets a markdown of an existing grocery item
        /// </summary>
        /// <param name="itemId">Grocery item identifier</param>
        /// <param name="markdown">price of markdown (How much to take off of price)</param>
        /// <returns>True if markdown is set and false if the item base price has not been configured yet</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if markdown is greater than unit price or is a negative number</exception>
        /// <exception cref="System.ArgumentException">Thrown if item ID has not been configured yet via the AddItem method</exception>
        public void SetMarkdown(string itemId, double markdown) {
            if (!_groceryPriceMap.TryGetValue(itemId, out GroceryItem groceryItem)) throw new ArgumentException("Item ID not found in inventory. Make sure that you add the item first with AddItem", "itemId");
            if (markdown < 0 || markdown >= groceryItem.UnitPrice) throw new ArgumentOutOfRangeException("markdown", "Markdown must be a positive number and less than unit price of item being marked down");
            
            groceryItem.MarkDownPrice = markdown;
        }

        /// <summary>
        /// Adds grocery item and quantity to checkout
        /// </summary>
        /// <param name="itemId">Grocery item identifier</param>
        /// <param name="weightOrQty">weight or quantity to add</param>
        /// <returns>True if item is scanned, and false if item is not a valid/configured grocery item. Items can be configured beforehand with the SetProductUnitPrice method</returns>
        public bool ScanItem(string itemId, double weightOrQty = 1) {
            if (_groceryPriceMap.TryGetValue(itemId, out GroceryItem gItem)) {
                _checkoutOrder.Add(gItem);
                gItem.OrderQuantity += weightOrQty;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes grocery item from checkout
        /// </summary>
        /// <param name="itemId">Grocery item identifier</param>
        public void RemoveScannedItem(string itemId) {
            if (_groceryPriceMap.TryGetValue(itemId, out GroceryItem groceryItem)) _checkoutOrder.Remove(groceryItem);
        }

        /// <summary>
        /// Calculates total pre-tax price of all current checkout items
        /// </summary>
        /// <returns>Total pre-tax price of current checkout items</returns>
        public double GetTotalPrice() => _checkoutOrder.Sum(x => x.GetAdjustedPrice());
    }
}
