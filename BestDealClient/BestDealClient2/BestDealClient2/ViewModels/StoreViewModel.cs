using BestDealClient2.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using BestDealClient2.Services;
using Xamarin.CommunityToolkit.Extensions;

namespace BestDealClient2.ViewModels
{
    public class StoreViewModel : INotifyPropertyChanged
    {
        private int _storeId;

        private string storeName;

        public event PropertyChangedEventHandler PropertyChanged;

        private Store _store;
        public Store Store
        {
            get { return _store; }
            set
            {
                _store = value;
                OnPropertyChanged(nameof(Store));
            }
        }

        public ICommand AddToCartCommand { get; }


        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }



        public ICommand SubmitSearchCommand { get; }

        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();

        public StoreViewModel(int storeId)
        {
            _storeId = storeId;
            LoadStore(storeId);
            SubmitSearchCommand = new Command(async () => await SubmitSearch());
            AddToCartCommand = new Command<Item>(AddToCart);
        }

        async void AddToCart(Item item)
        {
            var databaseService = DependencyService.Get<DatabaseService>();
            var cartForStore = await databaseService.GetCartForStoreAsync(item.StoreId.ToString());

            if (cartForStore == null)
            {
                // If no cart for this store, create a new one
                var newCart = new Cart
                {
                    StoreId = item.StoreId.ToString(),
                    StoreName = storeName,
                    ItemIds = item.Id.ToString(),
                    Quantity = 1
                };

                await databaseService.SaveCartAsync(newCart);
            }
            else
            {
                // If a cart for this store exists, add the item to it
                cartForStore.ItemIds += ";" + item.Id;
                cartForStore.Quantity += 1;

                await databaseService.SaveCartAsync(cartForStore);
            }
            await Application.Current.MainPage.DisplayToastAsync("Item added to cart.");
        }

        private async void LoadStore(int storeId)
        {
            StoreService storeService = new StoreService();
            Store store = await storeService.GetStoreById(storeId);
            if (store != null)
            {
                Store = store;
                storeName = store.Name;
            }
            else
            {
                await Application.Current.MainPage.DisplayToastAsync("Store not found.");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task SubmitSearch()
        {
            ItemService itemService = new ItemService();
            List<Item> items = await itemService.SearchItemsStoreIdPartialName(_storeId, SearchText);
            if (items != null && items.Count != 0)
            {
                Items.Clear();
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayToastAsync("No items found.");
            }
        }

    }
}
