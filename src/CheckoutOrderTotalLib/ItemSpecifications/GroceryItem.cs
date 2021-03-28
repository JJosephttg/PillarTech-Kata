namespace CheckoutOrderTotalLib {
    internal class GroceryItem {
        public double UnitPrice { get; set; }
        public double MarkDownPrice { get; set; }
        public double OrderQuantity { get; set; }
        public ISpecial CurrentSpecial { get; set; }

        public double GetAdjustedPrice() => UnitPrice - MarkDownPrice;
        public double GetTotalPrice() {
            var specialResult = CurrentSpecial?.Apply(this) ?? new SpecialResult(0, OrderQuantity);
            return specialResult.SpecialTotal + GetAdjustedPrice() * specialResult.QuantityLeft;
        }
    }
}
