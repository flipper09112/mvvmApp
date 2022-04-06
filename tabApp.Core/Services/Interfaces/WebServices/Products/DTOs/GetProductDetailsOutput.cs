using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Interfaces.WebServices.Products.DTOs
{
    public class GetProductDetailsOutput : BaseOutput
    {
        [JsonPropertyName("product")]
        public Product Product { get; set; }

        [JsonPropertyName("productImage")]
        public string ProductImage { get; set; }

        [JsonPropertyName("productImageName")]
        public string ProductImageName { get; set; }
    }
}
