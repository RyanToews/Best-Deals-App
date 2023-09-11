using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestDealLib.Models;
using CoreApi2.Data;
using CoreApi2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CoreApi2
{
    public class DatabaseUpdater
    {

        /// <summary>
        /// Fetches a list of stores from a specified API, adds new stores to the database, updates existing stores,
        /// and removes any stores from the database not present in the API's list.
        /// </summary>
        /// <param name="baseUrl">The base URL of the API.</param>
        /// <param name="authKey">The authorization key for the API.</param>
        /// <param name="authValue">The value of the authorization key.</param>
        /// <param name="dbContext">The application's DbContext.</param>
        /// <returns>The number of changes made to the database.</returns>
        public static async Task<int> FetchListUpdateStores(string baseUrl, string authKey, string authValue, ApplicationDbContext dbContext)
        {
            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(authKey, authValue);
            try
            {
                string url = baseUrl + "/locations";
                var jsonResponseMessage = await httpClient.GetAsync(url);
                string jsonResponse = await jsonResponseMessage.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<StoreResponse>(jsonResponse);

                if (response?.Data != null)
                {
                    List<int?> fetchedStoreNumbers = new List<int?>();

                    foreach (InputStore inputStore in response.Data)
                    {
                        Store store = new Store(inputStore, baseUrl);
                        fetchedStoreNumbers.Add(store.StoreNumber);

                        var existingStore = dbContext.Stores?.FirstOrDefault(s => s.StoreNumber == store.StoreNumber && s.BaseEndpoint == store.BaseEndpoint);
                        if (existingStore == null)
                        {
                            dbContext.Stores?.Add(store);
                        }
                        else
                        {
                            existingStore = StoreManager.UpdateFrom(store);
                        }
                    }

                    dbContext.Stores?.RemoveRange(dbContext.Stores.Where(s => !fetchedStoreNumbers.Contains(s.StoreNumber)));

                    return await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return 0;
        }

        /// <summary>
        /// Fetches a list of items from a specified API for a specific store, adds new items to the database, 
        /// updates existing items, and removes any items from the database for that store not present in the API's list.
        /// </summary>
        /// <param name="url">The URL of the API's endpoint for fetching items.</param>
        /// <param name="authKey">The authorization key for the API.</param>
        /// <param name="authValue">The value of the authorization key.</param>
        /// <param name="storeID">The ID of the store for which to fetch items.</param>
        /// <param name="dbContext">The application's DbContext.</param>
        /// <returns>The number of changes made to the database.</returns>
        public static async Task<int> FetchListUpdateItems(string url, string authKey, string authValue, int storeID, ApplicationDbContext dbContext)
        {
            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(authKey, authValue);
            try
            {
                var jsonResponseMessage = await httpClient.GetAsync(url);
                string jsonResponse = await jsonResponseMessage.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<Response>(jsonResponse);

                if (response?.Data != null)
                {
                    List<string> fetchedItemIds = new List<string>();

                    foreach (var inputItemWrapper in response.Data.Items!)
                    {
                        if (inputItemWrapper.InputItem != null)
                        {
                            Item item = new Item(inputItemWrapper.InputItem, storeID);
                            fetchedItemIds.Add(item.Id);

                            var existingItem = dbContext.Items?.FirstOrDefault(i => i.Id == item.Id && i.StoreId == item.StoreId);
                            if (existingItem == null)
                            {
                                dbContext.Items?.Add(item);
                            }
                            else
                            {
                                existingItem = ItemManager.UpdateFrom(item);
                            }
                        }
                    }

                    dbContext.Items?.RemoveRange(dbContext.Items.Where(i => i.StoreId == storeID && !fetchedItemIds.Contains(i.Id!)));

                    return await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return 0;
        }

    }
}
