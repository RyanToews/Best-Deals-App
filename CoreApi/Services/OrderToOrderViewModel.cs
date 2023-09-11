using BestDealLib.Models;
using CoreApi2.Data;

namespace CoreApi2.Services
{
    public static class OrderToOrderViewModel
    {
        public static List<OrderViewModel> OrdertoOrderViewModel(List<Order> orders, ApplicationDbContext dbContext)
        {
            List<OrderViewModel> viewModels = new List<OrderViewModel>();
            foreach (var order in orders)
            {
                Store? store = dbContext.Stores!.FirstOrDefault(store => store.Id == order.StoreId);
                OrderViewModel viewModel = new OrderViewModel();

                viewModel.Id = order.OrderId;
                viewModel.Number = order.OrderNum;
                viewModel.IsPaid = order.IsPaid;
                viewModel.ForDelivery = order.ForDelivery;
                viewModel.OrderType = order.OrderType;
                viewModel.CustomId = order.CustomId;
                viewModel.StoreId = order.StoreId;
                viewModel.LastFourDigits = order.LastFourDigits;
                viewModel.OrderDate = order.OrderDate;     // Added
                viewModel.TotalCharge = order.TotalCharge; // Added
                viewModel.StoreAddress = store!.Address + ", " + store.City + ", " + store.ProvinceState;

                if (order.OrderItems != null)
                {
                    viewModel.Products = new List<ProductViewModel>();
                    foreach (var item in order.OrderItems)
                    {
                        ProductViewModel productViewModel = new ProductViewModel()
                        {
                            Id = item.ItemId,
                            Name = item.Item?.Name,
                            Price = item.PriceAtSale.ToString(),
                            Quantity = item.QuantityOrdered
                        };
                        viewModel.Products.Add(productViewModel);
                    }
                }

                viewModels.Add(viewModel);
            }

            return viewModels;
        }
    }
}
