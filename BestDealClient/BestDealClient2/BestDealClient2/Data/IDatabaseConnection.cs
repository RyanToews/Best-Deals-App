using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BestDealClient2.Data
{
    public interface IDatabaseConnection
    {
        SQLiteAsyncConnection DbConnection();
    }

}
