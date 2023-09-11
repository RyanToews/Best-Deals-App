using Newtonsoft.Json;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace BestDealClient2.Models
{
    public class Product
    {
        [JsonProperty("id")]
        [Required]
        public string Id { get; set; }

        [JsonProperty("name")]
        [Required]
        public string Name { get; set; }

        [JsonProperty("price")]
        [Required]
        public string Price { get; set; }

        [JsonProperty("quantity")]
        [Required]
        public int Quantity { get; set; }
    }
}
