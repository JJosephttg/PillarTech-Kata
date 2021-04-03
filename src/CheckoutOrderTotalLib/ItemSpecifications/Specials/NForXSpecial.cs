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
            // Get the limit of how many times we can apply the special (Either our order quantity is small enough to be under our limit, or the limit is beyond what we have ordered)
            var limit = maxLimitAmt > _limit ? _limit : maxLimitAmt;
            //Multiply our calculated limit by the discount price to get the total applied special price
            //For the quantity it will be our total quantity minus what we applied to the special (Calculated limit * the quantity that qualified for it)
            return new SpecialResult(limit * _discountPrice, orderQty - limit * _qualifiedQty);
        }
    }
}
