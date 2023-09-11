using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestDealClient2.Models
{
    public class ItemWrapper
    {
        [JsonProperty("item")]
        public Item Item { get; set; }
    }
}
