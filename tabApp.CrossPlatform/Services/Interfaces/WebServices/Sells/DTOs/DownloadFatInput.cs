using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs
{
    public class DownloadFatInput : BaseInput
    {
        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("format")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DocFormatEnum Format { get; set; }

        [JsonProperty("paper_size")]
        public string PaperSize { get; set; }

        [JsonProperty("paper_left_margin")]
        public string PaperLeftMargin { get; set; }

        [JsonProperty("paper_right_margin")]
        public string PaperRightMargin { get; set; }

        [JsonProperty("paper_top_margin")]
        public string PaperTopMargin { get; set; }

        [JsonProperty("paper_bottom_margin")]
        public string PaperBottomMargin { get; set; }

        [JsonProperty("issue")]
        [JsonConverter(typeof(StringEnumConverter))]
        public IssueTypeEnum Issue { get; set; }
    }

    public enum IssueTypeEnum
    {
        [EnumMember(Value = "2ª via")]
        SecondTime,
        [EnumMember(Value = "Original")]
        Original
    }

    public enum DocFormatEnum
    {
        A4,
        POS
    }
}
