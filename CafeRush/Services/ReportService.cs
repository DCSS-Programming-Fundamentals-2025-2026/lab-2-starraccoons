using CafeRush.Domain;
using System;

namespace CafeRush.Services
{
    public class ReportService
    {
        private OrderService orderService;

        public ReportService(OrderService orderRef)
        {
            orderService = orderRef;
        }

        public void ShowDayReport()
        {
            Console.WriteLine("=== Звіт ===");
            Console.WriteLine("Замовлень: " + orderService.OrderCount);
            Console.WriteLine("Зароблено: " + orderService.Money + " грн");

            if (orderService.RatingCount > 0)
            {
                int[] counts = new int[6];
                int sum = 0;
                for (int i = 0; i < orderService.RatingCount; i++)
                {
                    int r = orderService.ServiceRatings[i];
                    counts[r] = counts[r] + 1;
                    sum = sum + r;
                }

                double avg = (double)sum / orderService.RatingCount;
                Console.WriteLine($"Середній рівень обслуговування: {avg:F2}");

                Console.WriteLine("Кількість оцінок:");
                for (int i = 1; i <= 5; i++)
                {
                    Console.WriteLine(i + ": " + counts[i]);
                }
            }
            else
            {
                Console.WriteLine("Оцінок обслуговування ще немає.");
            }
        }
    }
}
