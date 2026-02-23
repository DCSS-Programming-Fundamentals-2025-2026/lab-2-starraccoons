using CafeRush.Domain;

namespace CafeRush.Tests.Domain
{
    [TestFixture]
    public class DrinkTests
    {
        [Test]
        public void AddExtraIngredient_BeyondInitialCapacity_CountShouldBeEleven()
        {
            Drink drink = new Drink("TestDrink", 50m, 0, 0, 1, 0);

            drink.AddExtraIngredient("Ing0", 1);
            drink.AddExtraIngredient("Ing1", 2);
            drink.AddExtraIngredient("Ing2", 3);
            drink.AddExtraIngredient("Ing3", 4);
            drink.AddExtraIngredient("Ing4", 5);
            drink.AddExtraIngredient("Ing5", 6);
            drink.AddExtraIngredient("Ing6", 7);
            drink.AddExtraIngredient("Ing7", 8);
            drink.AddExtraIngredient("Ing8", 9);
            drink.AddExtraIngredient("Ing9", 10);
            drink.AddExtraIngredient("Ing10", 11);

            Assert.That(drink.ExtraIngredientCount, Is.EqualTo(11));
        }

        [Test]
        public void AddExtraIngredient_BeyondInitialCapacity_ArrayLengthShouldGrowToTwenty()
        {
            Drink drink = new Drink("TestDrink", 50m, 0, 0, 1, 0);

            drink.AddExtraIngredient("Ing0", 1);
            drink.AddExtraIngredient("Ing1", 2);
            drink.AddExtraIngredient("Ing2", 3);
            drink.AddExtraIngredient("Ing3", 4);
            drink.AddExtraIngredient("Ing4", 5);
            drink.AddExtraIngredient("Ing5", 6);
            drink.AddExtraIngredient("Ing6", 7);
            drink.AddExtraIngredient("Ing7", 8);
            drink.AddExtraIngredient("Ing8", 9);
            drink.AddExtraIngredient("Ing9", 10);
            drink.AddExtraIngredient("Ing10", 11);

            Assert.That(drink.ExtraIngredientNames.Length, Is.EqualTo(20));
        }
    }
}