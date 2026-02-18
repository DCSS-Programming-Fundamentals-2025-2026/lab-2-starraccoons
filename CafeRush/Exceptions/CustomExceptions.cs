
namespace CafeRush.Exceptions
{
    public class MenuFullException : Exception
    {
        public MenuFullException() : base("Menu is full.") { }
        public MenuFullException(string message) : base(message) { }
    }

    public class NotEnoughStockException : Exception
    {
        public NotEnoughStockException() : base("Not enough stock for the requested operation.") { }
        public NotEnoughStockException(string message) : base(message) { }
    }

    public class InvalidIngredientException : Exception
    {
        public InvalidIngredientException() : base("Invalid ingredient.") { }
        public InvalidIngredientException(string message) : base(message) { }
    }
}
