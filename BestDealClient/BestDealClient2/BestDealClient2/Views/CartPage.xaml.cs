using BestDealClient2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BestDealClient2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage : ContentPage
    {
        public CartPage()
        {
            InitializeComponent();
            this.BindingContext = new CartViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = BindingContext as CartViewModel;
            if (viewModel != null)
            {
                await viewModel.RefreshItems();
            }
        }
    }
}