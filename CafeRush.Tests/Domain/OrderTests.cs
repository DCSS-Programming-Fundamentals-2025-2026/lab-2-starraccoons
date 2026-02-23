using CafeRush.Domain;
using CafeRush.Services;

namespace CafeRush.Tests.Domain
{
    [TestFixture]
    public class OrderTests
    {
        private Stock _stock;
        private MenuService _menuService;

        [SetUp]
        public void Setup()
        {
            _stock = new Stock();
            _menuService = new MenuService();
        }

        [Test]
        public void AddDrink_ShouldIncreaseDrinksCount()
        {
            Order order = new Order();
            Drink d = new Drink("TestDrink", 30m, 5, 5, 1, 2);

            order.AddDrink(d);

            Assert.That(order.DrinksCount, Is.EqualTo(1));
        }

        [Test]
        public void AddDrink_TwoDrinks_ShouldUpdateTotalCorrectly()
        {
            Order order = new Order();
            Drink d1 = new Drink("D1", 10.5m, 1, 1, 1, 1);
            Drink d2 = new Drink("D2", 20.0m, 1, 1, 1, 1);

            order.AddDrink(d1);
            order.AddDrink(d2);

            Assert.That(order.Total, Is.EqualTo(30.5m));
        }

        [Test]
        public void CanPrepare_NotEnoughCoffeeBeans_ShouldReturnFalse()
        {
            _stock.CoffeeBeans = 5;
            Order order = new Order();
            order.AddDrink(_menuService.Menu[0]);

            bool canMake = order.CanPrepare(_stock);

            Assert.That(canMake, Is.False);
        }
    }
}