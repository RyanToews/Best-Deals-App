using BestDealClient2.Helpers;
using BestDealClient2.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace BestDealClient2.ViewModels
{
    public class OrderHistoryViewModel : BaseViewModel
    {
        public ObservableCollection<Order> Orders { get; set; }
        public OrderHistoryViewModel()
        {
            Title = "Order History";
            Orders = new ObservableCollection<Order>();
        }
        internal async Task LoadOrders()
        {
            Orders.Clear();
            OrderService orderService = new OrderService();
            var orders = await orderService.GetOrdersByUsername(UserManager.CurrentUserInfo.Username);

            // Call our new static method
            if (orders == null || orders.Count == 0)
            {
                await Application.Current.MainPage.DisplayToastAsync("No orders found.");
            } else
            {
                // Populate the Orders collection
                foreach (var order in orders)
                {
                    Orders.Add(order);
                }
            }
        }

    }
}
