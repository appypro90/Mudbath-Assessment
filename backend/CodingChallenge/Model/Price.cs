using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingChallenge.Model
{
    public class Price
    {
        [JsonProperty("drink_name")]
        public string DrinkName { get; set; }
        public Size Prices { get; set; }
    }
}
