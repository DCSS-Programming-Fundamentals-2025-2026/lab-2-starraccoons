using CafeRush.Domain;
using CafeRush.Services;
using NUnit.Framework;

namespace CafeRush.Tests
{
    [TestFixture]
    public class StockAndOrderTests
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
        public void Stock_AddExtra_NewIngredient_ShouldIncreaseExtraCount()
        {
            //Arrange
            int countBefore = _stock.ExtraCount;

            //Act
            _stock.AddExtra("Сироп", 100);

            //Assert
            Assert.That(_stock.ExtraCount, Is.EqualTo(countBefore + 1));
        }

        [Test]
        public void Stock_AddExtra_ExistingIngredient_ShouldAccumulateAmount()
        {
            //Arrange
            _stock.AddExtra("Сироп", 100);

            //Act
            _stock.AddExtra("Сироп", 50);

            //Assert
            Assert.That(_stock.HasExtra("Сироп", 150), Is.True);
        }

        [Test]
        public void Stock_HasExtra_WithUkrainianNameMapping_ShouldReturnTrue()
        {
            //Arrange
            _stock.CoffeeBeans = 100;

            //Act
            bool result = _stock.HasExtra("кава", 50);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Stock_HasExtra_NotEnoughAmount_ShouldReturnFalse()
        {
            //Arrange
            _stock.CoffeeBeans = 5;

            //Act
            bool result = _stock.HasExtra("coffee", 10);

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Stock_Consume_ShouldDecreaseCoffeeBeans()
        {
            //Arrange
            int initial = _stock.CoffeeBeans;

            //Act
            _stock.Consume("кава", 30);

            //Assert
            Assert.That(_stock.CoffeeBeans, Is.EqualTo(initial - 30));
        }

        [Test]
        public void Stock_Consume_ExtraIngredient_ShouldDecreaseExtraAmount()
        {
            //Arrange
            _stock.AddExtra("Карамель", 200);

            //Act
            _stock.Consume("Карамель", 50);

            //Assert
            Assert.That(_stock.HasExtra("Карамель", 150), Is.True);
        }

        [Test]
        public void Stock_HasExtra_DefaultMilk_ShouldReturnTrue()
        {
            //Arrange

            // Act
            bool result = _stock.HasExtra("молоко", 100);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Order_AddDrink_ShouldIncreaseDrinksCount()
        {
            //Arrange
            Order order = new Order();
            Drink d = new Drink("TestDrink", 30m, 5, 5, 1, 2);

            //Act
            order.AddDrink(d);

            //Assert
            Assert.That(order.DrinksCount, Is.EqualTo(1));
        }

        [Test]
        public void Order_AddDrink_ShouldUpdateTotalCorrectly()
        {
            //Arrange
            Order order = new Order();
            Drink d1 = new Drink("D1", 10.5m, 1, 1, 1, 1);
            Drink d2 = new Drink("D2", 20.0m, 1, 1, 1, 1);

            //Act
            order.AddDrink(d1);
            order.AddDrink(d2);

            //Assert
            Assert.That(order.Total, Is.EqualTo(30.5m));
        }

        [Test]
        public void Order_CanPrepare_NotEnoughCoffeeBeans_ShouldReturnFalse()
        {
            //Arrange
            _stock.CoffeeBeans = 5; 
            Order order = new Order();
            order.AddDrink(_menuService.Menu[0]); 

            //Act
            bool canMake = order.CanPrepare(_stock);

            //Assert
            Assert.That(canMake, Is.False);
        }
    }
}