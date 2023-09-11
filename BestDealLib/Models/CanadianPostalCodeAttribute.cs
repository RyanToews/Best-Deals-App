using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BestDealLib.Models
{
    public class CanadianPostalCodeAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return true;

            string postalCode = value.ToString()!;

            // Canadian postal code regex pattern
            string pattern = @"^[ABCEGHJKLMNPRSTVXY]\d[A-Z] \d[A-Z]\d$";
            
            // Check if the postal code matches the pattern
            return Regex.IsMatch(postalCode, pattern);
        }
    }
}
