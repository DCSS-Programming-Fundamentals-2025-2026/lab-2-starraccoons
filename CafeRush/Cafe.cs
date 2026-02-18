using CafeRush.Domain;
using CafeRush.Services;

namespace CafeRush
{
    public class Cafe
    {
        public Stock Stock { get; set; }
        public MenuService MenuService { get; set; }
        public OrderService OrderService { get; set; }
        public ReportService ReportService { get; set; }

        public Cafe()
        {
            Stock = new Stock();

            MenuService = new MenuService();
            OrderService = new OrderService(Stock, MenuService);
            ReportService = new ReportService(OrderService);
        }
    }
}
