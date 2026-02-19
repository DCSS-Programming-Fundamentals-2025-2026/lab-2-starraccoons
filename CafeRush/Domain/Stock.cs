using System;

namespace CafeRush.Domain
{
    public class Stock
    {
        public int CoffeeBeans { get; set; }
        public int Milk { get; set; }
        public int Cups { get; set; }
        public int Sugar { get; set; }

        public string[] ExtraNames { get; set; }
        public int[] ExtraAmounts { get; set; }
        public int ExtraCount { get; set; }

        public Stock()
        {
            CoffeeBeans = 300;
            Milk = 300;
            Cups = 150;
            Sugar = 200;

            ExtraNames = new string[50];
            ExtraAmounts = new int[50];
            ExtraCount = 0;
        }

        private IngredientType MapNameToType(string name)
        {
            name = name.ToLowerInvariant();
            if (name == "кава" || name == "зерно" || name == "зерна" || name == "coffeebeans" || name == "coffee")
            {
                return IngredientType.CoffeeBeans;
            }
            if (name == "молоко" || name == "milk")
            {
                return IngredientType.Milk;
            }
            if (name == "склянки" || name == "чашки" || name == "cups")
            {
                return IngredientType.Cups;
            }
            if (name == "цукор" || name == "sugar")
            {
                return IngredientType.Sugar;
            }
            return IngredientType.Extra;
        }

        private void EnsureExtraCapacity()
        {
            if (ExtraCount < ExtraNames.Length)
            {
                return;
            }

            int newSize = ExtraNames.Length * 2;
            string[] newNames = new string[newSize];
            int[] newAmounts = new int[newSize];

            for (int i = 0; i < ExtraNames.Length; i++)
            {
                newNames[i] = ExtraNames[i];
                newAmounts[i] = ExtraAmounts[i];
            }

            ExtraNames = newNames;
            ExtraAmounts = newAmounts;
        }

        public void AddExtra(string name, int amount)
        {
            for (int i = 0; i < ExtraCount; i++)
            {
                if (ExtraNames[i] == name)
                {
                    ExtraAmounts[i] = ExtraAmounts[i] + amount;
                    return;
                }
            }

            EnsureExtraCapacity();

            ExtraNames[ExtraCount] = name;
            ExtraAmounts[ExtraCount] = amount;
            ExtraCount++;
        }

        public bool HasExtra(string name, int needed)
        {
            IngredientType type = MapNameToType(name);
            if (type == IngredientType.CoffeeBeans)
            {
                return CoffeeBeans >= needed;
            }
            if (type == IngredientType.Milk)
            {
                return Milk >= needed;
            }
            if (type == IngredientType.Cups)
            {
                return Cups >= needed;
            }
            if (type == IngredientType.Sugar)
            {
                return Sugar >= needed;
            }

            for (int i = 0; i < ExtraCount; i++)
            {
                if (ExtraNames[i] == name && ExtraAmounts[i] >= needed)
                {
                    return true;
                }
            }
            return false;
        }

        public void Consume(string name, int amount)
        {
            IngredientType type = MapNameToType(name);
            if (type == IngredientType.CoffeeBeans)
            {
                CoffeeBeans = CoffeeBeans - amount;
                return;
            }
            if (type == IngredientType.Milk)
            {
                Milk = Milk - amount;
                return;
            }
            if (type == IngredientType.Cups)
            {
                Cups = Cups - amount;
                return;
            }
            if (type == IngredientType.Sugar)
            {
                Sugar = Sugar - amount;
                return;
            }

            for (int i = 0; i < ExtraCount; i++)
            {
                if (ExtraNames[i] == name)
                {
                    ExtraAmounts[i] = ExtraAmounts[i] - amount;
                    return;
                }
            }
        }

        public void Refill()
        {
            while (true)
            {
                Console.WriteLine("Введіть назву ресурсу (наприклад: Кава, Молоко, Цукор, Склянки або новий інгредієнт):");
                string name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Назва не може бути порожньою.");
                    continue;
                }

                Console.WriteLine("Введіть кількість:");
                string text = Console.ReadLine();
                int amount;
                if (!int.TryParse(text, out amount) || amount <= 0)
                {
                    Console.WriteLine("Невірна кількість. Спробуйте ще раз.");
                    continue;
                }

                IngredientType type = MapNameToType(name);
                if (type == IngredientType.CoffeeBeans)
                {
                    CoffeeBeans = CoffeeBeans + amount;
                }
                else if (type == IngredientType.Milk)
                {
                    Milk = Milk + amount;
                }
                else if (type == IngredientType.Cups)
                {
                    Cups = Cups + amount;
                }
                else if (type == IngredientType.Sugar)
                {
                    Sugar = Sugar + amount;
                }
                else
                {
                    AddExtra(name, amount);
                }

                Console.WriteLine("Склад поповнено.");
                break;
            }
        }

        public void Show()
        {
            Console.WriteLine("=== Склад ===");
            Console.WriteLine("Кава: " + CoffeeBeans);
            Console.WriteLine("Молоко: " + Milk);
            Console.WriteLine("Склянки: " + Cups);
            Console.WriteLine("Цукор: " + Sugar);

            if (ExtraCount > 0)
            {
                Console.WriteLine("--- Додаткові інгредієнти ---");
                for (int i = 0; i < ExtraCount; i++)
                {
                    Console.WriteLine(ExtraNames[i] + ": " + ExtraAmounts[i]);
                }
            }
            Console.WriteLine("=============");
        }
    }
}