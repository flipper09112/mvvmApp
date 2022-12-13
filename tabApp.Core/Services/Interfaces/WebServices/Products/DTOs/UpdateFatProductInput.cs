using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;

namespace tabApp.Core.Services.Interfaces.WebServices.Products.DTOs
{
    public class UpdateFatProductInput : BaseInput
    {
        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("vat")]
        public int Vat { get; set; }

        [JsonProperty("details_show_print")]
        public bool DetailsShowPrint { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FatProductTypeEnum Type { get; set; }

        [JsonProperty("prices")]
        public string Prices { get; internal set; }

        [JsonProperty("active")]
        public bool Active { get; internal set; }
    }
}
