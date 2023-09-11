using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace BestDealLib.Models
{
    public class Store
    {
        [Key]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("storeNumber")]
        public int? StoreNumber { get; set; }

        [JsonProperty("baseEndpoint")]
        public string? BaseEndpoint { get; set; }

        [JsonProperty("address")]
        public string? Address { get; set; }

        [JsonProperty("city")]
        public string? City { get; set; }

        [JsonProperty("provinceState")]
        public string? ProvinceState { get; set; }

        [JsonProperty("postalCode")]
        public string? PostalCode { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        public Store()
        {
            // Default constructor (needed by Entity Framework)
        }

        public Store(InputStore inputStore, string baseUrl)
        {
            StoreNumber = inputStore.Id;
            Address = inputStore.Address;
            PostalCode = inputStore.PostalCode;
            ProvinceState = inputStore.ProvinceState;
            City = inputStore.City;
            Name = inputStore.Name;
            BaseEndpoint = baseUrl;
        }
    }
}
