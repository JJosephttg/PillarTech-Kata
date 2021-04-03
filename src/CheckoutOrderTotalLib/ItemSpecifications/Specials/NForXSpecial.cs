namespace CheckoutOrderTotalLib {
    public class NForXSpecial : ISpecial {
        readonly double _qualifiedQty, _discountPrice;
        readonly int _limit;
        public NForXSpecial(double qualifiedQty, double discountPrice, int limit = 1) {
            _qualifiedQty = qualifiedQty;
            _discountPrice = discountPrice;
            _limit = limit;
        }

        SpecialResult ISpecial.Apply(GroceryItem groceryItem) {
            double totalQtyLeft = groceryItem.OrderQuantity, total = 0;

            for (int limit = 0; totalQtyLeft - _qualifiedQty >= 0 && limit < _limit; limit++) {
                total += _discountPrice;
                totalQtyLeft -= _qualifiedQty;
            }
            return new SpecialResult(total, totalQtyLeft);
        }
    }
}
