using BestDealClient2.ViewModels;
using BestDealClient2.Models;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BestDealClient2.Views
{
    public partial class LandingPage : ContentPage
    {
        public LandingPage()
        {
            InitializeComponent();
        }
        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var storeDistancePair = (StoreDistancePair)e.Item;
            await Navigation.PushAsync(new StorePage(storeDistancePair.Store.Id));
        }

    }

}