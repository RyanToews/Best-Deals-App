using BestDealClient2.Helpers;
using BestDealClient2.Services;
using BestDealClient2.Views;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace BestDealClient2.ViewModels
{
    public class CheckoutFormViewModel : BaseViewModel
    {
        string url;

        // Any suffix appended to the URL
        string suffix { get; set; }

        private string firstName;
        // Customer's first name.
        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        private string lastName;
        // Customer's last name.
        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }

        private string creditCardNumber;
        // Customer's credit card number.
        public string CreditCardNumber
        {
            get => creditCardNumber;
            set => SetProperty(ref creditCardNumber, value);
        }

        private string expiryMonth;
        // Expiry month of the credit card.
        public string ExpiryMonth
        {
            get => expiryMonth;
            set => SetProperty(ref expiryMonth, value);
        }

        private string expiryYear;
        // Expiry year of the credit card.
        public string ExpiryYear
        {
            get => expiryYear;
            set => SetProperty(ref expiryYear, value);
        }

        private string ccv;
        // The CCV number of the credit card.
        public string CCV
        {
            get => ccv;
            set => SetProperty(ref ccv, value);
        }

        // The command to be executed when an order is placed.
        public Command OrderCommand { get; }

        public CheckoutFormViewModel()
        {
            FirstName = UserManager.CurrentUserInfo.Firstname;
            LastName = UserManager.CurrentUserInfo.Lastname;
            url = "https://sandbox-apigateway.payfirma.com/transaction-service/sale";
            Title = "Checkout";
            OrderCommand = new Command(OnOrderClicked);
        }

        /// <summary>
        /// When the order is clicked, if the form is valid, an order will be placed.
        /// </summary>
        private async void OnOrderClicked(object obj)
        {
            if (IsFormValid())
            {
                string result = await OrderService.Order(url, ExpiryMonth, ExpiryYear, CreditCardNumber, CCV);
                await Application.Current.MainPage.DisplayToastAsync(result);
                if (result == "success")
                {
                    await Shell.Current.GoToAsync($"{nameof(OrderReceivedPage)}");
                }
            }
        }

        /// <summary>
        /// Checks if the form is valid, with all the required fields filled correctly.
        /// </summary>
        private bool IsFormValid()
        {
            // Checking each field one by one and displaying relevant messages if a field is not valid.
            // Return true if all fields are valid, else return false.
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                Application.Current.MainPage.DisplayToastAsync("First name cannot be empty.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(LastName))
            {
                Application.Current.MainPage.DisplayToastAsync("Last name cannot be empty.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(ExpiryMonth) || (CreditCardNumber.Trim().Length != 16 && CreditCardNumber.Trim().Length != 15))
            {
                Application.Current.MainPage.DisplayToastAsync("Invalid credit card number.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(ExpiryMonth) || !int.TryParse(ExpiryMonth, out _))
            {
                Application.Current.MainPage.DisplayToastAsync("Invalid expiration month.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(ExpiryYear) || !int.TryParse(ExpiryYear, out _))
            {
                Application.Current.MainPage.DisplayToastAsync("Invalid expiration year.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(CCV) || !int.TryParse(CCV, out _))
            {
                Application.Current.MainPage.DisplayToastAsync("Invalid CVV.");
                return false;
            }

            return true;
        }
    }
}
