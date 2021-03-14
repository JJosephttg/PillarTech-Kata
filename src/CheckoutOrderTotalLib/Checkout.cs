using System.Linq;
using System.Collections.Generic;

namespace CheckoutOrderTotalLib {
    public class CheckoutOrderManager {
        Dictionary<string, double> _groceryPriceMap = new Dictionary<string, double>();
        List<GroceryItemOrder> _checkoutOrder = new List<GroceryItemOrder>();

        public void ConfigureItemPrice(string groceryItem, double price) {
            _groceryPriceMap[groceryItem] = price;
        }

        public bool ScanItem(string groceryItem) {
            if (_groceryPriceMap.TryGetValue(groceryItem, out double price)) {
                _checkoutOrder.Add(new GroceryItemOrder(groceryItem, price));
                return true;
            }
            return false;
        }

        public double GetCurrentTotal() { return _checkoutOrder.Sum(x => x.Price); }
    }
}
