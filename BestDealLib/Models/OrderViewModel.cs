using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BestDealLib.Models
{
    public class OrderViewModel
    {
        [JsonProperty("id")]
        [Required]
        public int Id { get; set; }

        [JsonProperty("username")]
        [Required]
        public string? Username { get; set; }       

        [JsonProperty("storeId")]
        [Required]
        public int StoreId { get; set; }

        [JsonProperty("number")]
        [Required]
        public int Number { get; set; }

        [JsonProperty("isPaid")]
        [Required]
        public bool IsPaid { get; set; }

        [JsonProperty("forDelivery")]
        [Required]
        public bool ForDelivery { get; set; }

        [JsonProperty("orderType")]
        [Required]
        public string? OrderType { get; set; }

        [JsonProperty("customId")]
        [Required]
        public string? CustomId { get; set; }

        [JsonProperty("products")]
        [Required]
        public List<ProductViewModel>? Products { get; set; }

        [JsonProperty("lastFourDigits")]
        public string? LastFourDigits { get; set; }

        [JsonProperty("customer")]
        [Required]
        public CustomerViewModel? Customer { get; set; }

        [JsonProperty("orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonProperty("totalCharge")]
        public decimal TotalCharge { get; set; }

        [JsonProperty("transactionId")]
        public string? TransactionId { get; set; }

        [JsonProperty("storeAddress")]
        public string? StoreAddress { get; set; }
    }

    public class ProductViewModel
    {
        [JsonProperty("id")]
        [Required]
        public string? Id { get; set; }

        [JsonProperty("name")]
        [Required]
        public string? Name { get; set; }

        [JsonProperty("price")]
        [Required]
        public string? Price { get; set; }

        [JsonProperty("quantity")]
        [Required]
        public int Quantity { get; set; }
    }

    public class CustomerViewModel
    {
        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        [JsonProperty("lastName")]
        public string? LastName { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("phone")]
        public string? Phone { get; set; }

        [JsonProperty("address")]
        public string? Address { get; set; }

        [JsonProperty("addressTwo")]
        public string? AddressTwo { get; set; }

        [JsonProperty("city")]
        public string? City { get; set; }

        [JsonProperty("state")]
        public string? State { get; set; }

        [JsonProperty("postalCode")]
        public string? PostalCode { get; set; }
    }
}
