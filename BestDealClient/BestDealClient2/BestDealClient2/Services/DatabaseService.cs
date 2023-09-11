using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
using BestDealClient2.Data;
using BestDealClient2.Models;
using System.Linq;

[assembly: Dependency(typeof(BestDealClient2.Services.DatabaseService))]

namespace BestDealClient2.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            _database = DependencyService.Get<IDatabaseConnection>().DbConnection();
            _database.CreateTableAsync<Cart>().Wait();
        }

        public Task<int> SaveCartAsync(Cart cart)
        {
            var existingCart = _database.Table<Cart>().Where(x => x.StoreId == cart.StoreId).FirstOrDefaultAsync().Result;

            if (existingCart == null)
            {
                return _database.InsertAsync(cart);
            }
            else
            {
                return _database.UpdateAsync(cart);
            }
        }

        public async Task DeleteItemFromCartAsync(Item item)
        {
            // Find the cart that contains the item
            var cart = await _database.Table<Cart>().Where(x => x.ItemIds.Contains(item.Id)).FirstOrDefaultAsync();
            if (cart != null)
            {
                // Split the ItemIds string into a list
                var itemIds = cart.ItemIds.Split(';').ToList();

                // Remove the item's ID
                itemIds.Remove(item.Id);

                //if no more items are in the cart the cart will be deleted
                if (itemIds.Count == 0)
                {
                    await DeleteCartAsync(cart);
                    return;
                }

                // Update the ItemIds string
                cart.ItemIds = string.Join(";", itemIds);

                // Decrease the quantity
                cart.Quantity--;

                // Update the cart in the database
                await _database.UpdateAsync(cart);
            }
        }


        public async Task DeleteCartAsync(Cart cart)
        {
            // Delete the cart
            await _database.DeleteAsync(cart);
        }

        public Task<List<Cart>> GetCartItemsAsync()
        {
            return _database.Table<Cart>().ToListAsync();
        }

        public async Task<Cart> GetCartForStoreAsync(string storeId)
        {
            var cart = await _database.Table<Cart>().Where(x => x.StoreId == storeId).FirstOrDefaultAsync();
            return cart;
        }

    }
}
