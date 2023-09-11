using BestDealClient2.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using BestDealClient2.Models;
using BestDealClient2.Services;
using BestDealClient2.Helpers;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;

namespace BestDealClient2.ViewModels
{
    public class CartViewModel : BaseViewModel
    {
        public ICommand CheckoutCommand { get; private set; }
        public ICommand DeleteItemCommand { get; private set; }
        public ICommand DeleteCartCommand { get; private set; }

        private ObservableCollection<Cart> _carts;
        public ObservableCollection<Cart> Carts
        {
            get { return _carts; }
            set
            {
                _carts = value;
                OnPropertyChanged();
            }
        }

        public CartViewModel()
        {
            Title = "Cart";
            Carts = new ObservableCollection<Cart>();
            CheckoutCommand = new Command<Cart>(OnCheckoutClicked);
            DeleteItemCommand = new Command<Item>(OnDeleteItemClicked);
            DeleteCartCommand = new Command<Cart>(OnDeleteCartClicked);
        }
        private async void OnDeleteItemClicked(Item item)
        {
            // Delete item from cart in database
            var databaseService = DependencyService.Get<DatabaseService>();
            await databaseService.DeleteItemFromCartAsync(item);

            // Reload the carts
            await RefreshItems();
        }
        private async void OnDeleteCartClicked(Cart cart)
        {
            // Delete cart from database
            var databaseService = DependencyService.Get<DatabaseService>();
            await databaseService.DeleteCartAsync(cart);

            // Reload the carts
            await RefreshItems();
        }
        private async Task LoadCart()
        {
            var databaseService = DependencyService.Get<DatabaseService>();
            var cartItems = await databaseService.GetCartItemsAsync();
            List<Task> tasks = new List<Task>();

            foreach (var cartItem in cartItems)
            {
                tasks.Add(LoadCartItems(cartItem));
            }
            await Task.WhenAll(tasks);
            if (Carts.Count == 0)
            {
                await Application.Current.MainPage.DisplayToastAsync("No items in cart.");
            }
        }
        private async Task LoadCartItems(Cart cartItem)
        {
            ItemService itemService = new ItemService();
            cartItem.Items = await itemService.FindItemsStoreIdItemIds(cartItem.StoreId, cartItem.ItemIds);
            Carts.Add(cartItem);

        }
        public async Task RefreshItems()
        {
            Carts.Clear();
            await LoadCart();
        }
        private async void OnCheckoutClicked(Cart cart)
        {
            CartService.CurrentCart = cart;

            // Pass the encoded Cart object to the CheckoutPage
            await Shell.Current.GoToAsync($"{nameof(CheckoutPage)}");
        }
    }
}
