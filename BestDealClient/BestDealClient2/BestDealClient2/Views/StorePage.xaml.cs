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
	public partial class StorePage : ContentPage
	{
        public StorePage(int storeId)
        {
            InitializeComponent();
            BindingContext = new StoreViewModel(storeId);
        }

    }
}