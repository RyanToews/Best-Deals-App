using BestDealLib;
using CoreApi2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BestDealLib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CoreApi2.Services
{
    public class UpdateScheduler : BackgroundService
    {
        private readonly ApplicationDbContext _dbContext;

        public List<(string, string, string)> BaseAPIs { get; set; }

        public UpdateScheduler(ApplicationDbContext dbContext, List<(string baseUrl, string authKey, string authValue)> inputBaseUrls)
        {
            _dbContext = dbContext;
            BaseAPIs = inputBaseUrls;
        }



        // Seems to work, but I cannot get rid of a SQL error
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    foreach ((string baseUrl, string authKey, string authValue) baseApi in BaseAPIs)
                    {
                        //baseURL for allowing the use of different APIs provided.
                        var stores = await DatabaseUpdater.FetchListUpdateStores(baseApi.baseUrl, baseApi.authKey, baseApi.authValue, _dbContext);

                        for (int i = 0; i < _dbContext.Stores?.Count(); i++)
                        {
                            Store store = _dbContext.Stores.ToArray()[i];
                            if (store.BaseEndpoint == baseApi.baseUrl)
                            {
                                Console.WriteLine(store.Id);
                                var items = await DatabaseUpdater.FetchListUpdateItems(baseApi.baseUrl + "/menu/" + store.StoreNumber, baseApi.authKey, baseApi.authValue, store.Id, _dbContext);
                            }
                        }
                    }
                    Console.WriteLine($"Database updated at {DateTime.Now}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating database: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}
