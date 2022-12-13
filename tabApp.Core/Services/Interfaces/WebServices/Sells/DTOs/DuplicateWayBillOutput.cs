using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;

namespace tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs
{
    public class DuplicateWayBillOutput : BaseOutput
    {
        public bool status { get; set; }
        public int id { get; set; }
    }
}
