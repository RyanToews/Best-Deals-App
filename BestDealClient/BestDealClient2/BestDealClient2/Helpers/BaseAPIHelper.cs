

using BestDealClient2.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BestDealClient2.Helpers
{
    public static class BaseAPIHelper
    {
        /// <summary>
        /// Base URL for the API server.
        /// </summary>
        public static string BaseServerUrl { get; } = "https://bestdealcoreapi.azurewebsites.net";


        /// <summary>
        /// Method to get Merrco Token from the API.
        /// It initiates an HTTP client, makes a GET request to the specific API endpoint, 
        /// and retrieves the token in the response content.
        /// </summary>
        /// <returns>
        /// Returns Merrco Token as string if successful, 
        /// otherwise returns an empty string.
        /// </returns>
        public static async Task<string> getMerrcoToken()
        {
            using (HttpClient httpClient = await AuthorizedHttpClient.GetAPIAuthorizedClient())
            {
                try
                {
                    string url = $"{BaseAPIHelper.BaseServerUrl}/api/Order/merrcoToken";
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string token = await response.Content.ReadAsStringAsync();
                        return token;
                    }
                }
                catch (Exception e)
                {
                    string msg = e.Message;
                }
            }
            return "";
        }
    }
}
