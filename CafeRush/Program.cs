using System;
using System.Text;

namespace CafeRush
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Cafe cafe = new Cafe();

            ConsoleMenu consoleMenu = new ConsoleMenu(cafe);
            consoleMenu.Start();
        }
    }
}
