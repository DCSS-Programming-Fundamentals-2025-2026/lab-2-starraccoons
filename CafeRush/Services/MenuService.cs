using CafeRush.Domain;
using CafeRush.Exceptions;
using System;

namespace CafeRush.Services
{
    public class MenuService
    {
        public Drink[] Menu { get; set; }
        public int MenuCount { get; set; }

        public MenuService()
        {
            Menu = new Drink[20];
            MenuCount = 0;
            InitializeMenu();
        }

        private void EnsureMenuCapacity()
        {
            if (MenuCount < Menu.Length)
            {
                return;
            }

            int newSize = Menu.Length * 2;
            Drink[] newMenu = new Drink[newSize];
            for (int i = 0; i < Menu.Length; i++)
            {
                newMenu[i] = Menu[i];
            }

            Menu = newMenu;
        }

        private void InitializeMenu()
        {
            Menu[0] = new Drink("Espresso", 40m, 10, 0, 1, 0);
            Menu[1] = new Drink("Latte", 60m, 8, 10, 1, 2);
            Menu[2] = new Drink("Cappuccino", 55m, 9, 8, 1, 2);
            Menu[3] = new Drink("Americano", 45m, 10, 0, 1, 1);
            Menu[4] = new Drink("Mocha", 65m, 8, 10, 1, 3);
            Menu[5] = new Drink("FlatWhite", 60m, 9, 9, 1, 2);

            MenuCount = 6;
        }

        public void ShowMenu()
        {
            Console.WriteLine("=== Меню ===");
            for (int i = 0; i < MenuCount; i++)
            {
                Console.WriteLine(i + " - " + Menu[i].Name + " : " + Menu[i].Price + " грн");
            }
        }

        public void AddCustomDrinkToMenu(Drink drink)
        {
            EnsureMenuCapacity();
            Menu[MenuCount] = drink;
            MenuCount++;
        }

        private IngredientType MapIngredientName(string name)
        {
            string lower = name.ToLowerInvariant();
            if (lower == "кава" || lower == "зерно" || lower == "coffee" || lower == "coffeebeans" || lower == "beans")
            {
                return IngredientType.CoffeeBeans;
            }
            if (lower == "молоко" || lower == "milk")
            {
                return IngredientType.Milk;
            }
            if (lower == "склянки" || lower == "чашки" || lower == "cups")
            {
                return IngredientType.Cups;
            }
            if (lower == "цукор" || lower == "sugar")
            {
                return IngredientType.Sugar;
            }
            return IngredientType.Extra;
        }

        public void AddDrinkByDetails(string name, decimal price, string[] ingredientNames, int[] ingredientAmounts)
        {
            if (ingredientNames == null || ingredientAmounts == null || ingredientNames.Length != ingredientAmounts.Length)
            {
                throw new InvalidIngredientException("ingredient arrays invalid");
            }

            Drink d = new Drink(name, price, 0, 0, 0, 0);
            for (int i = 0; i < ingredientNames.Length; i++)
            {
                string ing = ingredientNames[i];
                int amt = ingredientAmounts[i];
                if (string.IsNullOrWhiteSpace(ing) || amt <= 0)
                {
                    throw new InvalidIngredientException("Invalid ingredient entry");
                }

                IngredientType type = MapIngredientName(ing);
                if (type == IngredientType.CoffeeBeans)
                {
                    d.BeansNeeded = d.BeansNeeded + amt;
                }
                else if (type == IngredientType.Milk)
                {
                    d.MilkNeeded = d.MilkNeeded + amt;
                }
                else if (type == IngredientType.Cups)
                {
                    d.CupsNeeded = d.CupsNeeded + amt;
                }
                else if (type == IngredientType.Sugar)
                {
                    d.SugarNeeded = d.SugarNeeded + amt;
                }
                else
                {
                    d.AddExtraIngredient(ing, amt);
                }
            }

            AddCustomDrinkToMenu(d);
        }

        public bool RemoveDrinkByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            int idx = -1;
            for (int i = 0; i < MenuCount; i++)
            {
                if (Menu[i].Name == name)
                {
                    idx = i;
                    break;
                }
            }

            if (idx == -1)
            {
                return false;
            }

            for (int j = idx; j < MenuCount - 1; j++)
            {
                Menu[j] = Menu[j + 1];
            }

            Menu[MenuCount - 1] = null;
            MenuCount--;
            return true;
        }
    }
}