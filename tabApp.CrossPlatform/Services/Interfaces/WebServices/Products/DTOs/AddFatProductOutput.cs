using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;

namespace tabApp.Core.Services.Interfaces.WebServices.Products.DTOs
{
    public class AddFatProductOutput : BaseOutput
    {
        public bool status { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public int id { get; set; }
    }
}
