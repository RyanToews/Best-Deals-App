using BestDealClient2.Data;
using BestDealClient2.Helpers;
using BestDealClient2.Models;
using BestDealClient2.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BestDealClient2
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            var databaseConnection = DependencyService.Get<IDatabaseConnection>().DbConnection();
            databaseConnection.CreateTableAsync<Cart>();
            MainPage = new AppShell();
            CheckAgeVerification();
        }

        private async void CheckAgeVerification()
        {
            bool verified = await SecureStorage.GetAsync("age_verified") == "true";
            if (!verified)
            {
                await Shell.Current.GoToAsync("//AgeVerificationPage");
            }
            else
            {
                CheckUserAuthentication();
            }
        }

        private async void CheckUserAuthentication()
        {
            string token = await SecureStorage.GetAsync("oauth_token");
            bool valid = await UserManager.CheckTokenValidity(token);
            if (!valid)
            {
                // If the token is null, navigate to the LoginPage
                await Shell.Current.GoToAsync("//LoginPage");
            } 
            else if (await SecureStorage.GetAsync("merrcoToken") == null)
            {
                await SecureStorage.SetAsync("merrcoToken", await BaseAPIHelper.getMerrcoToken());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
