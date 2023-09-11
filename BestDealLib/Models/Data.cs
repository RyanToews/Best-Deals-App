using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestDealLib.Models
{
    public class Data
    {
        [JsonProperty("items")]
        public List<InputItemWrapper>? Items { get; set; }
    }
}
