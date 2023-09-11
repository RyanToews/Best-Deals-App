using BestDealClient2.Helpers;
using BestDealClient2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Extensions;
using System.Threading.Tasks;

namespace BestDealClient2.Services
{
    public class ItemService
    {
        public async Task<List<Item>> SearchItemsPartialName(string searchText)
        {

            // Use HttpClient with the custom HttpMessageHandler
            using (HttpClient httpClient = await AuthorizedHttpClient.GetAPIAuthorizedClient())
            {
                try
                {
                    string url = $"{BaseAPIHelper.BaseServerUrl}/searchitem/{searchText}";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonData = await response.Content.ReadAsStringAsync();
                        List<Item> items = JsonConvert.DeserializeObject<List<Item>>(jsonData);
                        return items;
                    }
                }
                catch (Exception e)
                {
                    string msg = e.Message;
                }
            }
            return new List<Item>();
        }

        public async Task<List<Item>> SearchItemsStoreIdPartialName(int storeId, string searchText)
        {

            // Use HttpClient with the custom HttpMessageHandler
            using (HttpClient httpClient = await AuthorizedHttpClient.GetAPIAuthorizedClient())
            {
                try
                {
                    string url = $"{BaseAPIHelper.BaseServerUrl}/{storeId}/{searchText}";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonData = await response.Content.ReadAsStringAsync();
                        List<Item> items = JsonConvert.DeserializeObject<List<Item>>(jsonData);
                        return items;
                    }
                }
                catch (Exception e)
                {
                    string msg = e.Message;
                }
            }
            return new List<Item>();
        }

        public async Task<List<Item>> FindItemsStoreIdItemIds(string storeId, string itemIds)
        {

            using (HttpClient httpClient = await AuthorizedHttpClient.GetAPIAuthorizedClient())
            {
                try
                {
                    string itemIdsFormated = string.Join(";",itemIds);
                    string url = $"{BaseAPIHelper.BaseServerUrl}/items/{storeId}/{itemIdsFormated}";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonData = await response.Content.ReadAsStringAsync();
                        var items = JsonConvert.DeserializeObject<List<Item>>(jsonData);
                        return items;
                    }
                }
                catch (Exception e)
                {
                    await Application.Current.MainPage.DisplayToastAsync("Something went wrong");
                }
            }
            return new List<Item>();
        }
    }
}
