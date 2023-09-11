using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BestDealClient2.Models
{
    public class MerrcoErrorResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("errors")]
        public List<MercoError> Errors { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }
    }
}
