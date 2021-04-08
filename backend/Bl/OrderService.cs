using System;
using System.Collections.Generic;
using System.Linq;
using Dal.FileLoader;
using Entities.Models;
using Entities.ViewModels;

namespace Bl
{
    public class OrderService
    {
           
        private static IEnumerable<UserCost> GetTotalCostForEachUser(List<Price> prices, List<Order> orders)
        {
            var userCost = new List<UserCost>();
            foreach (var order in orders)
            {
                var userPrice = new UserCost();
                var drink = prices.First(x => x.DrinkName == order.Drink);
                var drinkSize = typeof(Size).GetProperties().First(prop => prop.Name.Equals(order.Size, StringComparison.InvariantCultureIgnoreCase));
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

        private static List<UserCost> GetTotalPaymentForEachUser(List<Payment> payments)
        {
            var totalPaymentForEachUser = payments.GroupBy(u => u.User)
                .Select(u => new UserCost
                {
                    Name = u.First().User,
                    Cost = u.Sum(c => c.Amount)
                }).ToList();
            return totalPaymentForEachUser;
        }

        private static IEnumerable<OrderPayment> GetResultForEachUser(List<Price> prices, List<Order> orders, List<Payment> payments)
        {
            var expectedResults = new List<OrderPayment>();
            var totalCostPerUser = GetTotalCostForEachUser(prices, orders);
            var totalPaymentPerUser = GetTotalPaymentForEachUser(payments);
            foreach (var user in totalCostPerUser)
            {
                var result = new OrderPayment
                {
                    User = user.Name,
                    OrderTotal = user.Cost,
                    PaymentTotal = totalPaymentPerUser.Any(u => u.Name == user.Name) ? totalPaymentPerUser.First(u => u.Name == user.Name).Cost : 0
                };
                result.Balance = result.OrderTotal - result.PaymentTotal;
                expectedResults.Add(result);
            }
            return expectedResults;
        }

        public static string GetResult(string priceJson, string orderJson, string paymentJson)
        {
            var price = LoadFiles.LoadData<Price>(priceJson);
            var orders = LoadFiles.LoadData<Order>(orderJson);
            var payments = LoadFiles.LoadData<Payment>(paymentJson);
            var result =  GetResultForEachUser(price, orders, payments);
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }
    }
}
