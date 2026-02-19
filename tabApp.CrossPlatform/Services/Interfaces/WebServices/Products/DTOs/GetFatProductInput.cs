using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;

namespace tabApp.Core.Services.Interfaces.WebServices.Products.DTOs
{
    public class GetFatProductInput : BaseInput
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("search_in")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FatProductPropertyEnum SearchIn { get; set; }
    }

    public enum FatProductPropertyEnum
    {
        ID,
        Reference, 
        Description, 
        Barcode
    }
}
