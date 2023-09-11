using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using BestDealClient2.Models;
using System.Collections.ObjectModel;
using BestDealClient2.Services;
using Xamarin.CommunityToolkit.Extensions;

namespace BestDealClient2.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public ICommand SubmitSearchCommand { get; }
        public ICommand AddToCartCommand { get; }

        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();

        public SearchViewModel()
        {
            // ... other initializations
            SubmitSearchCommand = new Command(async () => await SubmitSearch());
            AddToCartCommand = new Command<Item>(AddToCart);

            /*AddToCartCommand = new Command<Item>(AddToCart);*/
        }

        private async Task SubmitSearch()
        {
            ItemService itemService = new ItemService();
            var items = await itemService.SearchItemsPartialName(SearchText);
            if (items != null)
            {
                Items.Clear();
                foreach (var item in items)
                {
                    Items.Add(item);
                }
                if (Items.Count == 0)
                {
                    await Application.Current.MainPage.DisplayToastAsync("No Items found.");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayToastAsync("No Items found.");
            }
        }

        async void AddToCart(Item item)
        {
            var databaseService = DependencyService.Get<DatabaseService>();
            var cartForStore = await databaseService.GetCartForStoreAsync(item.StoreId.ToString());

            if (cartForStore == null)
            {
                StoreService storeService = new StoreService();
                Store store = await storeService.GetStoreById(item.StoreId);
                var newCart = new Cart
                {
                    StoreId = item.StoreId.ToString(),
                    StoreName = store.Name,
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

    }
}
