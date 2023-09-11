using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BestDealLib.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order? Order { get; set; }

        public string? ItemId { get; set; }

        public int ItemStoreId { get; set; }

        [ForeignKey(nameof(ItemId) + ", " + nameof(ItemStoreId))]
        public virtual Item? Item { get; set; }

        public decimal PriceAtSale { get; set; } 

        public int QuantityOrdered { get; set; }
    }
}
