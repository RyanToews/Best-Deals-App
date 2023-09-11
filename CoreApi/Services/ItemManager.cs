using BestDealLib.Models;
using Microsoft.CodeAnalysis;
using System.Xml.Linq;

namespace CoreApi2.Services
{
    public class ItemManager
    {

        public static Item UpdateFrom(Item other)
        {
            Item item = new Item();
            item.StoreId = other.StoreId;
            item.CategoryId = other.CategoryId;
            item.CategoryName = other.CategoryName;
            item.ParentCategoryId = other.ParentCategoryId;
            item.ParentCategoryName = other.ParentCategoryName;
            item.Sku = other.Sku;
            item.Thc = other.Thc;
            item.Cbd = other.Cbd;
            item.ThcExact = other.ThcExact;
            item.CbdExact = other.CbdExact;
            item.Species = other.Species;
            item.UnitOfMeasurement = other.UnitOfMeasurement;
            item.Name = other.Name;
            item.Description = other.Description;
            item.Upc = other.Upc;
            item.SellingUnit = other.SellingUnit;
            item.Stock = other.Stock;
            item.Price = other.Price;
            item.SalePrice = other.SalePrice;
            item.Location = other.Location;
            item.WeightPerUnit = other.WeightPerUnit;
            item.Brand = other.Brand;
            item.ImageUrl = other.ImageUrl;
            item.Dfe = other.Dfe;
            return item;
        }

    }
}
