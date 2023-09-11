using Newtonsoft.Json;


namespace BestDealClient2.Models
{
    public class Store
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("storeNumber")]
        public int StoreNumber { get; set; }

        [JsonProperty("baseEndpoint")]
        public string BaseEndpoint { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("provinceState")]
        public string ProvinceState { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public Store()
        {
            // Default constructor (needed by Entity Framework)
        }

    }
}
