using BestDealClient2.Helpers;
using BestDealClient2.ViewModels;
using BestDealClient2.Views;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BestDealClient2
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            //Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(SignupPage), typeof(SignupPage));
            Routing.RegisterRoute(nameof(CheckoutPage), typeof(CheckoutPage));
            Routing.RegisterRoute(nameof(CheckoutFormPage), typeof(CheckoutFormPage));
            Routing.RegisterRoute(nameof(OrderReceivedPage), typeof(OrderReceivedPage));
        }

        private async void OnLogoutItemClicked(object sender, EventArgs e)
        {
            SecureStorage.Remove("merrcoToken");
            SecureStorage.Remove("oauth_token");
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
