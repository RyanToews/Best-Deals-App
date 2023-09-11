using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestDealLib.Models
{
    public class StoreDistance
    {
        public int StoreId { get; set; }
        public int Distance { get; set; }

        public StoreDistance(int storeId, int distance)
        {
            this.StoreId = storeId;
            this.Distance = distance;
        }

        public StoreDistance()
        {

        }
    }
}
