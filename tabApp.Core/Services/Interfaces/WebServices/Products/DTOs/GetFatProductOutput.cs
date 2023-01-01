using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;

namespace tabApp.Core.Services.Interfaces.WebServices.Products.DTOs
{
    public class GetFatProductOutput : BaseOutput
    {
        public bool status { get; set; }
        public List<Datum> data { get; set; }
    }

    public class Averagecostprice
    {
        public int id { get; set; }
        public int subscription_id { get; set; }
        public int item_id { get; set; }
        public int? warehouse_id { get; set; }
        public double average_cost_price { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
    }

    public class Datum
    {
        public int id { get; set; }
        public int subscription_id { get; set; }
        public string reference { get; set; }
        public string description { get; set; }
        public string details { get; set; }
        public int details_show_print { get; set; }
        public object category_id { get; set; }
        public int unit_id { get; set; }
        public int vat_id { get; set; }
        public int vat_exemption_id { get; set; }
        public string type { get; set; }
        public int cost_price { get; set; }
        public string observations { get; set; }
        public object url_image { get; set; }
        public int reference_blocked { get; set; }
        public int description_blocked { get; set; }
        public string created_from { get; set; }
        public int active { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
        public int? stock { get; set; }
        public object category { get; set; }
        public List<Price> prices { get; set; }
        public List<object> barcodes { get; set; }
        public List<Averagecostprice> averagecostprices { get; set; }
        public List<Minimumstock> minimumstock { get; set; }
        public Vatexemption vatexemption { get; set; }
        public Vat vat { get; set; }
        public Unit unit { get; set; }
    }

    public class Minimumstock
    {
        public int id { get; set; }
        public int subscription_id { get; set; }
        public int item_id { get; set; }
        public int? warehouse_id { get; set; }
        public int quantity { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime created_at { get; set; }
    }

    public class Price
    {
        public int id { get; set; }
        public int subscription_id { get; set; }
        public int item_id { get; set; }
        public int price_id { get; set; }
        public double price { get; set; }
        public int discount { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime created_at { get; set; }
        public Pricetable pricetable { get; set; }
    }

    public class Pricetable
    {
        public int id { get; set; }
        public int subscription_id { get; set; }
        public string description { get; set; }
        public int is_default { get; set; }
        public int active { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
    }

    public class Unit
    {
        public int id { get; set; }
        public int subscription_id { get; set; }
        public string description { get; set; }
        public string symbol { get; set; }
        public int active { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
    }

    public class Vat
    {
        public int id { get; set; }
        public int subscription_id { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public int tax { get; set; }
        public string saft_region { get; set; }
        public int active { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
    }

    public class Vatexemption
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }


}
