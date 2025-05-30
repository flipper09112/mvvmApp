﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;

namespace tabApp.Core.Services.Interfaces.WebServices.Products.DTOs
{
    public class DeleteFatProductOutput : BaseOutput
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
