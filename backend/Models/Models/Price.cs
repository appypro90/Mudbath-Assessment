using Newtonsoft.Json;

namespace Entities.Models
{
    public class Price
    {
        [JsonProperty("drink_name")]
        public string DrinkName { get; set; }
        public Size Prices { get; set; }
    }
}
