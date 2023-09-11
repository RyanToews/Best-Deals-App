using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BestDealLib.Models
{
    public class PasswordRequirementsAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return true;

            string password = value.ToString()!;

            // Password length requirements
            if (password!.Length < 8 || password.Length > 32)
                return false;

            // Password character requirements
            if (!HasLowercaseLetter(password) || !HasUppercaseLetter(password) || !HasNumber(password))
                return false;

            return true;
        }

        private bool HasLowercaseLetter(string password)
        {
            return password.Any(char.IsLower);
        }

        private bool HasUppercaseLetter(string password)
        {
            return password.Any(char.IsUpper);
        }

        private bool HasNumber(string password)
        {
            return password.Any(char.IsDigit);
        }
    }
}