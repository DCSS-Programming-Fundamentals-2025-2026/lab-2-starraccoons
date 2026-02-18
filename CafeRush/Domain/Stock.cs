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

        private string MapName(string name)
        {
            name = name.ToLowerInvariant();
            switch (name)
            {
                case "кава":
                case "зерно":
                case "зерна":
                case "coffeebeans":
                case "coffee":
                    return "CoffeeBeans";
                case "молоко":
                case "milk":
                    return "Milk";
                case "склянки":
                case "чашки":
                case "cups":
                    return "Cups";
                case "цукор":
                case "sugar":
                    return "Sugar";
                default:
                    return name;
            }
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
            name = MapName(name);
            switch (name)
            {
                case "CoffeeBeans":
                    return CoffeeBeans >= needed;
                case "Milk":
                    return Milk >= needed;
                case "Cups":
                    return Cups >= needed;
                case "Sugar":
                    return Sugar >= needed;
                default:
                    for (int i = 0; i < ExtraCount; i++)
                    {
                        if (ExtraNames[i] == name && ExtraAmounts[i] >= needed)
                        {
                            return true;
                        }
                    }

                    return false;
            }
        }

        public void Consume(string name, int amount)
        {
            name = MapName(name);
            switch (name)
            {
                case "CoffeeBeans":
                    CoffeeBeans = CoffeeBeans - amount;
                    break;
                case "Milk":
                    Milk = Milk - amount;
                    break;
                case "Cups":
                    Cups = Cups - amount;
                    break;
                case "Sugar":
                    Sugar = Sugar - amount;
                    break;
                default:
                    for (int i = 0; i < ExtraCount; i++)
                    {
                        if (ExtraNames[i] == name)
                        {
                            ExtraAmounts[i] = ExtraAmounts[i] - amount;
                            return;
                        }
                    }

                    break;
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

                name = MapName(name);

                if (name == "CoffeeBeans")
                {
                    CoffeeBeans = CoffeeBeans + amount;
                }
                else if (name == "Milk")
                {
                    Milk = Milk + amount;
                }
                else if (name == "Cups")
                {
                    Cups = Cups + amount;
                }
                else if (name == "Sugar")
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
