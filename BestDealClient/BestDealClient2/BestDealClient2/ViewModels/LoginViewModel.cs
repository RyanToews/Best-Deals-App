using BestDealClient2.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using BestDealClient2.Models;
using Xamarin.Essentials;
using BestDealClient2.Helpers;
using Xamarin.CommunityToolkit.Extensions;

namespace BestDealClient2.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command SignupCommand { get; }
        public string UsernameText { get; set; }
        public string PasswordText { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            SignupCommand = new Command(OnSignupTapped);
        }

        private async void OnLoginClicked(object obj)
        {
            var loginModel = new Login
            {
                Username = UsernameText,
                Password = PasswordText
            };
            if (loginModel.Username == null || loginModel.Username == "")
            {
                await Application.Current.MainPage.DisplayToastAsync("username empty");
                return;
            }
            else if (loginModel.Password == null || loginModel.Password == "")
            {
                await Application.Current.MainPage.DisplayToastAsync("password empty");
            } else
            {
                bool signedIn = await UserManager.LoginAsync(loginModel);
                if (signedIn)
                {
                    await Shell.Current.GoToAsync($"//{nameof(LandingPage)}");
                    // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                }
            }
            
        }

        private async void OnSignupTapped(object sender)
        {
            await Shell.Current.GoToAsync(nameof(SignupPage));
        }
    }
}
