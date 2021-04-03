namespace CheckoutOrderTotalLib {
    public abstract class SpecialBase {
        protected readonly double _qualifiedQty;
        protected readonly int _limit;
        public SpecialBase(double qualifiedQty, int limit) {
            InputChecker.CheckBadInput(qualifiedQty, nameof(qualifiedQty));
            InputChecker.CheckBadInput(limit, nameof(limit));

            _qualifiedQty = qualifiedQty;
            _limit = limit;
        }

        internal abstract SpecialResult Apply(GroceryItem groceryItem);
    }
}
