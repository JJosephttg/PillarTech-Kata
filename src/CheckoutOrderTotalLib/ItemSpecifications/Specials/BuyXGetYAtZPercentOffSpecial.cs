namespace CheckoutOrderTotalLib {
    public class BuyXGetYAtZPercentOffSpecial : SpecialBase {
        readonly double _discountedQty, _percentOff;

        public BuyXGetYAtZPercentOffSpecial(double qualifiedQty, double discountedQty, double percentOff, int limit = 1) : base(qualifiedQty, limit) {
            var percentOffName = nameof(percentOff);
            InputChecker.CheckBadInput(discountedQty, nameof(discountedQty));
            InputChecker.CheckBadInput(percentOff, percentOffName);
            if (percentOff > 100) InputChecker.ThrowOutOfRange(percentOffName, $"{percentOffName} must be a valid percentage");

            _discountedQty = discountedQty;
            _percentOff = percentOff;
        }

        internal override SpecialResult Apply(GroceryItem groceryItem) {
            double qtyLeft = groceryItem.OrderQuantity, total = 0;
            int limit = 0;
            var adjustedPrice = groceryItem.GetAdjustedPrice();
            for (double discountQtyLeft; (discountQtyLeft = qtyLeft - _qualifiedQty) > 0 && limit < _limit; limit++) {
                var discountQtyToApply = discountQtyLeft > _discountedQty ? _discountedQty : discountQtyLeft;

                total += (_qualifiedQty * adjustedPrice) + (discountQtyToApply * adjustedPrice * (1 - (_percentOff * .01)));
                qtyLeft -= _qualifiedQty + discountQtyToApply;
            }
            return new SpecialResult(total, qtyLeft);
        }
    }
}
