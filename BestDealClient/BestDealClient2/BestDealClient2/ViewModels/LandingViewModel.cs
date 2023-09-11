using BestDealClient2.Helpers;
using BestDealClient2.Models;
using BestDealClient2.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BestDealClient2.ViewModels
{
    /**
     * Author: Ashkan Zahedanaraki
     * 
     * LandingViewModel class, part of BestDealClient2 application.
     * Handles operations related to retrieving and displaying store and their distances from user/phone.
     */
    public class LandingViewModel : BaseViewModel
    {
        /**
         * ICommand used to update the list of stores
         * based on the radius entered by the user.
         */
        public ICommand UpdateStoresCommand { get; set; }

        private List<StoreDistancePair> _sortedStores;

        //Default Radius should be 25km
        private string _radius = "25";

        /**
         * SortedStores property
         * Represents a sorted list of StoreDistancePair objects.
         */
        public List<StoreDistancePair> SortedStores
        {
            get { return _sortedStores; }
            set
            {
                _sortedStores = value;
                OnPropertyChanged();
            }
        }
        /**
         * Radius property
         * Represents the radius (in kilometers) for the store search.
         */
        public string Radius
        {
            get => _radius;
            set
            {
                if (_radius != value)
                {
                    _radius = value;
                    OnPropertyChanged();
                }
            }
        }

        /**
         * Constructor of the LandingViewModel.
         * Sets the Title property and the UpdateStoresCommand ICommand.
         * Also calls the UpdateStoresBasedOnRadius method.
         */
        public LandingViewModel()
        {

            Title = "Nearby Stores";
            UpdateStoresCommand = new Command(UpdateStoresBasedOnRadius);
            UpdateStoresBasedOnRadius();
        }

        /**
         * Method to update the SortedStores property based on the Radius property.
         */
        private void UpdateStoresBasedOnRadius()
        {
            StoreService storeService = new StoreService();
            Device.BeginInvokeOnMainThread(async () =>
            {
                int.TryParse(Radius, out int radius);
                var location = await GetDeviceLocationAsync();
                if (location != null)
                {
                    SortedStores = await storeService.GetStoresSortedByDistance(location.Longitude, location.Latitude, radius);
                }
                else
                {
                    await Application.Current.MainPage.DisplayToastAsync("Device location not received");
                }

                if (SortedStores == null || SortedStores.Count == 0)
                {
                    await Application.Current.MainPage.DisplayToastAsync("No Stores Found");
                }
            });
        }

        /**
         * Method to get the current device location.
         */
        private async Task<Location> GetDeviceLocationAsync()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                    location = await Geolocation.GetLocationAsync(request);
                }

                return location;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }

            return null;
        }
    }
}
