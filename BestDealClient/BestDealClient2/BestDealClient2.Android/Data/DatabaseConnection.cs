using BestDealClient2.Data;
using SQLite;
using BestDealClient2.Droid.Data;
using Xamarin.Forms;

[assembly: Dependency(typeof(DatabaseConnection))]

namespace BestDealClient2.Droid.Data
{
    public class DatabaseConnection : IDatabaseConnection
    {
        public SQLiteAsyncConnection DbConnection()
        {
            var dbName = "YourAppDb.db3";
            var path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbName);
            return new SQLiteAsyncConnection(path);
        }
    }
}
