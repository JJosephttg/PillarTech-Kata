namespace CheckoutOrderTotalLib {
    internal class SpecialResult {
        public SpecialResult(double specialTotal, double quantityLeft) {
            SpecialTotal = specialTotal;
            QuantityLeft = quantityLeft;
        }

        public double SpecialTotal { get; }
        public double QuantityLeft { get; }
    }
}
