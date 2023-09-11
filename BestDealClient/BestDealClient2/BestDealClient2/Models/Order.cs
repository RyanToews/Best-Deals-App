using BestDealClient2.Models;
using Newtonsoft.Json;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;

public class Order
{
    [JsonProperty("id")]
    [Required]
    public int Id { get; set; }

    [JsonProperty("username")]
    [Required]
    public string Username { get; set; }

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
    public string OrderType { get; set; }

    [JsonProperty("customId")]
    [Required]
    public string CustomId { get; set; }

    [JsonProperty("products")]
    [Required]
    public List<Product> Products { get; set; }

    [JsonProperty("lastFourDigits")]
    public string LastFourDigits { get; set; }

    [JsonProperty("customer")]
    [Required]
    public UserInfo Customer { get; set; }

    [JsonProperty("orderDate")]
    public DateTime OrderDate { get; set; }

    [JsonProperty("totalCharge")]
    public decimal TotalCharge { get; set; }

    [JsonProperty("transactionId")]
    public string TransactionId { get; set; }

    [JsonProperty("storeAddress")]
    public string StoreAddress { get; set; }

}
