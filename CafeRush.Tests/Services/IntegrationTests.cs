using CafeRush.Domain;
using CafeRush.Services;

namespace CafeRush.Tests.Services
{
    [TestFixture]
    public class IntegrationTests
    {
        private MenuService _menuService;
        private Stock _stock;
        private OrderService _orderService;

        [SetUp]
        public void Setup()
        {
            _menuService = new MenuService();
            _stock = new Stock();
            _orderService = new OrderService(_stock, _menuService);
        }

        [Test]
        public void PrepareEspresso_ShouldDecreaseBeans()
        {
            Drink espresso = _menuService.Menu[0];
            int initialBeans = _stock.CoffeeBeans;
            Order order = new Order();
            order.AddDrink(espresso);

            order.Prepare(_stock);

            Assert.That(_stock.CoffeeBeans, Is.EqualTo(initialBeans - espresso.BeansNeeded));
        }

        [Test]
        public void PrepareEspresso_ShouldDecreaseCups()
        {
            Drink espresso = _menuService.Menu[0];
            int initialCups = _stock.Cups;
            Order order = new Order();
            order.AddDrink(espresso);

            order.Prepare(_stock);

            Assert.That(_stock.Cups, Is.EqualTo(initialCups - espresso.CupsNeeded));
        }

        [Test]
        public void AddCustomDrinkThenOrder_ShouldConsumeExtraIngredient()
        {
            _stock.AddExtra("Сироп", 100);
            string[] ing = { "молоко", "Сироп" };
            int[] amt = { 50, 10 };
            _menuService.AddDrinkByDetails("СиропнийЛатте", 100m, ing, amt);
            Drink myDrink = _menuService.Menu[_menuService.MenuCount - 1];
            Order order = new Order();
            order.AddDrink(myDrink);

            order.Prepare(_stock);

            Assert.That(_stock.HasExtra("Сироп", 90), Is.True);
        }

        [Test]
        public void AddCustomDrinkThenOrder_TotalShouldBeCorrect()
        {
            _stock.AddExtra("Сироп", 100);
            string[] ing = { "молоко", "Сироп" };
            int[] amt = { 50, 10 };
            _menuService.AddDrinkByDetails("СиропнийЛатте", 100m, ing, amt);
            Drink myDrink = _menuService.Menu[_menuService.MenuCount - 1];
            Order order = new Order();
            order.AddDrink(myDrink);

            order.Prepare(_stock);

            Assert.That(order.Total, Is.EqualTo(100m));
        }

        [Test]
        public void TwoOrders_OrderCountShouldBeTwo()
        {
            Order order1 = new Order();
            order1.AddDrink(_menuService.Menu[0]); 

            Order order2 = new Order();
            order2.AddDrink(_menuService.Menu[1]); 

            order1.Prepare(_stock);
            _orderService.Orders[_orderService.OrderCount] = order1;
            _orderService.OrderCount++;
            _orderService.Money = _orderService.Money + order1.Total;

            order2.Prepare(_stock);
            _orderService.Orders[_orderService.OrderCount] = order2;
            _orderService.OrderCount++;
            _orderService.Money = _orderService.Money + order2.Total;

            Assert.That(_orderService.OrderCount, Is.EqualTo(2));
        }

        [Test]
        public void TwoOrders_TotalMoneyShouldMatch()
        {
            Order order1 = new Order();
            order1.AddDrink(_menuService.Menu[0]); 

            Order order2 = new Order();
            order2.AddDrink(_menuService.Menu[1]); 

            order1.Prepare(_stock);
            _orderService.Orders[_orderService.OrderCount] = order1;
            _orderService.OrderCount++;
            _orderService.Money = _orderService.Money + order1.Total;

            order2.Prepare(_stock);
            _orderService.Orders[_orderService.OrderCount] = order2;
            _orderService.OrderCount++;
            _orderService.Money = _orderService.Money + order2.Total;

            Assert.That(_orderService.Money, Is.EqualTo(100m));
        }

        [Test]
        public void NotEnoughStock_CanPrepareShouldReturnFalse()
        {
            _stock.CoffeeBeans = 0;
            Order order = new Order();
            order.AddDrink(_menuService.Menu[1]); 

            bool canPrepare = order.CanPrepare(_stock);

            Assert.That(canPrepare, Is.False);
        }

        [Test]
        public void NotEnoughStock_MilkShouldRemainUnchanged()
        {
            _stock.CoffeeBeans = 0;
            int initialMilk = _stock.Milk;
            Order order = new Order();
            order.AddDrink(_menuService.Menu[1]); 

            bool canPrepare = order.CanPrepare(_stock);
            if (canPrepare)
            {
                order.Prepare(_stock);
            }

            Assert.That(_stock.Milk, Is.EqualTo(initialMilk));
        }
    }
}