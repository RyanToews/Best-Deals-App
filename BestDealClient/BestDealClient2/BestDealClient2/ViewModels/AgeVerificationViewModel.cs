using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BestDealClient2.ViewModels
{
    class AgeVerificationViewModel : BaseViewModel
    {
        public Command AgeVerificationCommand { get; }
        public AgeVerificationViewModel()
        {
            Title = "Age Verification";
            AgeVerificationCommand = new Command(OnAgeVerificationClicked);
        }

        private async void OnAgeVerificationClicked(object obj)
        {
            await SecureStorage.SetAsync("age_verified", "true");
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
