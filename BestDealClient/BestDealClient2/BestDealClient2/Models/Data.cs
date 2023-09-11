using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestDealClient2.Models
{
    public class Data
    {
        [JsonProperty("items")]
        public List<ItemWrapper> Items { get; set; }
    }
}
