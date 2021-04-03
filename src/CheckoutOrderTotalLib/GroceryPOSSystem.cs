namespace CheckoutOrderTotalLib {
    public class GroceryPOSSystem {
        // Separate checkout from price config for efficiency purposes when calculating total (Think about the 1000's of inventory a store has but how little a customer actually orders)
        readonly GroceryItemScanner _scanner = new GroceryItemScanner();
        readonly StockInventoryManager _inventoryManager = new StockInventoryManager();

        /// <summary>
        /// Adds specified identifier to scannable items using the base price
        /// </summary>
        /// <param name="itemId">Grocery item identifier</param>
        /// <param name="price">Base unit price</param>
        public void AddScannableItem(string itemId, double unitPrice) {
            InputChecker.CheckBadInput(unitPrice, nameof(unitPrice));
            _inventoryManager.AddProduct(itemId, unitPrice);
        }

        #region Configuration
        /// <summary>
        /// Sets a markdown of an existing grocery item
        /// </summary>
        /// <param name="itemId">Grocery item identifier</param>
        /// <param name="markdown">price of markdown (How much to take off of price)</param>
        /// <returns>True if markdown is set and false if the item base price has not been configured yet</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if markdown is greater than unit price or is a negative number</exception>
        /// <exception cref="System.ArgumentException">Thrown if item ID has not been configured yet via the AddItem method</exception>
        public void SetMarkdown(string itemId, double markdown) => _inventoryManager.ConfigureMarkdown(itemId, markdown);

        /// <summary>
        /// Sets the special of an existing grocery item
        /// </summary>
        /// <param name="itemId">Grocery item identifier</param>
        /// <param name="special">The type of special that is to be used with the grocery item</param>
        public void SetSpecial(string itemId, SpecialBase special = null) => _inventoryManager.ConfigureSpecial(itemId, special);
        #endregion

        #region Scanning
        /// <summary>
        /// Adds grocery item and quantity to checkout
        /// </summary>
        /// <param name="itemId">Grocery item identifier</param>
        /// <param name="weightOrQty">weight or quantity to add</param>
        /// <returns>True if item is scanned, and false if item is not a valid/configured grocery item. Items can be configured beforehand with the SetProductUnitPrice method</returns>
        public bool ScanItem(string itemId, double weightOrQty = 1) {
            InputChecker.CheckBadInput(weightOrQty, nameof(weightOrQty));
            return _inventoryManager.PerformWorkIfItemExists(itemId, gItem => _scanner.ScanItem(gItem, weightOrQty));
        }

        /// <summary>
        /// Removes grocery item from checkout
        /// </summary>
        /// <param name="itemId">Grocery item identifier</param>
        public void RemoveScannedItem(string itemId) {
            _inventoryManager.PerformWorkIfItemExists(itemId, groceryItem => _scanner.RemoveItem(groceryItem));
        }
        #endregion

        /// <summary>
        /// Calculates total pre-tax price of all current checkout items
        /// </summary>
        /// <returns>Total pre-tax price of current checkout items</returns>
        public double GetTotalPrice() => _scanner.GetPreTaxTotal();
    }
}
