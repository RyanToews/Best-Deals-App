using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestDealClient2.Models
{
    public class Item
    {
        public string Id { get; set; }
        public int StoreId { get; set; }
        // Add other properties as needed
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public string Price { get; set; }

        public double PriceAsDouble
        {
            get
            {
                double.TryParse(Price, out double priceAsDouble);
                return priceAsDouble;
            }
        }

    }

}

