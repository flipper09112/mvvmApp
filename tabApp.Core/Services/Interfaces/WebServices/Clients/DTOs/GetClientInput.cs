using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;

namespace tabApp.Core.Services.Interfaces.WebServices.Clients.DTOs
{
    public class GetClientInput : BaseInput
    {     
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("search_in")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ClientFieldEnum SearchIn { get; set; }
    }

    public enum ClientFieldEnum
    {
        ID, 
        Code, 
        Name,
        Email, 
        Vat_Number, 
        Mobile
    }
}
