using BestDealClient2.Data;
using BestDealClient2.iOS.Data;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(DatabaseConnection))]

namespace BestDealClient2.iOS.Data
{
    public class DatabaseConnection : IDatabaseConnection
    {
        public SQLiteAsyncConnection DbConnection()
        {
            var dbName = "YourAppDb.db3";
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(libraryPath, dbName);
            return new SQLiteAsyncConnection(path);
        }
    }
}

