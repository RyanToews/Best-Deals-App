using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestDealLib.Models
{
    public class InputItemWrapper
    {
        [JsonProperty("item")]
        public InputItem? InputItem { get; set; }
    }
}
