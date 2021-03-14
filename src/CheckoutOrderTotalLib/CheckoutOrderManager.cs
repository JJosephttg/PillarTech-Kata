using System.Linq;
using System.Collections.Generic;

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
        Dictionary<string, double> _groceryPriceMap = new Dictionary<string, double>();
        List<GroceryItemOrder> _checkoutOrder = new List<GroceryItemOrder>();

        public void SetProductUnitPrice(string groceryItem, double price) {
            _groceryPriceMap[groceryItem] = price;
        }

        public bool ScanItem(string groceryItem, double weightOrQty = 1) {
            if (_groceryPriceMap.TryGetValue(groceryItem, out double price)) {
                _checkoutOrder.Add(new GroceryItemOrder(groceryItem, price * weightOrQty));
                return true;
            }
            return false;
        }

        public double GetTotalPrice() { return _checkoutOrder.Sum(x => x.Price); }
    }
}
