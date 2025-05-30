﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using tabApp.Core.Services.Implementations.Faturation;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace tabApp.Core.Services.Interfaces.WebServices.Bases
{
    public class BaseInput
    {
        [JsonProperty("api_token")]
        public string ApiToken { get; set; } = FaturationService.APIKEY;

        [JsonIgnore]
        public string Id { get; set; }
    }
}
