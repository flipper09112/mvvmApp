using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Interfaces.WebServices
{
    public class GetProductsOutput : BaseOutput
    {
        public List<ProductModel> products { get; set; }
    }

    public class ProductModel
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("productName")]
        public string ProductName { get; set; }

        [JsonPropertyName("productImage")]
        public string ProductImage { get; set; }

        [JsonPropertyName("productImageName")]
        public string ProductImageName { get; set; }
    }
}