using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BestDealClient2.Models
{
    public class PaymentData
    {
        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("card_expiry_month")]
        public int CardExpiryMonth { get; set; }

        [JsonProperty("card_expiry_year")]
        public int CardExpiryYear { get; set; }

        [JsonProperty("card_number")]
        public string CardNumber { get; set; }

        [JsonProperty("cvv2")]
        public int CVV2 { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }

}
