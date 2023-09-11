using BestDealClient2.Models;
using BestDealClient2.Services;
using BestDealClient2.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace BestDealClient2.ViewModels
{
    public class CheckoutViewModel : BaseViewModel
    {
        MerrcoResponse merrcoResponse;
        private Cart _cart;
        private bool _isOnline = false;
        private bool _isInStore = false;
        public bool IsOnline
        {
            get { return _isOnline; }
            set
            {
                SetProperty(ref _isOnline, value);
                if (_isOnline) IsInStore = false;
            }
        }

        public bool IsInStore
        {
            get { return _isInStore; }
            set
            {
                SetProperty(ref _isInStore, value);
                if (_isInStore) IsOnline = false;
            }
        }

        public Cart Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                OnPropertyChanged();
            }
        }

        public Command ProceedCommand { get; }

        public CheckoutViewModel()
        {
            Cart = CartService.CurrentCart;
            Title = "Checkout";
            ProceedCommand = new Command(OnProceedClicked);
        }

        private async void OnProceedClicked(object obj)
        {
            if (!IsOnline && !IsInStore)
            {
                await Application.Current.MainPage.DisplayToastAsync("Please select an option.");
                return;
            }
            if (IsOnline)
            {
                await Shell.Current.GoToAsync($"{nameof(CheckoutFormPage)}");
            } 
            else
            {
                string result = await OrderService.Order();
                await Application.Current.MainPage.DisplayToastAsync(result);
                if (result == "success")
                {
                    await Shell.Current.GoToAsync($"{nameof(OrderReceivedPage)}");
                }
            }
        }
    }
}
