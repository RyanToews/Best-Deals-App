using BestDealClient2.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DatabaseHelper
{
    readonly SQLiteAsyncConnection _database;

    public DatabaseHelper(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<Cart>().Wait();
    }

    public Task<List<Cart>> GetItemsAsync()
    {
        return _database.Table<Cart>().ToListAsync();
    }

    public Task<int> SaveItemAsync(Cart item)
    {
        return _database.InsertAsync(item);
    }

    // Other database operations like Delete, Update, etc.
}
