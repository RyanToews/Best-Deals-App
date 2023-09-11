using BestDealClient2.Helpers;
using BestDealClient2.Models;
using BestDealClient2.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BestDealClient2.Services
{

    public class OrderService
    {
        private static MerrcoResponse merrcoRes;

        static OrderService()
        {
            merrcoRes = new MerrcoResponse();
        }
        public async Task<List<Order>> GetOrdersByUsername(string username)
        {
            using (HttpClient httpClient = await AuthorizedHttpClient.GetAPIAuthorizedClient())
            {
                string url = $"{BaseAPIHelper.BaseServerUrl}/api/Order/order/{username}";

                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonData = await response.Content.ReadAsStringAsync();
                    var orders = JsonConvert.DeserializeObject<List<Order>>(jsonData);
                    return orders;
                }
            }
            return new List<Order>();
        }


        /// <summary>
        /// Handles the ordering process. If successful, it empties the current cart and navigates to the OrderReceivedPage.
        /// </summary>
        /// <param name="online">A boolean value indicating whether the order is processed online.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal static async Task<string> Order()
        {
            string orderRes = await ProcessOrder(false);
            if (orderRes == "success")
            {
                var databaseService = DependencyService.Get<DatabaseService>();
                await databaseService.DeleteCartAsync(CartService.CurrentCart);
            }
            return orderRes;
        }


        /// <summary>
        /// Handles the ordering process. It starts by executing a transaction, then, if successful, processes the order.
        /// </summary>
        /// <param name="merrcoResponse">Response object containing details of the payment transaction.</param>
        /// <param name="online">A boolean value indicating whether the order is processed online.</param>
        /// <param name="url">The endpoint URL for the transaction.</param>
        /// <param name="expiryMonth">The expiry month of the credit card.</param>
        /// <param name="expiryYear">The expiry year of the credit card.</param>
        /// <param name="creditCardNumber">The credit card number.</param>
        /// <param name="ccv">The Card Verification Value (CVV) of the credit card.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the status of the transaction and order processing.</returns>
        internal static async Task<string> Order(string url, string expiryMonth, string expiryYear, string creditCardNumber, string ccv)
        {
            string transactionRes = await Transact(url, expiryMonth, expiryYear, creditCardNumber, ccv);
            if (transactionRes == "approved")
            {
                string orderRes = await ProcessOrder(true);
                if (orderRes == "success")
                {
                    var databaseService = DependencyService.Get<DatabaseService>();
                    await databaseService.DeleteCartAsync(CartService.CurrentCart);
                    return "success";
                }
            }
            return transactionRes;
        }


        /// <summary>
        /// Processes a transaction by sending the payment data to the specified URL.
        /// </summary>
        /// <param name="url">The endpoint URL for the transaction.</param>
        /// <param name="expiryMonth">The expiry month of the credit card.</param>
        /// <param name="expiryYear">The expiry year of the credit card.</param>
        /// <param name="creditCardNumber">The credit card number.</param>
        /// <param name="ccv">The Card Verification Value (CVV) of the credit card.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the status of the transaction.</returns>
        internal static async Task<string> Transact(string url, string expiryMonth, string expiryYear, string creditCardNumber, string ccv)
        {
            string result = "Unknown error";

            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("merrcoToken"));

                var paymentDataModel = new PaymentData
                {
                    Amount = CartService.CurrentCart.TotalPrice,
                    Currency = "CAD",
                    CardExpiryMonth = int.Parse(expiryMonth),
                    CardExpiryYear = int.Parse(expiryYear),
                    CardNumber = creditCardNumber.Trim(),
                    CVV2 = int.Parse(ccv),
                    Email = UserManager.CurrentUserInfo.Email
                };

                var jsonData = JsonConvert.SerializeObject(paymentDataModel);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);
                string responseData = await response.Content.ReadAsStringAsync();
                if (responseData.Contains("transaction_success"))
                {
                    var apiResponse = JsonConvert.DeserializeObject<MerrcoResponse>(responseData);
                    // Handle successful transaction
                    merrcoRes = apiResponse;
                    result = apiResponse.TransactionResult.ToLower();
                }
                else if (responseData.Contains("status"))
                {
                    var apiResponse = JsonConvert.DeserializeObject<MerrcoErrorResponse>(responseData);
                    // Handle error response
                    if (apiResponse.Status != "APPROVED")
                    {
                        result = apiResponse.Errors[0].Message;
                    }
                }

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }


        /// <summary>
        /// Processes an order either online or expressCheckout, then calls the ProcessOrderApi method.
        /// </summary>
        /// <param name="online">A boolean value indicating whether the order is processed online.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the status message of the order processing.</returns>
        internal static async Task<string> ProcessOrder(bool online)
        {
            List<Product> products = new List<Product>();
            foreach (Item item in CartService.CurrentCart.Items)
            {
                Product product = new Product();
                product.Name = item.Name;
                product.Price = item.Price;
                product.Quantity = 1;
                product.Id = item.Id;
                products.Add(product);
            }

            Order Order = new Order
            {
                Id = 1,
                Username = UserManager.CurrentUserInfo.Username,
                StoreId = int.Parse(CartService.CurrentCart.StoreId),
                Number = 1,
                IsPaid = online,
                ForDelivery = false,
                OrderType = online ? "online" : "expressCheckout",
                CustomId = "DRO",
                Products = products,
                LastFourDigits = online ? merrcoRes.CardSuffix : "",
                Customer = UserManager.CurrentUserInfo,
                TransactionId = online ? merrcoRes.TransactionId : "",
            };


            var result = await ProcessOrderApi(Order);

            return result;
        }

        /// <summary>
        /// Asynchronously processes an order by sending it to the server using HTTP.
        /// </summary>
        /// <param name="order">The order to be processed.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the status message of the order processing.</returns>
        private static async Task<string> ProcessOrderApi(Order order)
        {
            HttpClient client = await AuthorizedHttpClient.GetAPIAuthorizedClient();
            var json = JsonConvert.SerializeObject(order);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await client.PostAsync($"{BaseAPIHelper.BaseServerUrl}/api/Order/order", content);

                if (response.IsSuccessStatusCode)
                {
                    var orderId = await response.Content.ReadAsStringAsync();
                    UserManager.lastOrder = order;
                    UserManager.lastOrder.Number = int.Parse(orderId);
                    UserManager.lastOrder.Id = int.Parse(orderId);
                    return "success";
                }
                else
                {
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
