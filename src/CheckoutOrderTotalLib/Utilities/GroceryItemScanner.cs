using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckoutOrderTotalLib {
    internal class GroceryItemScanner {
        HashSet<GroceryItem> _checkoutOrder = new HashSet<GroceryItem>();

        public void ScanItem(GroceryItem groceryItem, double weightOrQty) {
            if (weightOrQty <= 0 || !double.IsFinite(weightOrQty)) throw new ArgumentOutOfRangeException("weightOrQty", "Weight/Quantity must be finite and be greater than 0");
            _checkoutOrder.Add(groceryItem);
            groceryItem.OrderQuantity += weightOrQty;
        }

        public void RemoveItem(GroceryItem groceryItem) => _checkoutOrder.Remove(groceryItem);

        public double GetPreTaxTotal() => _checkoutOrder.Sum(x => x.GetTotalPrice());
    }
}
