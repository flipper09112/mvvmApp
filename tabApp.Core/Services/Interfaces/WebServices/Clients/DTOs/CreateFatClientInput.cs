using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models.Faturation;
using tabApp.Core.Services.Interfaces.WebServices.Bases;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;

namespace tabApp.Core.Services.Interfaces.WebServices.Clients.DTOs
{
    public class CreateFatClientInput : BaseInput
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("vat_number")]
        public string VatNumber { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FatClientTypeEnum Type { get; set; }

        [JsonProperty("vat_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public VatTypeEnum VatType { get; set; }
    }

    public enum FatClientTypeEnum
    {
        Empresarial, 
        Particular
    }
}
