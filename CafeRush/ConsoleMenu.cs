using CafeRush.Domain;
using CafeRush.Services;
using System;

namespace CafeRush
{
    public class ConsoleMenu
    {
        private Cafe cafe;

        public ConsoleMenu(Cafe cafeRef)
        {
            cafe = cafeRef;
        }

        public void Start()
        {
            bool run = true;
            while (run)
            {
                ShowMenu();
                string input = Console.ReadLine();

                if (input == "1")
                {
                    cafe.OrderService.CreateOrder();
                }
                else if (input == "2")
                {
                    cafe.Stock.Show();
                }
                else if (input == "3")
                {
                    cafe.ReportService.ShowDayReport();
                }
                else if (input == "4")
                {
                    cafe.MenuService.ShowMenu();
                }
                else if (input == "5")
                {
                    cafe.OrderService.CreateCustomDrink();
                }
                else if (input == "6")
                {
                    cafe.Stock.Refill();
                }
                else if (input == "7")
                {
                    cafe.OrderService.RestartDay();
                }
                else if (input == "8")
                {
                    Console.WriteLine("Введіть назву напою для додавання у меню:");
                    string name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        Console.WriteLine("Назва не може бути порожньою.");
                    }
                    else
                    {
                        Console.WriteLine("Введіть ціну:");
                        string p = Console.ReadLine();
                        decimal price;
                        if (!decimal.TryParse(p, out price) || price <= 0m)
                        {
                            Console.WriteLine("Невірна ціна.");
                        }
                        else
                        {
                            Console.WriteLine("Скільки інгредієнтів буде у напої?");
                            string c = Console.ReadLine();
                            int count;
                            if (!int.TryParse(c, out count) || count <= 0)
                            {
                                Console.WriteLine("Невірна кількість інгредієнтів.");
                            }
                            else
                            {
                                string[] ingNames = new string[count];
                                int[] ingAmounts = new int[count];
                                for (int i = 0; i < count; i++)
                                {
                                    Console.WriteLine($"Назва інгредієнта #{i + 1}:");
                                    ingNames[i] = Console.ReadLine();
                                    Console.WriteLine($"Кількість для 1 напою:");
                                    string ai = Console.ReadLine();
                                    int aiVal;
                                    if (!int.TryParse(ai, out aiVal) || aiVal <= 0)
                                    {
                                        Console.WriteLine("Невірна кількість, використано 1.");
                                        aiVal = 1;
                                    }

                                    ingAmounts[i] = aiVal;
                                }

                                try
                                {
                                    cafe.MenuService.AddDrinkByDetails(name, price, ingNames, ingAmounts);
                                    Console.WriteLine("Напій додано у меню.");
                                }
                                catch (System.Exception ex)
                                {
                                    Console.WriteLine("Помилка при додаванні напою: " + ex.Message);
                                }
                            }
                        }
                    }
                }
                else if (input == "9")
                {
                    Console.WriteLine("Введіть назву напою для видалення:");
                    string name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        Console.WriteLine("Назва не може бути порожньою.");
                    }
                    else
                    {
                        bool ok = cafe.MenuService.RemoveDrinkByName(name);
                        if (ok)
                        {
                            Console.WriteLine("Напій видалено з меню.");
                        }
                        else
                        {
                            Console.WriteLine("Напій не знайдено.");
                        }
                    }
                }
                else if (input == "0")
                {
                    run = false;
                }
                else
                {
                    Console.WriteLine("Невідомий вибір.");
                }
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("=== Cafe Rush ===");
            Console.WriteLine("1 - Створити замовлення");
            Console.WriteLine("2 - Показати склад");
            Console.WriteLine("3 - Звіт за день");
            Console.WriteLine("4 - Показати меню напоїв");
            Console.WriteLine("5 - Створити власний напій");
            Console.WriteLine("6 - Поповнити склад");
            Console.WriteLine("7 - Перезапуск дня");
            Console.WriteLine("8 - Додати напій у меню (інтерактивно)");
            Console.WriteLine("9 - Видалити напій з меню");
            Console.WriteLine("0 - Вихід");
            Console.Write("Ваш вибір: ");
        }
    }
}
