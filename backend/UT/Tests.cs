using System.Collections.Generic;
using Bl;
using Entities.ViewModels;
using Xunit;

namespace UT
{
    public class Tests
    {
        //Example Prices JSON test data that may be used
        private string pricesJson = " [{ 'drink_name': 'short espresso', 'prices': { 'small': 3.03 }}," +
                                     "{ 'drink_name': 'latte', 'prices': { 'small': 3.50, 'medium': 4.00, 'large': 4.50 } }," +
                                     "{ 'drink_name': 'flat white', 'prices': { 'small': 3.50, 'medium': 4.00, 'large': 4.50 } }," +
                                     "{ 'drink_name': 'long black', 'prices': { 'small': 3.25, 'medium': 3.50 } }," +
                                     "{ 'drink_name': 'mocha', 'prices': { 'small': 4.00, 'medium': 4.50, 'large': 5.00 } }," +
                                     "{ 'drink_name': 'supermochacrapucaramelcream', 'prices': { 'large': 5.00, 'huge': 5.50, 'mega': 6.00, 'ultra': 7.00 } }]";

        ////Example Orders JSON test data 
        private string ordersJson = "[{ 'user': 'coach', 'drink': 'long black', 'size': 'medium' }," +
                         "{ 'user': 'ellis', 'drink': 'long black', 'size': 'small' }," +
                         "{ 'user': 'rochelle', 'drink': 'flat white', 'size': 'large' }," +
                         "{ 'user': 'coach', 'drink': 'flat white', 'size': 'large' }," +
                         "{ 'user': 'zoey', 'drink': 'long black', 'size': 'medium' }," +
                         "{ 'user': 'zoey', 'drink': 'short espresso', 'size': 'small'}]";

        ////Example Payments JSON test data
        private string paymentsJson = "[{ 'user': 'coach', 'amount': 2.50 }," +
                             "{ 'user': 'ellis', 'amount': 2.60 }," +
                             "{ 'user': 'rochelle', 'amount': 4.50 }," +
                             "{ 'user': 'ellis', 'amount': 0.65 }]" ;

        [Fact]
        public void OutputJsonInExpectedForm()
        {
            // Based on test data above the expected result would be 
            // { "user": "coach","order_total": 8.00, "payment_total": 2.50, "balance": 5.50 },
            // { "user": "ellis","order_total": 3.25, "payment_total": 3.25, "balance": 0.00 },
            // { "user": "rochelle", "order_total": 4.50, "payment_total": 4.50, "balance": 0.00 },
            // { "user": "zoey","order_total": 6.53, "payment_total": 0.00, "balance": 6.53 }

           var result = OrderService.GetResult(pricesJson, ordersJson, paymentsJson);

           Assert.Contains("{\"user\":\"coach\",\"order_total\":8.00,\"payment_total\":2.50,\"balance\":5.50},", result);
           Assert.Contains("{\"user\":\"ellis\",\"order_total\":3.25,\"payment_total\":3.25,\"balance\":0.00},", result);
           Assert.Contains("{\"user\":\"rochelle\",\"order_total\":4.50,\"payment_total\":4.50,\"balance\":0.00},", result);
           Assert.Contains("{\"user\":\"zoey\",\"order_total\":6.53,\"payment_total\":0.0,\"balance\":6.53}", result);
        }

        [Fact]
        public void HasUsersWhoOrderedCoffee()
        {
            // Assert result[0].user == "coach"
            // Assert result[1].user == "ellis"
            // Assert result[2].user == "rochelle"
            // Assert result[3].user == "zoey"

            var stringResult = OrderService.GetResult(pricesJson, ordersJson, paymentsJson);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderPayment>>(stringResult);

            Assert.Equal("coach", result[0].User);
            Assert.Equal("ellis", result[1].User);
            Assert.Equal("rochelle", result[2].User);
            Assert.Equal("zoey", result[3].User);
        }

        [Fact]
        public void HasOrderTotalsForUsers()
        {
            // Assert result[0].order_total == 8.00
            // Assert result[1].order_total == 3.25
            // Assert result[2].order_total == 4.50
            // Assert result[3].order_total == 6.53

            var stringResult = OrderService.GetResult(pricesJson, ordersJson, paymentsJson);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderPayment>>(stringResult);

            Assert.Equal(result[0].OrderTotal, (decimal)8.00);
            Assert.Equal(result[1].OrderTotal, (decimal)3.25);
            Assert.Equal(result[2].OrderTotal, (decimal)4.50);
            Assert.Equal(result[3].OrderTotal, (decimal)6.53);
        }

        [Fact]
        public void HasPaymentTotalsForUsers()
        {
            // Assert result[0].payment_total == 2.50
            // Assert result[1].payment_total == 3.25
            // Assert result[2].payment_total == 4.50
            // Assert result[3].payment_total == 0.00

            var stringResult = OrderService.GetResult(pricesJson, ordersJson, paymentsJson);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderPayment>>(stringResult);

            Assert.Equal(result[0].PaymentTotal, (decimal)2.50);
            Assert.Equal(result[1].PaymentTotal, (decimal)3.25);
            Assert.Equal(result[2].PaymentTotal, (decimal)4.50);
            Assert.Equal(result[3].PaymentTotal, (decimal)0.00);
        }

        [Fact]
        public void HasCurrentBalanceForUsers()
        {
            // Assert result[0].balance == 5.50
            // Assert result[1].balance == 0.00
            // Assert result[2].balance == 0.00
            // Assert result[3].balance == 6.53

            var stringResult = OrderService.GetResult(pricesJson, ordersJson, paymentsJson);
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrderPayment>>(stringResult);

            Assert.Equal(result[0].Balance, (decimal)5.50);
            Assert.Equal(result[1].Balance, (decimal)0.00);
            Assert.Equal(result[2].Balance, (decimal)0.00);
            Assert.Equal(result[3].Balance, (decimal)6.53);
        }
    }
}
