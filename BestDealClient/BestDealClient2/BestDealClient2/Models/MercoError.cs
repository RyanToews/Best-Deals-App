using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BestDealClient2.Models
{
    public class MercoError
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

}
