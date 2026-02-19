using System;
using CafeRush.Contracts;

namespace CafeRush.Domain
{
    public class Drink : MenuItemBase, IRecipe
    {
        public int BeansNeeded { get; set; }
        public int MilkNeeded { get; set; }
        public int CupsNeeded { get; set; }
        public int SugarNeeded { get; set; }

        public string[] ExtraIngredientNames { get; set; }
        public int[] ExtraIngredientAmounts { get; set; }
        public int ExtraIngredientCount { get; set; }

        public Drink()
        {
            ExtraIngredientNames = new string[10];
            ExtraIngredientAmounts = new int[10];
            ExtraIngredientCount = 0;
        }

        public Drink(string name, decimal price, int beans, int milk, int cups, int sugar) : this()
        {
            Name = name;
            Price = price;
            BeansNeeded = beans;
            MilkNeeded = milk;
            CupsNeeded = cups;
            SugarNeeded = sugar;
        }

        public void AddExtraIngredient(string name, int amount)
        {
            if (ExtraIngredientCount >= ExtraIngredientNames.Length)
            {
                int newSize = ExtraIngredientNames.Length * 2;
                string[] newNames = new string[newSize];
                int[] newAmounts = new int[newSize];

                for (int i = 0; i < ExtraIngredientNames.Length; i++)
                {
                    newNames[i] = ExtraIngredientNames[i];
                    newAmounts[i] = ExtraIngredientAmounts[i];
                }

                ExtraIngredientNames = newNames;
                ExtraIngredientAmounts = newAmounts;
            }

            ExtraIngredientNames[ExtraIngredientCount] = name;
            ExtraIngredientAmounts[ExtraIngredientCount] = amount;
            ExtraIngredientCount++;
        }

        public bool CanMake(Stock stock)
        {
            if (stock.CoffeeBeans < BeansNeeded)
            {
                return false;
            }
            if (stock.Milk < MilkNeeded)
            {
                return false;
            }
            if (stock.Cups < CupsNeeded)
            {
                return false;
            }
            if (stock.Sugar < SugarNeeded)
            {
                return false;
            }

            for (int i = 0; i < ExtraIngredientCount; i++)
            {
                string name = ExtraIngredientNames[i];
                int needed = ExtraIngredientAmounts[i];
                if (!stock.HasExtra(name, needed))
                {
                    return false;
                }
            }

            return true;
        }

        public void Consume(Stock stock)
        {
            stock.CoffeeBeans = stock.CoffeeBeans - BeansNeeded;
            stock.Milk = stock.Milk - MilkNeeded;
            stock.Cups = stock.Cups - CupsNeeded;
            stock.Sugar = stock.Sugar - SugarNeeded;

            for (int i = 0; i < ExtraIngredientCount; i++)
            {
                string name = ExtraIngredientNames[i];
                int amount = ExtraIngredientAmounts[i];
                stock.Consume(name, amount);
            }
        }
    }
}