using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using CodingChallenge.Model;
using System.Linq;

namespace CodingChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            GetResultForEachUser();
            //Console.WriteLine(LoadPrices());
            GetTotalPaymentForEachUser();
            GetPricesForUser();
            Console.ReadLine();

        }
        private static List<Price> LoadPrices()
        {
            List<Price> prices;
            string path = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string actualPath = path.Substring(0, path.LastIndexOf("bin"));
            string projectPath = new Uri(actualPath).LocalPath;
            string filePath = projectPath + @"Data\prices.json";
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                prices = JsonConvert.DeserializeObject<List<Price>>(json);
            }
            return prices;
        }

        private static List<Order> LoadOrders()
        {
            List<Order> orders;
            string path = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string actualPath = path.Substring(0, path.LastIndexOf("bin"));
            string projectPath = new Uri(actualPath).LocalPath;
            string filePath = projectPath + @"Data\orders.json";
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                orders = JsonConvert.DeserializeObject<List<Order>>(json);
            }
            return orders;
        }

        private static List<Payment> LoadPayments()
        {
            List<Payment> payments;
            string path = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string actualPath = path.Substring(0, path.LastIndexOf("bin"));
            string projectPath = new Uri(actualPath).LocalPath;
            string filePath = projectPath + @"Data\payments.json";
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                payments = JsonConvert.DeserializeObject<List<Payment>>(json);
            }
            return payments;
        }
        public class UserCost
        {
            public string Name { get; set; }
            public decimal Cost { get; set; }

        }
        private static IEnumerable<UserCost> GetPricesForUser()
        {
            var userCost = new List<UserCost>();
            var prices = LoadPrices();
            var orders = LoadOrders();
            foreach (var order in orders)
            {
                var userPrice = new UserCost();
                var drink = prices.FirstOrDefault(x => x.DrinkName == order.Drink);
                var drinkSize = typeof(Size).GetProperties().FirstOrDefault(prop => prop.Name.Equals(order.Size, StringComparison.InvariantCultureIgnoreCase));
                userPrice.Name = order.User;
                userPrice.Cost = (decimal)drinkSize.GetValue(drink.Prices);
                userCost.Add(userPrice);
            }
            var finalUserCosts = userCost.GroupBy(u => u.Name)
                .Select(u => new UserCost
                {
                    Name = u.First().Name,
                    Cost = u.Sum(c => c.Cost)
                });
            return finalUserCosts;
        }

        private static IEnumerable<UserCost> GetTotalPaymentForEachUser()
        {
            var payments = LoadPayments();
            var totalPaymentForEachUser = payments.GroupBy(u => u.User)
                .Select(u => new UserCost
                {
                    Name = u.First().User,
                    Cost = u.Sum(c => c.Amount)
                });
            return totalPaymentForEachUser;
        }

        public class ExpectedResult
        {
            public string User { get; set; }
            public decimal OrderTotal { get; set; }
            public decimal PaymentTotal { get; set; }
            public decimal Balance { get; set; }
        }
        private static IEnumerable<ExpectedResult> GetResultForEachUser()
        {
            var expectedResults = new List<ExpectedResult>();
            var totalCostUser = GetPricesForUser();
            var totalPaymentCostUser = GetTotalPaymentForEachUser();
            foreach(var user in totalCostUser)
            {
                var expectedResult = new ExpectedResult();
                expectedResult.User = user.Name;
                expectedResult.OrderTotal = user.Cost;
                expectedResult.PaymentTotal = totalPaymentCostUser.FirstOrDefault(u => u.Name == user.Name).Cost;
                expectedResult.Balance =  expectedResult.PaymentTotal - expectedResult.OrderTotal;
                expectedResults.Add(expectedResult);
            }
            return expectedResults;
        }
    }
}
