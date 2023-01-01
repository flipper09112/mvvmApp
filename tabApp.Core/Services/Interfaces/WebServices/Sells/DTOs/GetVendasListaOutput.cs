using Autofac.Core;
using iText.StyledXmlParser.Jsoup.Nodes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;

namespace tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs
{
    public class GetVendasListaOutput : BaseOutput
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("data")]
        public List<Datum> Data { get; set; }
    }

    public class Createdby
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public object subscription { get; set; }
        public List<object> restrictions { get; set; }
    }

    public class Currency
    {
        public int id { get; set; }
        public string description { get; set; }
        public string symbol { get; set; }
    }

    public class Customer
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string contact { get; set; }
        public string vat_number { get; set; }
        public string country { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string postal_code { get; set; }
        public string email { get; set; }
        public string telephone { get; set; }
        public string mobile { get; set; }
        public int code_blocked { get; set; }
        public int vat_number_blocked { get; set; }
        public int name_blocked { get; set; }
        public string created_at { get; set; }
    }

    public class Customercountry
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code_iso_one { get; set; }
        public string code_iso_two { get; set; }
    }

    public class Datum
    {
        public int id { get; set; }
        public int subscription_id { get; set; }
        public int number { get; set; }
        public string issue_date { get; set; }
        public object due_date { get; set; }
        public int serie_id { get; set; }
        public int document_type_id { get; set; }
        public double gross_total { get; set; }
        public int total_discount { get; set; }
        public double net_total { get; set; }
        public double total_base_vat { get; set; }
        public double total_vat { get; set; }
        public int total_shipping { get; set; }
        public double total_services { get; set; }
        public double grand_total { get; set; }
        public int final_discount_financial { get; set; }
        public int final_discount_global { get; set; }
        public int final_discount_global_value { get; set; }
        public int customer_id { get; set; }
        public string customer_code { get; set; }
        public string customer_name { get; set; }
        public string customer_vat_number { get; set; }
        public string customer_country { get; set; }
        public string customer_city { get; set; }
        public string customer_address { get; set; }
        public string customer_postal_code { get; set; }
        public string customer_delivery_address_country { get; set; }
        public string customer_delivery_address_address { get; set; }
        public string customer_delivery_address_city { get; set; }
        public string customer_delivery_address_postal_code { get; set; }
        public string company_name { get; set; }
        public string company_vat_number { get; set; }
        public string company_country { get; set; }
        public string company_address { get; set; }
        public string company_city { get; set; }
        public string company_postal_code { get; set; }
        public int company_vat_scheme { get; set; }
        public object payment_method_id { get; set; }
        public object payment_condition_id { get; set; }
        public object shipping_mode_id { get; set; }
        public int? shipping_value { get; set; }
        public int? shipping_vat_id { get; set; }
        public int price_id { get; set; }
        public int currency_id { get; set; }
        public int currency_exchange { get; set; }
        public string vat_type { get; set; }
        public string observations { get; set; }
        public int irs_retention_apply { get; set; }
        public object irs_retention_base { get; set; }
        public object irs_retention_total { get; set; }
        public int? irs_retention_tax { get; set; }
        public string url_file { get; set; }
        public int file_previewed { get; set; }
        public string file_last_generated { get; set; }
        public string file_language { get; set; }
        public string file_format { get; set; }
        public int email_sent { get; set; }
        public int sms_sent { get; set; }
        public string status { get; set; }
        public string status_last_change { get; set; }
        public int? vehicle_id { get; set; }
        public object employee_id { get; set; }
        public string waybill_shipping_date { get; set; }
        public object at_code { get; set; }
        public string at_message { get; set; }
        public int location_origin_id { get; set; }
        public string location_origin_name { get; set; }
        public string location_origin_country { get; set; }
        public string location_origin_address { get; set; }
        public string location_origin_city { get; set; }
        public string location_origin_postal_code { get; set; }
        public int? warehouse_origin_id { get; set; }
        public object warehouse_destiny_id { get; set; }
        public object location_destiny_id { get; set; }
        public object mb_entity { get; set; }
        public object mb_sub_entity { get; set; }
        public object mb_reference { get; set; }
        public string cargo_location { get; set; }
        public string discharge_location { get; set; }
        public object cargo_date { get; set; }
        public object discharge_date { get; set; }
        public int signed { get; set; }
        public object signed_id { get; set; }
        public int sign_attempt { get; set; }
        public object sign_error { get; set; }
        public int created_by { get; set; }
        public int? finished_by { get; set; }
        public string finished_at { get; set; }
        public object canceled_by { get; set; }
        public object canceled_at { get; set; }
        public object canceled_reason { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
        public object reference { get; set; }
        public Serie serie { get; set; }
        public Documenttype documenttype { get; set; }
        public Documenttypeserie documenttypeserie { get; set; }
        public Customer customer { get; set; }
        public object paymentmethod { get; set; }
        public object paymentcondition { get; set; }
        public object shippingmode { get; set; }
        public Shippingvat shippingvat { get; set; }
        public Price price { get; set; }
        public Currency currency { get; set; }
        public Vehicle vehicle { get; set; }
        public object employee { get; set; }
        public object locationdestiny { get; set; }
        public WarehouseOrigin warehouse_origin { get; set; }
        public object warehouse_destiny { get; set; }
        public List<Vat> vats { get; set; }
        public List<Item> items { get; set; }
        public Createdby createdby { get; set; }
        public Finishedby finishedby { get; set; }
        public object canceledby { get; set; }
        public Customercountry customercountry { get; set; }
        public object accountopen { get; set; }
    }

    public class Documenttype
    {
        public int id { get; set; }
        public string description { get; set; }
        public string saft_initials { get; set; }
        public string type { get; set; }
    }

    public class Documenttypeserie
    {
        public int id { get; set; }
        public int document_type_id { get; set; }
        public int serie_id { get; set; }
        public int document_number { get; set; }
        public string atcud { get; set; }
    }

    public class Finishedby
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public object subscription { get; set; }
        public List<object> restrictions { get; set; }
    }

    public class Item
    {
        public int id { get; set; }
        public int sale_id { get; set; }
        public int item_id { get; set; }
        public string item_details { get; set; }
        public double unit_price { get; set; }
        public int quantity { get; set; }
        public int discount { get; set; }
        public double gross_total { get; set; }
        public double net_total { get; set; }
        public double total_base_vat { get; set; }
        public double total_vat { get; set; }
        public int total_discount { get; set; }
        public double grand_total { get; set; }
        public int vat_id { get; set; }
        public int unit_id { get; set; }
        public int vat_exemption_id { get; set; }
        public Item2 item { get; set; }
        public Vat2 vat { get; set; }
        public Unit unit { get; set; }
        public Vatexemption vatexemption { get; set; }
    }

    public class Item2
    {
        public int id { get; set; }
        public string reference { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public int reference_blocked { get; set; }
        public int description_blocked { get; set; }
        public string created_at { get; set; }
    }

    public class Price
    {
        public int id { get; set; }
        public string description { get; set; }
    }

    public class Serie
    {
        public int id { get; set; }
        public string description { get; set; }
        public string valid_until { get; set; }
    }

    public class Shippingvat
    {
        public int id { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public int tax { get; set; }
    }

    public class Unit
    {
        public int id { get; set; }
        public string description { get; set; }
        public string symbol { get; set; }
    }

    public class Vat
    {
        public int id { get; set; }
        public int sale_id { get; set; }
        public int vat_id { get; set; }
        public double total_base_vat { get; set; }
        public double total_vat { get; set; }
        public Vat vat { get; set; }
    }

    public class Vat2
    {
        public int id { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public int tax { get; set; }
        public string saft_region { get; set; }
    }

    public class Vatexemption
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    public class Vehicle
    {
        public int id { get; set; }
        public string name { get; set; }
        public string license_plate { get; set; }
    }

    public class WarehouseOrigin
    {
        public int id { get; set; }
        public string description { get; set; }
    }


}
