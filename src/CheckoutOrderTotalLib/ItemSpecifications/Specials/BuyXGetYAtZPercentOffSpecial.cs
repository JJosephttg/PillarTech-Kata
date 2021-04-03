﻿namespace CheckoutOrderTotalLib {
    public class BuyXGetYAtZPercentOffSpecial : ISpecial {
        readonly double _qualifiedQty, _discountedQty, _percentOff;
        readonly int _limit;

        public BuyXGetYAtZPercentOffSpecial(double qualifiedQty, double discountedQty, double percentOff, int limit = 1) {
            var percentOffName = nameof(percentOff);
            InputChecker.CheckBadInput(qualifiedQty, nameof(qualifiedQty));
            InputChecker.CheckBadInput(discountedQty, nameof(discountedQty));
            InputChecker.CheckBadInput(percentOff, percentOffName);
            InputChecker.CheckBadInput(limit, nameof(limit));
            if (percentOff > 100) InputChecker.ThrowOutOfRange(percentOffName, $"{percentOffName} must be a valid percentage");

            _qualifiedQty = qualifiedQty;
            _discountedQty = discountedQty;
            _percentOff = percentOff;
            _limit = limit;
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
