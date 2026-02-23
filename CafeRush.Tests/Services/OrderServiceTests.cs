using CafeRush.Domain;
using CafeRush.Services;

namespace CafeRush.Tests.Services
{
    [TestFixture]
    public class OrderServiceTests
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
        public void InitialState_MoneyShouldBeZero()
        {
            decimal money = _orderService.Money;

            Assert.That(money, Is.EqualTo(0m));
        }

        [Test]
        public void InitialState_OrderCountShouldBeZero()
        {
            int count = _orderService.OrderCount;

            Assert.That(count, Is.EqualTo(0));
        }

        [Test]
        public void RestartDay_ShouldResetOrderCount()
        {
            _orderService.Orders[0] = new Order();
            _orderService.OrderCount = 3;

            _orderService.RestartDay();

            Assert.That(_orderService.OrderCount, Is.EqualTo(0));
        }

        [Test]
        public void RestartDay_ShouldResetMoney()
        {
            _orderService.Money = 150m;

            _orderService.RestartDay();

            Assert.That(_orderService.Money, Is.EqualTo(0m));
        }

        [Test]
        public void RestartDay_ShouldResetRatingCount()
        {
            _orderService.ServiceRatings[0] = 5;
            _orderService.RatingCount = 1;

            _orderService.RestartDay();

            Assert.That(_orderService.RatingCount, Is.EqualTo(0));
        }
    }
}