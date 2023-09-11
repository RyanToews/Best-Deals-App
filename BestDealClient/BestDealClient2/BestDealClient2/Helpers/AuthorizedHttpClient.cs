using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Extensions;

namespace BestDealClient2.Helpers
{
    public class AuthorizedHttpClient
    {
        private static HttpClient _client;

        public static async Task<HttpClient> GetAPIAuthorizedClient()
        {
                HttpClientHandler handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                _client = new HttpClient(handler);
                string token = await SecureStorage.GetAsync("oauth_token");
                if (!string.IsNullOrWhiteSpace(token))
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                } else
                {
                    await Application.Current.MainPage.DisplayToastAsync("Could not find auth token");
                }
            return _client;
        }
    }
}
