using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BestDealClient2.Models
{
    public class MerrcoResponse
    {
        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("card_type")]
        public string CardType { get; set; }

        [JsonProperty("card_suffix")]
        public string CardSuffix { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }

        [JsonProperty("transaction_success")]
        public bool TransactionSuccess { get; set; }

        [JsonProperty("transaction_result")]
        public string TransactionResult { get; set; }

        [JsonProperty("transaction_message")]
        public string TransactionMessage { get; set; }

        [JsonProperty("transaction_time")]
        public long TransactionTime { get; set; }

        [JsonProperty("transaction_type")]
        public string TransactionType { get; set; }
    }

}
