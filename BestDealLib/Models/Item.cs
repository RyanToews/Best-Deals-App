using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestDealLib.Models
{

    /// <summary>
    /// InputItem our API will respond with.
    /// </summary>
    public class Item
    {

        [Key]
        [Column(Order = 1)]
        [JsonProperty("id")]
        public string? Id { get; set; }

        [Key]
        [Column(Order = 2)]
        [JsonProperty("storeId")]
        public int StoreId { get; set; }

        [JsonProperty("categoryId")]
        public string? CategoryId { get; set; }

        [JsonProperty("categoryName")]
        public string? CategoryName { get; set; }

        [JsonProperty("parentCategoryId")]
        public string? ParentCategoryId { get; set; }

        [JsonProperty("parentCategoryName")]
        public string? ParentCategoryName { get; set; }

        [JsonProperty("sku")]
        public string? Sku { get; set; }

        [JsonProperty("thc")]
        public string? Thc { get; set; }

        [JsonProperty("cbd")]
        public string? Cbd { get; set; }

        [JsonProperty("thcExact")]
        public float? ThcExact { get; set; }

        [JsonProperty("cbdExact")]
        public float? CbdExact { get; set; }

        [JsonProperty("species")]
        public int Species { get; set; }

        [JsonProperty("unitOfMeasurement")]
        public int UnitOfMeasurement { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("upc")]
        public string? Upc { get; set; }

        [JsonProperty("sellingUnit")]
        public string? SellingUnit { get; set; }

        [JsonProperty("stock")]
        public string? Stock { get; set; }

        [JsonProperty("price")]
        public string? Price { get; set; }

        [JsonProperty("salePrice")]
        public string? SalePrice { get; set; }

        [JsonProperty("location")]
        public string? Location { get; set; }

        [JsonProperty("weightPerUnit")]
        public double? WeightPerUnit { get; set; }

        [JsonProperty("brand")]
        public string? Brand { get; set; }

        [JsonProperty("imageUrl")]
        public string? ImageUrl { get; set; }

        [JsonProperty("dfe")]
        public double? Dfe { get; set; }

        public Item()
        {
            // Default constructor (needed by Entity Framework)
        }

        public Item(InputItem inputItem, int storeId)
        {
            StoreId = storeId;
            Id = inputItem.Id;
            CategoryId = inputItem.CategoryId;
            CategoryName = inputItem.CategoryName;
            ParentCategoryId = inputItem.ParentCategoryId;
            ParentCategoryName = inputItem.ParentCategoryName;
            Sku = inputItem.Sku;
            Thc = inputItem.Thc;
            Cbd = inputItem.Cbd;
            ThcExact = inputItem.ThcExact;
            CbdExact = inputItem.CbdExact;
            Species = inputItem.Species;
            UnitOfMeasurement = inputItem.UnitOfMeasurement;

            if (inputItem.Variants?[0] != null)
            {
                Name = inputItem.Variants[0].Name;
                Description = inputItem.Variants[0].Description;
                Upc = inputItem.Variants[0].Upc;
                SellingUnit = inputItem.Variants[0].SellingUnit;
                Stock = inputItem.Variants[0].Stock;
                Price = inputItem.Variants[0].Price;
                SalePrice = inputItem.Variants[0].SalePrice;
                Location = inputItem.Variants[0].Location;
                WeightPerUnit = inputItem.Variants[0].WeightPerUnit;
                Brand = inputItem.Variants[0].Brand;
                ImageUrl = inputItem.Variants[0].ImageUrl;
                Dfe = inputItem.Variants[0].Dfe;
            }
        }
    }
}
