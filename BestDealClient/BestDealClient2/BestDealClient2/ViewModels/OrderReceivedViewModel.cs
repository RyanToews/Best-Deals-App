using BestDealClient2.Helpers;
using BestDealClient2.Models;
using BestDealClient2.Services;
using BestDealClient2.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BestDealClient2.ViewModels
{
    public class OrderReceivedViewModel : ContentView
    {
        string _orderId;

        public string OrderId { 
            get
            {
                return _orderId;
            }
            set 
            { 
                _orderId = value; 
                OnPropertyChanged(nameof(OrderId));
            }
        }

        string _storeAddress;

        public string StoreAddress
        {
            get
            {
                return _storeAddress;
            }
            set
            {
                _storeAddress = value;
                OnPropertyChanged(nameof(StoreAddress));
            }
        }

        public Command ContinueCommand { get; }

        public OrderReceivedViewModel()
        {
            ContinueCommand = new Command(OnContinueClicked);
            OrderId = UserManager.lastOrder.Id.ToString();
            StoreService storeService = new StoreService();
            Store store = new Store();
            Task.Run(async () =>
            {
                store = await storeService.GetStoreById(UserManager.lastOrder.StoreId);
            }).GetAwaiter().GetResult();
            StoreAddress = store.Address + ", " + store.City + ", " + store.ProvinceState;
        }

        private async void OnContinueClicked(object obj)
        {
            await Shell.Current.GoToAsync($"///{nameof(CartPage)}");
        }
    }

}