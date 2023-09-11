using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestDealClient2.Models
{
    public class StoreDistance
    {
        public int StoreID { get; set; }
        public int Distance { get; set; }

        public StoreDistance(int storeID, int distance)
        {
            this.StoreID = storeID;
            this.Distance = distance;
        }

        public StoreDistance()
        {

        }

    }
}
