
using CafeRush.Services;

namespace CafeRush.Tests.Services
{
    [TestFixture]
    public class MenuServiceTests
    {
        private MenuService _menuService;

        [SetUp]
        public void Setup()
        {
            _menuService = new MenuService();
        }

        [Test]
        public void RemoveDrinkByName_ExistingDrink_ShouldReturnTrue()
        {
            string drinkToRemove = "Espresso";

            bool result = _menuService.RemoveDrinkByName(drinkToRemove);

            Assert.That(result, Is.True);
        }

        [Test]
        public void RemoveDrinkByName_ExistingDrink_ShouldDecrementCount()
        {
            int initialCount = _menuService.MenuCount;

            _menuService.RemoveDrinkByName("Espresso");

            Assert.That(_menuService.MenuCount, Is.EqualTo(initialCount - 1));
        }

        [Test]
        public void RemoveDrinkByName_ExistingDrink_ShouldShiftArrayLeft()
        {
            _menuService.RemoveDrinkByName("Espresso");

            Assert.That(_menuService.Menu[0].Name, Is.EqualTo("Latte"));
        }

        [Test]
        public void RemoveDrinkByName_ExistingDrink_LastSlotShouldBeNull()
        {
            int countBefore = _menuService.MenuCount;

            _menuService.RemoveDrinkByName("Espresso");

            Assert.That(_menuService.Menu[countBefore - 1], Is.Null);
        }

        [Test]
        public void RemoveDrinkByName_NonExistentDrink_ShouldReturnFalse()
        {
            string fakeDrink = "NonExistent";

            bool result = _menuService.RemoveDrinkByName(fakeDrink);

            Assert.That(result, Is.False);
        }

        [Test]
        public void RemoveDrinkByName_NonExistentDrink_ShouldNotChangeCount()
        {
            int initialCount = _menuService.MenuCount;

            _menuService.RemoveDrinkByName("NonExistent");

            Assert.That(_menuService.MenuCount, Is.EqualTo(initialCount));
        }

        [Test]
        public void RemoveDrinkByName_EmptyName_ShouldReturnFalse()
        {
            string emptyName = "";

            bool result = _menuService.RemoveDrinkByName(emptyName);

            Assert.That(result, Is.False);
        }

        [Test]
        public void AddDrinkByDetails_ValidData_ShouldIncreaseMenuCount()
        {
            int countBefore = _menuService.MenuCount;
            string[] ingredients = { "кава", "цукор" };
            int[] amounts = { 10, 5 };

            _menuService.AddDrinkByDetails("CustomCoffee", 50m, ingredients, amounts);

            Assert.That(_menuService.MenuCount, Is.EqualTo(countBefore + 1));
        }

        [Test]
        public void AddDrinkByDetails_ValidData_ShouldStoreDrinkAtCorrectIndex()
        {
            int countBefore = _menuService.MenuCount;
            string[] ingredients = { "кава", "цукор" };
            int[] amounts = { 10, 5 };

            _menuService.AddDrinkByDetails("CustomCoffee", 50m, ingredients, amounts);

            Assert.That(_menuService.Menu[countBefore].Name, Is.EqualTo("CustomCoffee"));
        }

        [Test]
        public void AddDrinkByDetails_ValidData_ShouldStorePriceCorrectly()
        {
            int countBefore = _menuService.MenuCount;
            string[] ingredients = { "кава", "цукор" };
            int[] amounts = { 10, 5 };

            _menuService.AddDrinkByDetails("CustomCoffee", 75m, ingredients, amounts);

            Assert.That(_menuService.Menu[countBefore].Price, Is.EqualTo(75m));
        }
    }
}