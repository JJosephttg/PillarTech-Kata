using System;

namespace PillarTech_Kata {
    class Program {
        /*
         * Understanding Requirements:
         * 
         * Goal: To calculate pre-tax total price as items are scanned in
         * 
         * Memory-based
         * Structured as logical function calls all working together - Most likely a class to hold/manage items in checkout
         * 
         * Classifications
         * 
         * Checkout Items:
         * Can use strings to identify items - (Ground beef, soup, etc..)
         * Can be sold by weight or per-unit - Can possibly reduce to per-"unit" (considering weight and unit can be simplified to denominator amounts; for example: per pound, per ounce, per unit)
         * Items can be marked down - Alternate pricing. Specials can have limits
         * 
         * Discounts:
         * Different types as described in Functional Requirements
         *  
         * Functional Requirements:
         * Accept scanned item (and weight... Add item)
         * Accept markdowns (discount price) as well as specials optionally with limits (Get N items get M at % off, N for $X, Get N, get M of equal/lesser value for % off weighted items) - Quantity seems needed in this case
         * Remove scanned items (Can affect special price so a check is needed)
         * *Configure pricing before item is scanned
         * 
         */
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");
        }
    }
}
