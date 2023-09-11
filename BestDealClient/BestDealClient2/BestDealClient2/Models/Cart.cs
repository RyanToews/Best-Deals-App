using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace BestDealClient2.Models
{
    public class Cart
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string StoreId { get; set; }

        public string StoreName { get; set; }
        public string ItemIds { get; set; }
        public int Quantity { get; set; }

        [Ignore]
        public List<Item> Items { get; set; } // This property is a list of Item objects. It's not stored in the database.

        [Ignore]
        public double TotalPrice
        {
            get
            {
                if (Items == null || !Items.Any()) return 0;
                return Items.Sum(i => i.PriceAsDouble);
            }
        }

    }


}
