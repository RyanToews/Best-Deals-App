using Xamarin.Forms;
using System.Text.RegularExpressions;
using System.Linq;
using Xamarin.CommunityToolkit.Extensions;

namespace BestDealClient.Services
{
    /// <summary>
    /// Static class that provides methods to validate user input.
    /// </summary>
    public static class InputValidator
    {
        /// <summary>
        /// Validates that none of the provided fields are empty or whitespace.
        /// </summary>
        /// <returns>True if all fields are filled, otherwise false.</returns>
        public static bool ValidateEmptyFields(string[] fields)
        {
            foreach (var field in fields)
            {
                if (string.IsNullOrWhiteSpace(field))
                {
                    Application.Current.MainPage.DisplayToastAsync("All fields must be filled");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Validates that the provided password and confirmation match.
        /// </summary>
        /// <returns>True if they match, otherwise false.</returns>
        public static bool ValidatePasswordsMatch(string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                Application.Current.MainPage.DisplayToastAsync("Passwords do not match");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates that the provided password meets strength requirements.
        /// </summary>
        /// <returns>True if the password is strong, otherwise false.</returns>
        public static bool ValidatePasswordStrength(string password)
        {
            if (password.Length < 8 || !password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsDigit))
            {
                Application.Current.MainPage.DisplayToastAsync("Password should be at least 8 characters, have a capital and lower case letter, and a number");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates that the provided email is in a valid format.
        /// </summary>
        /// <returns>True if the email is valid, otherwise false.</returns>
        public static bool ValidateEmailFormat(string email)
        {
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                Application.Current.MainPage.DisplayToastAsync("Invalid email format");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates that the provided phone number is in a valid format.
        /// </summary>
        /// <returns>True if the phone number is valid, otherwise false.</returns>
        public static bool ValidatePhoneFormat(string phone)
        {
            if (!Regex.IsMatch(phone, @"^[0-9]{10}$"))
            {
                Application.Current.MainPage.DisplayToastAsync("Invalid phone format");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates that the provided postal code is in a valid US (5 or 9 digits) or Canadian (A1A 1A1) format.
        /// </summary>
        /// <returns>True if the postal code is valid, otherwise false.</returns>
        public static bool ValidatePostalCodeFormat(string postalCode)
        {
            // Match US (5 or 9 digits) or Canadian (A1A 1A1) postal code formats
            if (!Regex.IsMatch(postalCode, @"(^\d{5}(-\d{4})?$)|(^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$)"))
            {
                Application.Current.MainPage.DisplayToastAsync("Invalid postal code format");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates that the provided username is in a valid format.
        /// </summary>
        /// <returns>True if the username is valid, otherwise false.</returns>
        public static bool ValidateUsernameFormat(string username)
        {
            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9]+$"))
            {
                Application.Current.MainPage.DisplayToastAsync("Invalid username format");
                return false;
            }
            return true;
        }
    }
}
