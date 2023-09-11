using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BestDealLib.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderId { get; set; }
        public int OrderNum { get; set; }
        public bool IsPaid { get; set; }
        public bool ForDelivery { get; set; }
        public string? OrderType { get; set; }
        public string? CustomId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? LastFourDigits { get; set; }
        
        [ForeignKey("Store")]
        public int StoreId { get; set; }
        public virtual Store? Store { get; set; }
        public List<OrderItem>? OrderItems { get; set; }

        public decimal TotalCharge { get; set; }
        public string? TransactionId { get; set; }

        [NotMapped]
        public string EightDigitId
        {
            get
            {
                return OrderId.ToString().PadLeft(8, '0');
            }
        }
    }
}
