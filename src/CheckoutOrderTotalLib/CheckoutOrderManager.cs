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

        public void SetProductUnitPrice(string itemId, double price) {
            var groceryItem = TryGetOrAdd(_groceryPriceMap, itemId, new GroceryItem());
            groceryItem.UnitPrice = price;
        }

        public void SetMarkdown(string itemId, double markdown) {
            var groceryItem = TryGetOrAdd(_groceryPriceMap, itemId, new GroceryItem());
            groceryItem.MarkDownPrice = markdown;
        }

        public bool ScanItem(string itemId, double weightOrQty = 1) {
            if (_groceryPriceMap.TryGetValue(itemId, out GroceryItem gItem)) {
                _checkoutOrder.Add(gItem);
                gItem.OrderQuantity += weightOrQty;

                return true;
            }
            return false;
        }

        public void RemoveScannedItem(string itemId) {
            if (_groceryPriceMap.TryGetValue(itemId, out GroceryItem groceryItem)) {
                var quantity = groceryItem.OrderQuantity;
                quantity = quantity - 1 <= 0 ? 0 : --quantity;

                if (quantity <= 0) _checkoutOrder.Remove(groceryItem); //Remove if exists in checkout order since customer is not ordering this item any more
                
                groceryItem.OrderQuantity = quantity;
            }    
        }

        public double GetTotalPrice() => _checkoutOrder.Sum(x => x.GetAdjustedPrice());

        private V TryGetOrAdd<K, V>(Dictionary<K, V> map, K key, V newVal) {
            if (!map.TryGetValue(key, out V val)) map.Add(key, val = newVal);
            return val;
        }
    }
}
