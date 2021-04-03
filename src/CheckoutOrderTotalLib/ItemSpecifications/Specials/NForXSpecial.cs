namespace CheckoutOrderTotalLib {
    public class NForXSpecial : SpecialBase {
        readonly double _discountPrice;
        public NForXSpecial(double qualifiedQty, double discountPrice, int limit = 1) : base(qualifiedQty, limit) {
            InputChecker.CheckBadInput(discountPrice, nameof(discountPrice));
            _discountPrice = discountPrice;
        }

        internal override SpecialResult Apply(GroceryItem groceryItem) {
            double orderQty = groceryItem.OrderQuantity;
            int maxLimitAmt = (int)(orderQty / _qualifiedQty);
            var limit = maxLimitAmt > _limit ? _limit : maxLimitAmt;
            return new SpecialResult(limit * _discountPrice, orderQty - limit * _qualifiedQty);
        }
    }
}
