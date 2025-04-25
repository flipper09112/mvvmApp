using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;

namespace tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs
{
    public class CreateSellDocumentOutput : BaseOutput
    {
        public string id { get; set; }
        public string url_file { get; set; }
        public List<Item> items { get; set; }
        public string waybill_shipping_date { get; set; }
        public string file_last_generated { get; set; }
    }
}
