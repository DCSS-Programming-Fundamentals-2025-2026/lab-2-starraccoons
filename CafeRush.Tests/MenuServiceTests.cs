using CafeRush.Domain;
using CafeRush.Exceptions;
using CafeRush.Services;
using NUnit.Framework;
using System;

namespace CafeRush.Tests
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

        // ==========================================
        // UNIT TESTS: MenuService - Видалення
        // ==========================================

        [Test]
        public void RemoveDrinkByName_ExistingDrink_ShouldReturnTrue()
        {
            // Arrange
            string drinkToRemove = "Espresso";

            // Act
            bool result = _menuService.RemoveDrinkByName(drinkToRemove);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void RemoveDrinkByName_ExistingDrink_ShouldDecrementCount()
        {
            // Arrange
            int initialCount = _menuService.MenuCount;

            // Act
            _menuService.RemoveDrinkByName("Espresso");

            // Assert
            Assert.That(_menuService.MenuCount, Is.EqualTo(initialCount - 1));
        }

        [Test]
        public void RemoveDrinkByName_ExistingDrink_ShouldShiftArrayLeft()
        {
            // Arrange
            // За замовчуванням: індекс 0 = Espresso, індекс 1 = Latte

            // Act
            _menuService.RemoveDrinkByName("Espresso");

            // Assert
            // Після видалення Espresso, Latte має зайняти індекс 0
            Assert.That(_menuService.Menu[0].Name, Is.EqualTo("Latte"));
        }

        [Test]
        public void RemoveDrinkByName_ExistingDrink_LastSlotShouldBeNull()
        {
            // Arrange
            int countBefore = _menuService.MenuCount;

            // Act
            _menuService.RemoveDrinkByName("Espresso");

            // Assert
            // Останній слот після зсуву має бути null
            Assert.That(_menuService.Menu[countBefore - 1], Is.Null);
        }

        [Test]
        public void RemoveDrinkByName_NonExistentDrink_ShouldReturnFalse()
        {
            // Arrange
            string fakeDrink = "NonExistent";

            // Act
            bool result = _menuService.RemoveDrinkByName(fakeDrink);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void RemoveDrinkByName_NonExistentDrink_ShouldNotChangeCount()
        {
            // Arrange
            int initialCount = _menuService.MenuCount;

            // Act
            _menuService.RemoveDrinkByName("NonExistent");

            // Assert
            Assert.That(_menuService.MenuCount, Is.EqualTo(initialCount));
        }

        [Test]
        public void RemoveDrinkByName_EmptyName_ShouldReturnFalse()
        {
            // Arrange
            string emptyName = "";

            // Act
            bool result = _menuService.RemoveDrinkByName(emptyName);

            // Assert
            Assert.That(result, Is.False);
        }

        // ==========================================
        // UNIT TESTS: MenuService - Додавання
        // ==========================================

        [Test]
        public void AddDrinkByDetails_ValidData_ShouldIncreaseMenuCount()
        {
            // Arrange
            int countBefore = _menuService.MenuCount;
            string[] ingredients = { "кава", "цукор" };
            int[] amounts = { 10, 5 };

            // Act
            _menuService.AddDrinkByDetails("CustomCoffee", 50m, ingredients, amounts);

            // Assert
            Assert.That(_menuService.MenuCount, Is.EqualTo(countBefore + 1));
        }

        [Test]
        public void AddDrinkByDetails_ValidData_ShouldStoreDrinkAtCorrectIndex()
        {
            // Arrange
            int countBefore = _menuService.MenuCount;
            string[] ingredients = { "кава", "цукор" };
            int[] amounts = { 10, 5 };

            // Act
            _menuService.AddDrinkByDetails("CustomCoffee", 50m, ingredients, amounts);

            // Assert
            Assert.That(_menuService.Menu[countBefore].Name, Is.EqualTo("CustomCoffee"));
        }

        [Test]
        public void AddDrinkByDetails_InvalidArrayLengths_ShouldThrowInvalidIngredientException()
        {
            // Arrange
            string[] names = { "Cup" };
            int[] amounts = { 1, 2 }; // довжина не збігається

            // Act & Assert
            Assert.Throws<InvalidIngredientException>(() =>
            {
                _menuService.AddDrinkByDetails("Water", 10m, names, amounts);
            });
        }
    }
}