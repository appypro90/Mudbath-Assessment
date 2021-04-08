using System;
using System.IO;
using Bl;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello There! This is Mudbath Coding Challange!");
            var prices = LoadJsonFile("Data\\prices.json");
            var orders = LoadJsonFile("Data\\orders.json");
            var payments = LoadJsonFile("Data\\payments.json");
            Console.WriteLine(OrderService.GetResult(prices, orders, payments));
            Console.ReadLine();
        }

        public static string LoadJsonFile(string fileName)
        {
            var path = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            var actualPath = path.Substring(0, path.LastIndexOf("bin", StringComparison.Ordinal));
            var projectPath = new Uri(actualPath).LocalPath;
            var filePath = projectPath + @fileName;
            string json;
            json = ReadJsonFile(filePath);
            return json;
        }

        public static string ReadJsonFile(string fileName)
        {
            string json;
            using (var r = new StreamReader(fileName))
            {
                json = r.ReadToEnd();
            }
            return json;
        }
    }
}
