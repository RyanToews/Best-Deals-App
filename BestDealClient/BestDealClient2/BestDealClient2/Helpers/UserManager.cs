using BestDealClient2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Extensions;

namespace BestDealClient2.Helpers
{
    internal static class UserManager
    {

        static HttpClientHandler handler;
        static HttpClient httpClient;

        private static UserInfo _currentUserInfo = new UserInfo();
        public static Order lastOrder = new Order();

        public static UserInfo CurrentUserInfo
        {
            get { return _currentUserInfo; }
            set
            {
                if (_currentUserInfo != value)
                {
                    _currentUserInfo = value;
                    CurrentUserInfoChanged?.Invoke(null, EventArgs.Empty);
                }
            }
        }

        public static event EventHandler CurrentUserInfoChanged;


        static UserManager()
        {
            handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            httpClient = new HttpClient(handler);
        }

        
        /// <summary>
        /// Gets the user info given a token, and returns it.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<UserInfo> GetUserInfo(string token)
        {
            TokenRequest tokenRequestModel = new TokenRequest();
            tokenRequestModel.Token = token;

            var json = JsonConvert.SerializeObject(tokenRequestModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            UserInfo userInfo = new UserInfo();
            try
            {
                var response = await httpClient.PostAsync($"{BaseAPIHelper.BaseServerUrl}/api/user-info", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
                    userInfo.Username = data["username"];
                    userInfo.Firstname = data["firstname"];
                    userInfo.Lastname = data["lastname"];
                    userInfo.Email = data["email"];
                    userInfo.Address = data["address"];
                    userInfo.City = data["city"];
                    userInfo.PostalCode = data["postalCode"];
                    userInfo.Province = data["province"];
                    userInfo.Phone = data["phone"];
                    return userInfo;
                }
                else
                {
                    // Handle any errors here.
                    return null;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return null;
        }


        /// <summary>
        /// Tries to Login, vertify the token, and then returns a bool depending on whether it was successfull.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<bool> LoginAsync(Login model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await httpClient.PostAsync($"{BaseAPIHelper.BaseServerUrl}/api/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
                    if (await CheckTokenValidity(data["token"]))
                    {
                        //set the auth token and merrco token.
                        await SecureStorage.SetAsync("oauth_token", data["token"]);
                        await SecureStorage.SetAsync("merrcoToken", await BaseAPIHelper.getMerrcoToken());

                        CurrentUserInfo = await GetUserInfo(data["token"]);
                        return true;
                    }
                    await Application.Current.MainPage.DisplayToastAsync("Wrong username or password");
                    return false;
                }
                else
                {
                    // Handle any errors here.
                    await Application.Current.MainPage.DisplayToastAsync("Wrong username or password");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            await Application.Current.MainPage.DisplayToastAsync("Error logging in");
            return false;
        }


        /// <summary>
        /// Vertifies the validity of a token and outputs a bool depending on if it is valid. 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        internal static async Task<bool> CheckTokenValidity(string token)
        {
            if (token == null)
            {
                return false;
            }
            
            try
            {
                TokenVerification model = new TokenVerification();
                model.Token = token;
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync($"{BaseAPIHelper.BaseServerUrl}/api/verify-token", content);
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Dictionary<string, bool>>(jsonResponse);
                if (!data["valid"])
                {
                    SecureStorage.Remove("oauth_token");
                }
                CurrentUserInfo = await GetUserInfo(token);
                return data["valid"];
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return false;
        }


        /// <summary>
        /// Creates the user account based on the RegisterModel given.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<string> SignupAsync(Register model)
        {
            var json = JsonConvert.SerializeObject(model);
            if (model.Username == null || model.Username == "")
            {
                return "Username empty.";
            }
            else if (model.Password == null || model.Password == "")
            {
                return "Password empty.";
            } 
            else if (model.Password != model.ConfirmPassword)
            {
                return "Passwords don't match.";
            }
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            string response = await SignupAsyncHelper(content);

            return response;
        }


        /// <summary>
        /// Creates the user account based on the RegisterModel given.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private static async Task<string> SignupAsyncHelper(StringContent content)
        {
            try
            {
                var response = await httpClient.PostAsync($"{BaseAPIHelper.BaseServerUrl}/api/register", content);

                if (response.IsSuccessStatusCode)
                {
                    return "success";
                }
                else
                {
                    // Handle any errors here.
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return errorMessage;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            
        }
    }
}
