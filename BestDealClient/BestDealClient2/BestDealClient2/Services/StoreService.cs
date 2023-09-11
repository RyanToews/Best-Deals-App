using BestDealClient2.Helpers;
using BestDealClient2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BestDealClient2.Services
{
    public class StoreService
    {
        public async Task<Store> GetStoreById(int storeId)
        {
            HttpClient httpClient = await AuthorizedHttpClient.GetAPIAuthorizedClient();

            var response = await httpClient.GetAsync($"{BaseAPIHelper.BaseServerUrl}/api/Stores/{storeId}");
            if (response.IsSuccessStatusCode)
            {
                var storeJson = await response.Content.ReadAsStringAsync();
                Store store = JsonConvert.DeserializeObject<Store>(storeJson);
                return store;
            }
            return null;
        }

        public async Task<List<StoreDistancePair>> GetStoresSortedByDistance(double longitude, double latitude, double radius)
        {

            using (HttpClient httpClient = await AuthorizedHttpClient.GetAPIAuthorizedClient())
            {
                try
                {
                    var response = await httpClient.GetAsync($"{BaseAPIHelper.BaseServerUrl}/{latitude},{longitude};{radius}");
                    if (response.IsSuccessStatusCode)
                    {
                        var distancesJson = await response.Content.ReadAsStringAsync();
                        var storeDistancesList = JsonConvert.DeserializeObject<List<StoreDistance>>(distancesJson);
                        var storeDistances = new List<StoreDistancePair>();
                        foreach (var storeDistance in storeDistancesList)
                        {
                            var storeJson = await response.Content.ReadAsStringAsync();
                            var store = await GetStoreById(storeDistance.StoreID);
                            storeDistances.Add(new StoreDistancePair { Store = store, Distance = storeDistance.Distance });
                        }
                        return storeDistances;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            return null;
        }

        public async Task<List<Store>> GetStoresByIds(List<int> storeIds)
        {
            HttpClient httpClient = await AuthorizedHttpClient.GetAPIAuthorizedClient();

            // Convert the list of store ids to a semicolon-separated string.
            string idsParam = string.Join(";", storeIds);

            var response = await httpClient.GetAsync($"{BaseAPIHelper.BaseServerUrl}/stores/{idsParam}");
            if (response.IsSuccessStatusCode)
            {
                var storesJson = await response.Content.ReadAsStringAsync();

                // Attempt to deserialize the JSON as a list of stores
                List<Store> stores;
                try
                {
                    stores = JsonConvert.DeserializeObject<List<Store>>(storesJson);
                }
                catch (JsonSerializationException)
                {
                    // If deserialization as a list failed, try to deserialize it as a single store
                    Store singleStore = JsonConvert.DeserializeObject<Store>(storesJson);
                    stores = new List<Store> { singleStore };
                }

                return stores;
            }
            return new List<Store>();
        }
    }
}
