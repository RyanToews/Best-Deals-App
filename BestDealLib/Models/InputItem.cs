using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestDealLib.Models
{

    /// <summary>
    /// InputItem our API gets from the Techpros API with variants, 
    /// because to keep the variant data, the JSON returned 
    /// from the Techpros API needs to be put in a class 
    /// with a Variants Iterable Object, like an array or List.
    /// </summary>
    public class InputItem
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

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

        [JsonProperty("variants")]
        public List<Variant>? Variants { get; set; }
    }
}
