using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using BestDealLib.Models;
using CoreApi2.Data;
using System.Collections.Generic;

public class GoogleMapsService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    // Radius to search in meters.
    // private int RADIUS = 25000; 

    public GoogleMapsService(string? apiKey)
    {
        _httpClient = new HttpClient();
        _apiKey = apiKey!;
    }
    
    /** This function gets the distance from a handheld device to a selected
    address, using Google Geolocation API. */
    public async Task<int> GetDistance(string origin, string destination)
    {
        int distanceValue = int.MaxValue;

        try
        {
            var response = await _httpClient.GetStringAsync(
            $"https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&origins={origin}&destinations={destination}&key={_apiKey}");

            var data = JObject.Parse(response);
            string status = data.SelectToken("rows[0].elements[0].status")!.Value<string>()!;
            if (status == "OK")
            {
                distanceValue = data.SelectToken("rows[0].elements[0].distance.value")!.Value<int>();
            }
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return distanceValue;
    }
    
    /** This function takes an input radius value and finds all stores within
    the radius, centered at the geolocation of the user's handheld device. */
    public async Task<List<StoreDistance>> GetDistancesAsync(double currentLatitude, double currentLongitude, ApplicationDbContext dbContext, int radius)
    {
        string origin = $"{currentLatitude},{currentLongitude}";
        List<StoreDistance> storeDistances = new List<StoreDistance>();
        
        
        
        foreach (var store in dbContext.Stores!)
        {
            int distance = await GetDistance(origin, store.Address + " " + store.PostalCode);
            if (distance <= radius)
            {
                storeDistances.Add(new StoreDistance(store.Id, distance));
            }
        }

        return storeDistances;
    }
}