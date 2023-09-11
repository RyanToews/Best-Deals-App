using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BestDealLib.Models
{
    public class RegisterViewModel
    {
        [JsonProperty("username")]
        [Required]
        public string? Username { get; set; }

        [JsonProperty("password")]
        [Required]
        [PasswordRequirements(ErrorMessage = 
            "Password must be 8-32 characters and include at least 1 of each of uppercase/lowercase/numbers")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [JsonProperty("confirmPassword")]
        [Required]
        [PasswordRequirements(ErrorMessage = 
            "Password must be 8-32 characters and include at least 1 of each of uppercase/lowercase/numbers")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [JsonProperty("firstname")]
        [Required]
        public string? Firstname { get; set; }

        [JsonProperty("lastname")]
        [Required]
        public string? Lastname { get; set; }
        
        [JsonProperty("email")]
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [JsonProperty("address")]
        [Required]
        public string? Address { get; set; }

        [JsonProperty("addressTwo")]
        public string? AddressTwo { get; set; }

        [JsonProperty("city")]
        [Required]
        public string? City { get; set; }

        [JsonProperty("postalCode")]
        [Required]
        [CanadianPostalCode(ErrorMessage = "Invalid Postal Code; must match format A1A 1A1")]
        public string? PostalCode { get; set; }

        [JsonProperty("province")]
        [Required]
        public string? Province { get; set; }

        [JsonProperty("phone")]
        [Required]
        public string? Phone { get; set; }
    }
}