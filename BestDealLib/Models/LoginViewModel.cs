using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BestDealLib.Models
{
    public class LoginViewModel
    {
        [JsonProperty("username")]
        [Required]
        public string? Username { get; set; }

        [JsonProperty("password")]
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }    
    }

    public class TokenVerificationViewModel
    {
        public string? Token { get; set; }
    }
}