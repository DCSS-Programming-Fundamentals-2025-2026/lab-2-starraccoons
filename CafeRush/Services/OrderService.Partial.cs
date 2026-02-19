using CafeRush.Domain;
using System;

namespace CafeRush.Services
{
    public partial class OrderService
    {
        public void CreateOrder()
        {
            menuService.ShowMenu();
            Console.WriteLine("Введіть номери позицій через пробіл:");
            string line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                return;
            }

            Order order = new Order();
            string[] parts = line.Split(' ');

            for (int i = 0; i < parts.Length; i++)
            {
                int index;
                bool ok = int.TryParse(parts[i], out index);
                if (ok && index >= 0 && index < menuService.MenuCount)
                {
                    order.AddDrink(menuService.Menu[index]);
                }
            }

            if (order.DrinksCount == 0)
            {
                Console.WriteLine("Нічого не обрано.");
                return;
            }

            if (!order.CanPrepare(stock))
            {
                Console.WriteLine("Недостатньо ресурсів.");
                return;
            }

            order.Prepare(stock);

            customerCounter++;
            decimal finalAmount = order.Total;
            if (customerCounter % 3 == 0)
            {
                decimal discount = finalAmount * 0.05m;
                finalAmount = finalAmount - discount;
                Console.WriteLine("Вітаємо! Ви кожен третій клієнт.");
                Console.WriteLine("Знижка 5% застосована.");
            }

            EnsureOrdersCapacity();
            Orders[OrderCount] = order;
            OrderCount++;
            Money = Money + finalAmount;
            PrintReceiptWithDiscount(order, finalAmount);

            AskForRating();
        }

        public void CreateCustomDrink()
        {
            Console.WriteLine("Введіть назву вашого напою:");
            string drinkName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(drinkName))
            {
                Console.WriteLine("Назва не може бути порожньою.");
                return;
            }

            int count;
            while (true)
            {
                Console.WriteLine("Скільки інгредієнтів буде використано?");
                string countInput = Console.ReadLine();
                if (int.TryParse(countInput, out count) && count > 0)
                {
                    break;
                }

                Console.WriteLine("Некоректне значення. Спробуйте ще раз.");
            }

            Order order = new Order();

            for (int i = 0; i < count; i++)
            {
                string ingName;
                int needed;

                while (true)
                {
                    Console.WriteLine("Назва інгредієнта #" + (i + 1) + ":");
                    ingName = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(ingName))
                    {
                        break;
                    }

                    Console.WriteLine("Назва не може бути порожньою. Спробуйте ще раз.");
                }

                while (true)
                {
                    Console.WriteLine("Скільки потрібно для 1 напою?");
                    string neededInput = Console.ReadLine();
                    if (int.TryParse(neededInput, out needed) && needed > 0)
                    {
                        break;
                    }

                    Console.WriteLine("Некоректне значення. Введіть число більше 0.");
                }

                if (!stock.HasExtra(ingName, needed))
                {
                    Console.WriteLine("Немає на складі " + ingName);
                    Console.WriteLine("Чи приніс клієнт цей інгредієнт? (так/ні)");
                    string answer = Console.ReadLine();
                    if (answer != null && answer.ToLower() == "так")
                    {
                        stock.Refill();
                        if (!stock.HasExtra(ingName, needed))
                        {
                            Console.WriteLine("Недостатньо для приготування напою. Замовлення скасовано.");
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Замовлення скасовано.");
                        return;
                    }
                }

                stock.Consume(ingName, needed);
            }

            Drink custom = new Drink(drinkName, 120m, 0, 0, 0, 0);
            order.AddDrink(custom);

            EnsureOrdersCapacity();
            Orders[OrderCount] = order;
            OrderCount++;
            Money = Money + 120m;

            order.PrintReceipt();

            AskForRating();
            Console.WriteLine("Бажаєте додати цей напій у меню? (так/ні)");
            string addMenu = Console.ReadLine();
            if (addMenu != null && addMenu.ToLower() == "так")
            {
                menuService.AddCustomDrinkToMenu(custom);
                Console.WriteLine("Напій додано в меню.");
            }
        }

        private void PrintReceiptWithDiscount(Order order, decimal finalAmount)
        {
            Console.WriteLine();
            Console.WriteLine("====== ЧЕК ======");
            for (int i = 0; i < order.DrinksCount; i++)
            {
                Console.WriteLine((i + 1) + ". " + order.Drinks[i].Name + " - " + order.Drinks[i].Price + " грн");
            }
            Console.WriteLine("-----------------");
            Console.WriteLine("Сума без знижки: " + order.Total + " грн");
            if (finalAmount < order.Total)
            {
                Console.WriteLine("Знижка 5%");
            }

            Console.WriteLine("До сплати: " + finalAmount + " грн");
            Console.WriteLine("=================");
        }

        private void AskForRating()
        {
            int rating;
            while (true)
            {
                Console.WriteLine("Оцініть обслуговування (1-5):");
                string input = Console.ReadLine();
                if (int.TryParse(input, out rating) && rating >= 1 && rating <= 5)
                {
                    EnsureRatingsCapacity();
                    ServiceRatings[RatingCount] = rating;
                    RatingCount++;
                    break;
                }

                Console.WriteLine("Невірне значення. Введіть число від 1 до 5.");
            }
        }

        public void RestartDay()
        {
            OrderCount = 0;
            Money = 0m;
            customerCounter = 0;
            stock = new Stock();
            RatingCount = 0;
            Console.WriteLine("Новий день розпочато.");
        }
    }
}