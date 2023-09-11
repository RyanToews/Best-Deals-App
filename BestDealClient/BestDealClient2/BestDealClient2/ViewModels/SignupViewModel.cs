using BestDealClient.Services;
using BestDealClient2.Helpers;
using BestDealClient2.Models;
using BestDealClient2.Views;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;


namespace BestDealClient2.ViewModels
{
    public class SignupViewModel : BaseViewModel
    {
        // The command that is executed when the signup button is tapped.
        public Command SignupCommand { get; }

        // Stores the text inputted into the username field.
        public string UsernameText { get; set; }

        // Stores the text inputted into the password field.
        public string PasswordText { get; set; }

        // Stores the text inputted into the confirm password field.
        public string ConfirmPasswordText { get; set; }

        // Stores the text inputted into the first name field.
        public string FirstnameText { get; set; }

        // Stores the text inputted into the last name field.
        public string LastnameText { get; set; }

        // Stores the text inputted into the email field.
        public string EmailText { get; set; }

        // Stores the text inputted into the address field.
        public string AddressText { get; set; }

        // Stores the text inputted into the city field.
        public string CityText { get; set; }

        // Stores the text inputted into the postal code field.
        public string PostalCodeText { get; set; }

        // Stores the text inputted into the province field.
        public string ProvinceText { get; set; }

        // Stores the text inputted into the phone field.
        public string PhoneText { get; set; }

        // The constructor for the SignupViewModel. It initializes the signup command.
        public SignupViewModel()
        {
            SignupCommand = new Command(OnSignupTapped);
        }


        /// <summary>
        /// Handles the event when the Signup button is tapped.
        /// </summary>
        /// <param name="sender">The object that initiated the event.</param>
        private async void OnSignupTapped(object sender)
        {
            // Validate user input
            if (ValidateInput())
            {
                // Construct the registration model
                var signupModel = new Register
                {
                    Username = UsernameText,
                    Password = PasswordText,
                    ConfirmPassword = ConfirmPasswordText,
                    Firstname = FirstnameText,
                    Lastname = LastnameText,
                    Email = EmailText,
                    Address = AddressText,
                    City = CityText,
                    PostalCode = PostalCodeText,
                    Province = ProvinceText,
                    Phone = PhoneText
                };

                // Attempt to sign up the user
                string response = await UserManager.SignupAsync(signupModel);

                // If the registration was successful...
                if (response == "success")
                {
                    // Construct the login model
                    var loginModel = new Login
                    {
                        Username = UsernameText,
                        Password = PasswordText
                    };

                    // Attempt to log in the user
                    bool signedIn = await UserManager.LoginAsync(loginModel);

                    // If login was successful, navigate to the landing page
                    if (signedIn)
                    {
                        await Shell.Current.GoToAsync($"//{nameof(LandingPage)}");
                    }
                }
                // If the registration failed, display the error message
                else if (response != null)
                {
                    await Application.Current.MainPage.DisplayToastAsync(response);
                }
            }
        }

        /// <summary>
        /// Performs input validation before initiating the signup process.
        /// </summary>
        /// <returns>True if the input is valid, otherwise false.</returns>
        private bool ValidateInput()
        {
            string[] fields = { UsernameText, PasswordText, ConfirmPasswordText, FirstnameText, LastnameText, EmailText, AddressText, CityText, PostalCodeText, ProvinceText, PhoneText };
            return InputValidator.ValidateEmptyFields(fields) &&
                   InputValidator.ValidatePasswordsMatch(PasswordText, ConfirmPasswordText) &&
                   InputValidator.ValidatePasswordStrength(PasswordText) &&
                   InputValidator.ValidateEmailFormat(EmailText) &&
                   InputValidator.ValidatePhoneFormat(PhoneText) &&
                   InputValidator.ValidatePostalCodeFormat(PostalCodeText) &&
                   InputValidator.ValidateUsernameFormat(UsernameText);
        }
    }
}