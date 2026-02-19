using System;
using System.Collections;
using CafeRush.Domain;

namespace CafeRush.Comparers
{
    public class DrinkNameComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            Drink drinkX = x as Drink;
            Drink drinkY = y as Drink;

            if (drinkX == null && drinkY == null)
            {
                return 0;
            }
            if (drinkX == null)
            {
                return -1;
            }
            
            if (drinkY == null)
            {
                return -1;
            }

            return string.Compare(drinkX.Name, drinkY.Name, StringComparison.OrdinalIgnoreCase);
        }
    }
}