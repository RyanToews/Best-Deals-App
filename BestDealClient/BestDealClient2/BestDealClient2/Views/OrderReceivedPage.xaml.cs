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
    public partial class OrderReceivedPage : ContentPage
    {
        public OrderReceivedPage()
        {
            InitializeComponent();
            this.BindingContext = new OrderReceivedViewModel();
        }
    }
}