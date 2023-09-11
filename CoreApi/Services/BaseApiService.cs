using BestDealLib.Models;

namespace CoreApi2.Services
{
    /** This class contains methods to access API Keys. */
    public static class BaseApiService
    {
        //Base APIs with both Base URL and key for the URL. 
        public static List<(string, string, string)> baseAPIs = new List<(string baseUrl, string authKey, string authValue)>();

        static BaseApiService() {
            baseAPIs = KeyService.GetTechPOS();
        }


        public static HttpClient GetAuthHttpClient(Store store)
        {
            var httpClient = new HttpClient();
            for (int i = 0; i < baseAPIs.Count; i++)
            {
                if (store!.BaseEndpoint == baseAPIs[i].Item1)
                {
                    // Set the header value (need to be able to set this based on store)
                    httpClient.DefaultRequestHeaders.Add(baseAPIs[i].Item2, baseAPIs[i].Item3);
                    return httpClient;
                }
            }
            return new HttpClient();
        }
    }
}
