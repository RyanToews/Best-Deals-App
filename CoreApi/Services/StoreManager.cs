using BestDealLib.Models;
using System.Net;
using System.Xml.Linq;

namespace CoreApi2.Services
{
    public class StoreManager
    {

        public static Store UpdateFrom(Store other)
        {
            Store temp = new Store();
            temp.BaseEndpoint = other.BaseEndpoint;
            temp.StoreNumber = other.StoreNumber;
            temp.Address = other.Address;
            temp.City = other.City;
            temp.ProvinceState = other.ProvinceState;
            temp.PostalCode = other.PostalCode;
            temp.Name = other.Name;
            return temp;
        }
    }
}
