using System;
using System.Collections.Generic;

namespace CheckoutOrderTotalLib {
    internal class StockInventoryManager {
        readonly Dictionary<string, GroceryItem> _groceryPriceMap = new Dictionary<string, GroceryItem>();

        public void AddProduct(string itemId, double unitPrice) {
            if (!_groceryPriceMap.TryGetValue(itemId, out GroceryItem groceryItem)) _groceryPriceMap.Add(itemId, groceryItem = new GroceryItem());
            groceryItem.UnitPrice = unitPrice;
        }

        public void ConfigureMarkdown(string itemId, double markdown) {
            if(!PerformWorkIfItemExists(itemId, groceryItem => {
                    if (markdown < 0 || markdown >= groceryItem.UnitPrice) throw new ArgumentOutOfRangeException("markdown", "Markdown must be a positive number and less than unit price of item being marked down");
                    groceryItem.MarkDownPrice = markdown;
                })
            ) throw new ArgumentException("Item ID not found in inventory. Make sure that you add the item first with AddItem", "itemId");
        }

        public void ConfigureSpecial(string itemId, ISpecial special) => PerformWorkIfItemExists(itemId, groceryItem => groceryItem.CurrentSpecial = special);

        public bool PerformWorkIfItemExists(string itemId, Action<GroceryItem> work) {
            if (_groceryPriceMap.TryGetValue(itemId, out GroceryItem groceryItem)) {
                work(groceryItem);
                return true;
            }
            return false;
        }
    }
}
