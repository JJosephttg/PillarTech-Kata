namespace CheckoutOrderTotalLib {
    public class NForXSpecial : ISpecial {
        readonly double _qualifiedQty, _discountPrice;
        public NForXSpecial(double qualifiedQty, double discountPrice) {
            _qualifiedQty = qualifiedQty;
            _discountPrice = discountPrice;
        }

        SpecialResult ISpecial.Apply(GroceryItem groceryItem) => 
            groceryItem.OrderQuantity >= _qualifiedQty ? new SpecialResult(_discountPrice, groceryItem.OrderQuantity - _qualifiedQty) : new SpecialResult(0, groceryItem.OrderQuantity);
    }
}
