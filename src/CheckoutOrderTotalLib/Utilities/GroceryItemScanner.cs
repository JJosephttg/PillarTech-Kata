using System.Collections.Generic;
using System.Linq;

namespace CheckoutOrderTotalLib {
    internal class GroceryItemScanner {
        readonly HashSet<GroceryItem> _checkoutOrder = new HashSet<GroceryItem>();

        public void ScanItem(GroceryItem groceryItem, double weightOrQty) {
            _checkoutOrder.Add(groceryItem);
            groceryItem.OrderQuantity += weightOrQty;
        }

        public void RemoveItem(GroceryItem groceryItem) => _checkoutOrder.Remove(groceryItem);

        public double GetPreTaxTotal() => _checkoutOrder.Sum(x => x.GetTotalPrice());
    }
}
