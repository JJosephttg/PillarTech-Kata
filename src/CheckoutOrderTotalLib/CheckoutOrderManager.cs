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
        public void AddItem(string itemId, double price) {
            if (!_groceryPriceMap.TryGetValue(itemId, out GroceryItem groceryItem)) _groceryPriceMap.Add(itemId, groceryItem = new GroceryItem());
            groceryItem.UnitPrice = price;
        }

        /// <summary>
        /// Sets a markdown of an existing grocery item
        /// </summary>
        /// <param name="itemId">Grocery item identifier</param>
        /// <param name="markdown">price of markdown (How much to take off of price)</param>
        /// <returns>True if markdown is set and false if the item base price has not been configured yet</returns>
        public bool SetMarkdown(string itemId, double markdown) {
            if (!_groceryPriceMap.TryGetValue(itemId, out GroceryItem groceryItem)) return false;
            groceryItem.MarkDownPrice = markdown;
            return true;
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
            if (_groceryPriceMap.TryGetValue(itemId, out GroceryItem groceryItem)) {
                var quantity = groceryItem.OrderQuantity;
                quantity = quantity - 1 <= 0 ? 0 : --quantity;

                if (quantity <= 0) _checkoutOrder.Remove(groceryItem); //Remove if exists in checkout order since customer is not ordering this item any more
                
                groceryItem.OrderQuantity = quantity;
            }    
        }

        /// <summary>
        /// Calculates total pre-tax price of all current checkout items
        /// </summary>
        /// <returns>Total pre-tax price of current checkout items</returns>
        public double GetTotalPrice() => _checkoutOrder.Sum(x => x.GetAdjustedPrice());
    }
}
