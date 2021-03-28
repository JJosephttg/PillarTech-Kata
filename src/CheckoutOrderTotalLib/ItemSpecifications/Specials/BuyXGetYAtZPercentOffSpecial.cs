namespace CheckoutOrderTotalLib {
    public class BuyXGetYAtZPercentOffSpecial : ISpecial {
        double _qualifiedQty, _discountedQty, _percentOff;
        public BuyXGetYAtZPercentOffSpecial(double qualifiedQty, double discountedQty, double percentOff) {
            _qualifiedQty = qualifiedQty;
            _discountedQty = discountedQty;
            _percentOff = percentOff;
        }

        //How special works:
        //If quantity ordered doesn't meet necessary quantity (X), special won't be applied
        //Quantity up to qualified amount is going to be at normal/marked down price
        //Quantity up to discounted quantity after qualified amount gets special discounted price;

        //while qualified items left (X items remaining?) (and optionally limit)
        //get up to next 2 discounted Y items (If discounted item count > _discountQty, use _discountQty)
        //Add to total the qualified items and discounted quantity
        SpecialResult ISpecial.Apply(GroceryItem groceryItem) {
            double qtyLeft = groceryItem.OrderQuantity, total = 0;
            for (double discountQtyLeft; (discountQtyLeft = qtyLeft - _qualifiedQty) > 0;) {
                var adjustedPrice = groceryItem.GetAdjustedPrice();
                var discountQtyToApply = discountQtyLeft > _discountedQty ? _discountedQty : discountQtyLeft;

                total += (_qualifiedQty * adjustedPrice) + (discountQtyToApply * adjustedPrice * (1 - (_percentOff * .01)));
                qtyLeft = groceryItem.OrderQuantity - (_qualifiedQty + discountQtyToApply);
            }
            return new SpecialResult(total, qtyLeft);
        }
    }
}
