using System;
using System.IO;
using Bl;
using Dal.Helpers;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello There! This is Mudbath Coding Challange!");
            var prices = FileReader.LoadJsonFile("Data\\prices.json");
            var orders = FileReader.LoadJsonFile("Data\\orders.json");
            var payments = FileReader.LoadJsonFile("Data\\payments.json");
            Console.WriteLine(OrderService.GetResult(prices, orders, payments));
            Console.ReadLine();
        }
    }
}
