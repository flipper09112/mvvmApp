using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;

namespace tabApp.Core.Services.Interfaces.WebServices.Admin.DTOs
{
    public class GetVehiclesOutput : BaseOutput
    {
        public bool status { get; set; }
        public List<Datum> data { get; set; }
    }

    public class Datum
    {
        public int id { get; set; }
        public int subscription_id { get; set; }
        public string name { get; set; }
        public string license_plate { get; set; }
        public int active { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
    }
}
