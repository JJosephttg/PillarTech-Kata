namespace CheckoutOrderTotalLib {
    internal class GroceryItem {
        public double UnitPrice { get; set; }
        public double MarkDownPrice { get; set; }
        public double OrderQuantity { get; set; }

        public double GetAdjustedPrice() => (UnitPrice - MarkDownPrice) * OrderQuantity;
    }
}
