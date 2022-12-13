using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;

namespace tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs
{
    public class DuplicateWayBillInput : BaseInput
    {
        [JsonProperty("document_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DocumentTypeEnum DocumentType { get; set; }
    }
}
