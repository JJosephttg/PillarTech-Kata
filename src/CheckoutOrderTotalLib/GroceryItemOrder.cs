namespace CheckoutOrderTotalLib {
    public class GroceryItemOrder {
        public GroceryItemOrder(string name, double price) {
            Name = name;
            Price = price;
        }

        public string Name { get; }
        public double Price { get; }
    }
}
