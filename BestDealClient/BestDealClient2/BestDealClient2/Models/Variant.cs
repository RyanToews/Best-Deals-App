using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestDealClient2.Models
{
    public class Variant
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("upc")]
        public string Upc { get; set; }

        [JsonProperty("sellingUnit")]
        public string SellingUnit { get; set; }

        [JsonProperty("stock")]
        public string Stock { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("salePrice")]
        public string SalePrice { get; set; }

        [JsonProperty("location")]
        public object Location { get; set; }

        [JsonProperty("weightPerUnit")]
        public double WeightPerUnit { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("dfe")]
        public double Dfe { get; set; }
    }
}

