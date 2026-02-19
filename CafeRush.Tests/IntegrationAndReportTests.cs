using CafeRush.Domain;
using CafeRush.Services;
using NUnit.Framework;

namespace CafeRush.Tests
{
    [TestFixture]
    public class IntegrationAndReportTests
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
        public void OrderService_InitialState_MoneyShouldBeZero()
        {
            //Arrange + Act у Setup

            //Assert
            Assert.That(_orderService.Money, Is.EqualTo(0m));
        }

        [Test]
        public void OrderService_InitialState_OrderCountShouldBeZero()
        {
            //Arrange + Act у Setup

            // Assert
            Assert.That(_orderService.OrderCount, Is.EqualTo(0));
        }

        [Test]
        public void OrderService_RestartDay_ShouldResetOrderCountAndMoney()
        {
            //Arrange
            _orderService.Orders[0] = new Order();
            _orderService.OrderCount = 3;
            _orderService.Money = 150m;

            //Avt
            _orderService.RestartDay();

            // Asert
            Assert.Multiple(() =>
            {
                Assert.That(_orderService.OrderCount, Is.EqualTo(0));
                Assert.That(_orderService.Money, Is.EqualTo(0m));
            });
        }

        [Test]
        public void OrderService_RestartDay_ShouldResetRatingCount()
        {
            //Arange
            _orderService.ServiceRatings[0] = 5;
            _orderService.RatingCount = 1;

            //Act
            _orderService.RestartDay();

            //Assert
            Assert.That(_orderService.RatingCount, Is.EqualTo(0));
        }

        [Test]
        public void Drink_AddExtraIngredient_BeyondInitialCapacity_ShouldResize()
        {
            //Arrange
            Drink drink = new Drink("TestDrink", 50m, 0, 0, 1, 0);

            //Act
            for (int i = 0; i < 11; i++)
            {
                drink.AddExtraIngredient("Ing" + i, i + 1);
            }

            //Assert
            Assert.That(drink.ExtraIngredientCount, Is.EqualTo(11));
        }

        [Test]
        public void Drink_AddExtraIngredient_BeyondInitialCapacity_ArrayShouldGrow()
        {
            //Arrange
            Drink drink = new Drink("TestDrink", 50m, 0, 0, 1, 0);

            //Act
            for (int i = 0; i < 11; i++)
            {
                drink.AddExtraIngredient("Ing" + i, i + 1);
            }

            //Assert
            Assert.That(drink.ExtraIngredientNames.Length, Is.EqualTo(20));
        }

        [Test]
        public void Integration_PrepareEspresso_ShouldDecreaseBeansAndCups()
        {
            //Arrange
            Drink espresso = _menuService.Menu[0];
            int initialBeans = _stock.CoffeeBeans;
            int initialCups = _stock.Cups;
            Order order = new Order();
            order.AddDrink(espresso);

            //Act
            bool canPrepare = order.CanPrepare(_stock);
            if (canPrepare)
            {
                order.Prepare(_stock);
            }

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(canPrepare, Is.True);
                Assert.That(_stock.CoffeeBeans, Is.EqualTo(initialBeans - espresso.BeansNeeded));
                Assert.That(_stock.Cups, Is.EqualTo(initialCups - espresso.CupsNeeded));
            });
        }

        [Test]
        public void Integration_AddCustomDrinkToMenuThenOrder_ShouldConsumeExtraIngredient()
        {
            //Arrange
            _stock.AddExtra("Сироп", 100);
            string[] ing = { "молоко", "Сироп" };
            int[] amt = { 50, 10 };

            //Act
            _menuService.AddDrinkByDetails("СиропнийЛатте", 100m, ing, amt);
            Drink myDrink = _menuService.Menu[_menuService.MenuCount - 1];

            Order order = new Order();
            order.AddDrink(myDrink);

            bool canPrepare = order.CanPrepare(_stock);
            if (canPrepare)
            {
                order.Prepare(_stock);
            }

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(canPrepare, Is.True);
                Assert.That(order.Total, Is.EqualTo(100m));
                Assert.That(_stock.HasExtra("Сироп", 90), Is.True);
            });
        }

        [Test]
        public void Integration_MultipleOrdersThenReport_TotalMoneyShouldMatch()
        {
            //Arrange
            Drink espresso = _menuService.Menu[0];
            Drink latte = _menuService.Menu[1];

            Order order1 = new Order();
            order1.AddDrink(espresso);

            Order order2 = new Order();
            order2.AddDrink(latte);

            //Act
            if (order1.CanPrepare(_stock))
            {
                order1.Prepare(_stock);
                _orderService.Orders[_orderService.OrderCount] = order1;
                _orderService.OrderCount++;
                _orderService.Money = _orderService.Money + order1.Total;
            }

            if (order2.CanPrepare(_stock))
            {
                order2.Prepare(_stock);
                _orderService.Orders[_orderService.OrderCount] = order2;
                _orderService.OrderCount++;
                _orderService.Money = _orderService.Money + order2.Total;
            }

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(_orderService.OrderCount, Is.EqualTo(2));
                Assert.That(_orderService.Money, Is.EqualTo(100m));
            });
        }

        [Test]
        public void Integration_NotEnoughStock_OrderShouldNotModifyStock()
        {
            
            //Arrange
            _stock.CoffeeBeans = 0;
            int initialMilk = _stock.Milk;

            Order order = new Order();
            order.AddDrink(_menuService.Menu[1]); 

            //Act
            bool canPrepare = order.CanPrepare(_stock);
            if (canPrepare)
            {
                order.Prepare(_stock);
            }

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(canPrepare, Is.False);
                Assert.That(_stock.Milk, Is.EqualTo(initialMilk));
            });
        }
    }
}