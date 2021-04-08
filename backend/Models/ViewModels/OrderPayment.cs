using Newtonsoft.Json;

namespace Entities.ViewModels
{
    public class OrderPayment
    {
        [JsonProperty("user")]
        public string User { get; set; }
        [JsonProperty("order_total")]
        public decimal OrderTotal { get; set; }
        [JsonProperty("payment_total")]
        public decimal PaymentTotal { get; set; }
        [JsonProperty("balance")]
        public decimal Balance { get; set; }
    }
}
