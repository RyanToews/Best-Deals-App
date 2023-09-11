using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BestDealLib.Models;
using CoreApi2.Data;
using CoreApi2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreApi2.Controllers
{
    /// <summary>
    /// OrderController.
    /// This class contains the methods available to the API that enable
    /// creation and access to order data.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [EnableCors("Policy")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        // Database context object
        private readonly ApplicationDbContext _dbContext;

        // Constructor for the class
        public OrderController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// CreateOrder(). This method takes the data from the OrderViewModel,
        /// creates a new order and persists it in the database. It then
        /// formats the data and sends it to a remote order entry API.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("order")]
        public async Task<ActionResult<int>> CreateOrder([FromBody] OrderViewModel model)
        {
            // Fetch the user based on the provided username
            var user = await _dbContext.Users!.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            // Create a new Order entity
            var order = new Order
            {
                OrderId = GenerateEightDigitId(1),
                OrderDate = DateTime.Now,
                StoreId = model.StoreId,
                IsPaid = model.IsPaid ? true : false,
                ForDelivery = model.ForDelivery ? true : false,
                OrderType = model.OrderType,
                CustomId = "DRO",
                LastFourDigits = model.LastFourDigits,
                TransactionId = model.TransactionId,
                TotalCharge = 0
            };

            // Save the order to the database
            _dbContext.Orders!.Add(order);
            await _dbContext.SaveChangesAsync();

            // Assign the order ID to OrderNum
            order.OrderNum = order.OrderId;

            // Add the order to the user's Orders list
            user.Orders ??= new List<Order>();
            user.Orders.Add(order);

            // Save the changes to the database
            await _dbContext.SaveChangesAsync();

            // Add the order items
            if (model.Products != null)
            {
                foreach (var product in model.Products)
                {
                    // Convert the price string to decimal
                    if (!decimal.TryParse(product.Price, out decimal price))
                    {
                        // Handle conversion failure if necessary
                        return BadRequest("Invalid price format");
                    }

                    // Check if the item exists in the database first
                    var item = await _dbContext.Items!.FindAsync(new object[] { product.Id, order.StoreId });
                    if (item == null)
                    {
                        return BadRequest($"Item with ID '{product.Id}' in store '{order.StoreId}' not found");
                    }

                    var orderItem = new OrderItem
                    {
                        OrderId = order.OrderId,
                        ItemId = product.Id,
                        ItemStoreId = order.StoreId,
                        PriceAtSale = price,
                        QuantityOrdered = product.Quantity
                    };

                    // Save the order item to the database
                    _dbContext.OrderItems!.Add(orderItem);
                }
            }

            await _dbContext.SaveChangesAsync();

            // Calculate the total charge
            order.TotalCharge = _dbContext.OrderItems!
                .Where(item => item.OrderId == order.OrderId)
                .Sum(item => item.PriceAtSale * item.QuantityOrdered);

            _dbContext.SaveChanges();

            // Retrieve the store information from the database based on StoreId
            var store = await _dbContext.Stores!.FirstOrDefaultAsync(s => s.Id == model.StoreId);
            if (store == null)
            {
                return BadRequest("Store not found");
            }

            // Construct the customer object
            var customer = new
            {
                firstName = user.Firstname,
                lastName = user.Lastname,
                email = user.Email,
                phone = user.Phone,
                address = user.Address,
                addressTwo = "",
                city = user.City,
                state = user.Province,
                postalCode = user.PostalCode
            };

            // Construct the products array
            var products = model.Products?.Select(product => new
            {
                id = product.Id,
                name = product.Name,
                quantity = product.Quantity
            });

            // Construct the order object
            var formattedOrder = new
            {
                id = order.OrderId,
                number = order.OrderNum,
                isPaid = order.IsPaid,
                forDelivery = order.ForDelivery,
                order_type = order.OrderType,
                customId = order.CustomId,
                products = products,
                lastFourDigits = order.LastFourDigits,
                customer = customer
            };

            // Construct the API URL
            var apiUrl = store.BaseEndpoint + "/order/" + store.StoreNumber;

            var httpClient = BaseApiService.GetAuthHttpClient(order.Store!);

            var orderJson = JsonSerializer.Serialize(formattedOrder);
            var content = new StringContent(orderJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(apiUrl, content);
            if (!response.IsSuccessStatusCode)
            {
                // Handle API error if necessary
                return BadRequest("Failed to send order to API");
            }

            return Ok(order.OrderId);
        }

        /// <summary>
        /// GenerateEightDigitId(). This method creates a unique 8-digit, from 2
        /// digits of the year with 6 random digits. If the number exists in the
        /// database, it recursively calls itself again until a unique order
        /// number is found.
        /// </summary>
        /// <param name="existingOrderId"></param>
        /// <returns></returns>
        private int GenerateEightDigitId(int existingOrderId)
        {
            // Take the last two digits of the year
            int year = DateTime.Now.Year % 100;

            Random random = new Random();
            int randomNum = random.Next(1000, 9999);

            int orderNumber = (year * 1000000) + randomNum;

            // Check if the generated ID already exists in the database
            if (_dbContext.Orders!.Any(o => o.OrderId == orderNumber))
            {
                // If the generated ID already exists, recursively call the method to generate a new ID
                return GenerateEightDigitId(existingOrderId);
            }

            // If the generated ID doesn't exist, return it
            return orderNumber;
        }

        /// <summary>
        /// GetOrdersByUsername(). This method gets all orders in the database
        /// for a given username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("order/{username}")]
        public IActionResult GetOrdersByUsername(string username)
        {
            // Find the user based on the provided username
            var user = _dbContext.Users!.Include(u => u.Orders).FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Retrieve all orders associated with the user
            var orders = user.Orders;
            var orderViewModels = OrderToOrderViewModel.OrdertoOrderViewModel(orders!, _dbContext);
            // Return the orders
            return Ok(orderViewModels);
        }

        /// <summary>
        /// Gets the merrco token for payment.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("merrcoToken")]
        public ActionResult<string> GetMerrcoToken()
        {
            IConfiguration configuration;
            configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
            return configuration["MerrcoApiKey"]!;
        }

    }
}
