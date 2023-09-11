using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BestDealClient2.Models
{
    public class Register
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Province { get; set; }

        public string Phone { get; set; }
    }
}
