namespace CheckoutOrderTotalLib {
    public class NForXSpecial : SpecialBase {
        readonly double _discountPrice;
        public NForXSpecial(double qualifiedQty, double discountPrice, int limit = 1) : base(qualifiedQty, limit) {
            InputChecker.CheckBadInput(discountPrice, nameof(discountPrice));
            _discountPrice = discountPrice;
        }

        internal override SpecialResult Apply(GroceryItem groceryItem) {
            double totalQtyLeft = groceryItem.OrderQuantity, total = 0;

            for (int limit = 0; totalQtyLeft - _qualifiedQty >= 0 && limit < _limit; limit++) {
                total += _discountPrice;
                totalQtyLeft -= _qualifiedQty;
            }
            return new SpecialResult(total, totalQtyLeft);
        }
    }
}
