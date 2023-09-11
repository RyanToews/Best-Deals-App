using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BestDealLib.Models
{
    public class User
    {
        [Key]
        public string? Username { get; set; }
        
        [JsonIgnore]
        [DataType(DataType.Password)]
        public string? HashedPassword { get; set; }

        public string? Firstname { get; set; }
        public string? Lastname { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        public string? Address { get; set; }
        public string? AddressTwo { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Province { get; set; }
        public string? Phone { get; set; }
        public List<Order>? Orders { get; set; }

        public string? Salt { get; set; }
    }
}
