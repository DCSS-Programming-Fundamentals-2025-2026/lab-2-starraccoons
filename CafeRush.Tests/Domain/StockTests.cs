using CafeRush.Domain;

namespace CafeRush.Tests.Domain
{
    [TestFixture]
    public class StockTests
    {
        private Stock _stock;

        [SetUp]
        public void Setup()
        {
            _stock = new Stock();
        }

        [Test]
        public void AddExtra_NewIngredient_ShouldIncreaseExtraCount()
        {
            int countBefore = _stock.ExtraCount;

            _stock.AddExtra("Сироп", 100);

            Assert.That(_stock.ExtraCount, Is.EqualTo(countBefore + 1));
        }

        [Test]
        public void AddExtra_ExistingIngredient_ShouldAccumulateAmount()
        {
            _stock.AddExtra("Сироп", 100);

            _stock.AddExtra("Сироп", 50);

            Assert.That(_stock.HasExtra("Сироп", 150), Is.True);
        }

        [Test]
        public void HasExtra_WithUkrainianNameMapping_ShouldReturnTrue()
        {
            _stock.CoffeeBeans = 100;

            bool result = _stock.HasExtra("кава", 50);

            Assert.That(result, Is.True);
        }

        [Test]
        public void HasExtra_NotEnoughAmount_ShouldReturnFalse()
        {
            _stock.CoffeeBeans = 5;

            bool result = _stock.HasExtra("coffee", 10);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Consume_CoffeeBeans_ShouldDecreaseAmount()
        {
            int initial = _stock.CoffeeBeans;

            _stock.Consume("кава", 30);

            Assert.That(_stock.CoffeeBeans, Is.EqualTo(initial - 30));
        }

        [Test]
        public void Consume_ExtraIngredient_ShouldDecreaseAmount()
        {
            _stock.AddExtra("Карамель", 200);

            _stock.Consume("Карамель", 50);

            Assert.That(_stock.HasExtra("Карамель", 150), Is.True);
        }

        [Test]
        public void HasExtra_DefaultMilk_ShouldReturnTrue()
        {
            bool result = _stock.HasExtra("молоко", 100);

            Assert.That(result, Is.True);
        }
    }
}