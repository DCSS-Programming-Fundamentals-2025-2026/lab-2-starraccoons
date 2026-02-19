using System;

namespace CafeRush.Domain
{
    public class Order
    {
        public Drink[] Drinks { get; set; }
        public int DrinksCount { get; set; }
        public decimal Total { get; set; }
        public string Id { get; set; }

        public Order()
        {
            Drinks = new Drink[20];
            DrinksCount = 0;
            Total = 0m;
            Id = Guid.NewGuid().ToString();
        }

        private void EnsureDrinksCapacity()
        {
            if (DrinksCount < Drinks.Length)
            {
                return;
            }

            int newSize = Drinks.Length * 2;
            Drink[] newArr = new Drink[newSize];
            for (int i = 0; i < Drinks.Length; i++)
            {
                newArr[i] = Drinks[i];
            }

            Drinks = newArr;
        }

        public void AddDrink(Drink d)
        {
            EnsureDrinksCapacity();
            Drinks[DrinksCount] = d;
            DrinksCount = DrinksCount + 1;
            Total = Total + d.Price;
        }

        private void CollectExtras(
            out int needBeans, out int needMilk, out int needCups, out int needSugar,
            out string[] tempExtraNames, out int[] tempExtraAmounts, out int tempExtraCount)
        {
            needBeans = 0;
            needMilk = 0;
            needCups = 0;
            needSugar = 0;

            tempExtraNames = new string[50];
            tempExtraAmounts = new int[50];
            tempExtraCount = 0;

            for (int i = 0; i < DrinksCount; i++)
            {
                needBeans = needBeans + Drinks[i].BeansNeeded;
                needMilk = needMilk + Drinks[i].MilkNeeded;
                needCups = needCups + Drinks[i].CupsNeeded;
                needSugar = needSugar + Drinks[i].SugarNeeded;

                for (int e = 0; e < Drinks[i].ExtraIngredientCount; e++)
                {
                    string ename = Drinks[i].ExtraIngredientNames[e];
                    int eamount = Drinks[i].ExtraIngredientAmounts[e];

                    bool found = false;
                    for (int t = 0; t < tempExtraCount; t++)
                    {
                        if (tempExtraNames[t] == ename)
                        {
                            tempExtraAmounts[t] = tempExtraAmounts[t] + eamount;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        if (tempExtraCount >= tempExtraNames.Length)
                        {
                            int newSize = tempExtraNames.Length * 2;
                            string[] nn = new string[newSize];
                            int[] na = new int[newSize];
                            for (int k = 0; k < tempExtraNames.Length; k++)
                            {
                                nn[k] = tempExtraNames[k];
                                na[k] = tempExtraAmounts[k];
                            }
                            tempExtraNames = nn;
                            tempExtraAmounts = na;
                        }

                        tempExtraNames[tempExtraCount] = ename;
                        tempExtraAmounts[tempExtraCount] = eamount;
                        tempExtraCount++;
                    }
                }
            }
        }

        public bool CanPrepare(Stock stock)
        {
            int needBeans;
            int needMilk;
            int needCups;
            int needSugar;
            string[] tempExtraNames;
            int[] tempExtraAmounts;
            int tempExtraCount;

            CollectExtras(
                out needBeans, out needMilk, out needCups, out needSugar,
                out tempExtraNames, out tempExtraAmounts, out tempExtraCount);

            if (stock.CoffeeBeans < needBeans)
            {
                return false;
            }
            if (stock.Milk < needMilk)
            {
                return false;
            }
            if (stock.Cups < needCups)
            {
                return false;
            }
            if (stock.Sugar < needSugar)
            {
                return false;
            }

            for (int t = 0; t < tempExtraCount; t++)
            {
                if (!stock.HasExtra(tempExtraNames[t], tempExtraAmounts[t]))
                {
                    return false;
                }
            }

            return true;
        }

        public void Prepare(Stock stock)
        {
            int needBeans;
            int needMilk;
            int needCups;
            int needSugar;
            string[] tempExtraNames;
            int[] tempExtraAmounts;
            int tempExtraCount;

            CollectExtras(
                out needBeans, out needMilk, out needCups, out needSugar,
                out tempExtraNames, out tempExtraAmounts, out tempExtraCount);

            stock.CoffeeBeans = stock.CoffeeBeans - needBeans;
            stock.Milk = stock.Milk - needMilk;
            stock.Cups = stock.Cups - needCups;
            stock.Sugar = stock.Sugar - needSugar;

            for (int t = 0; t < tempExtraCount; t++)
            {
                stock.Consume(tempExtraNames[t], tempExtraAmounts[t]);
            }
        }

        public void PrintReceipt()
        {
            Console.WriteLine();
            Console.WriteLine("====== ЧЕК ======");
            Console.WriteLine("Замовлення Id: " + Id);
            Console.WriteLine("Позиції:");

            for (int i = 0; i < DrinksCount; i++)
            {
                Console.WriteLine((i + 1) + ". " + Drinks[i].Name + " - " + Drinks[i].Price + " грн");
            }

            Console.WriteLine("-----------------");
            Console.WriteLine("Всього: " + Total + " грн");
            Console.WriteLine("=================");
            Console.WriteLine();
        }
    }
}