using CafeRush.Domain;
using CafeRush.Services;
using System;

namespace CafeRush.Services
{
    public partial class OrderService
    {
        public Order[] Orders { get; set; }
        public int OrderCount { get; set; }
        public decimal Money { get; set; }
        private Stock stock;
        private MenuService menuService;
        private int customerCounter;

        public int[] ServiceRatings { get; set; }
        public int RatingCount { get; set; }

        public OrderService(Stock stockRef, MenuService menuRef)
        {
            Orders = new Order[200];
            OrderCount = 0;
            Money = 0m;
            stock = stockRef;
            menuService = menuRef;
            customerCounter = 0;

            ServiceRatings = new int[200];
            RatingCount = 0;
        }

        private void EnsureOrdersCapacity()
        {
            if (OrderCount < Orders.Length)
            {
                return;
            }

            int newSize = Orders.Length * 2;
            Order[] newArr = new Order[newSize];
            for (int i = 0; i < Orders.Length; i++)
            {
                newArr[i] = Orders[i];
            }

            Orders = newArr;
        }

        private void EnsureRatingsCapacity()
        {
            if (RatingCount < ServiceRatings.Length)
            {
                return;
            }

            int newSize = ServiceRatings.Length * 2;
            int[] newArr = new int[newSize];
            for (int i = 0; i < ServiceRatings.Length; i++)
            {
                newArr[i] = ServiceRatings[i];
            }

            ServiceRatings = newArr;
        }
    }
}
