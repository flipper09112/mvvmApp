using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;

namespace tabApp.Core.Services.Interfaces.WebServices.Clients.DTOs
{
    public class GetClientOutput : BaseOutput
    {
        public bool status { get; set; }
        public List<Datum> data { get; set; }
    }

    public class Currency
    {
        public int id { get; set; }
        public int subscription_id { get; set; }
        public string description { get; set; }
        public string symbol { get; set; }
        public string code_iso { get; set; }
        public int exchange_sale { get; set; }
        public int exchange_buy { get; set; }
        public int is_default { get; set; }
        public int active { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
    }

    public class Datum
    {
        public int id { get; set; }
        public int subscription_id { get; set; }
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
        public int currency_id { get; set; }
        public object payment_method_id { get; set; }
        public object payment_condition_id { get; set; }
        public object shipping_mode_id { get; set; }
        public object price_id { get; set; }
        public object employee_id { get; set; }
        public string type { get; set; }
        public int vat_exemption_id { get; set; }
        public string vat_type { get; set; }
        public int? irs_retention_tax { get; set; }
        public string observations { get; set; }
        public string other_emails { get; set; }
        public int code_blocked { get; set; }
        public int vat_number_blocked { get; set; }
        public int name_blocked { get; set; }
        public string created_from { get; set; }
        public int receive_sms { get; set; }
        public int receive_emails { get; set; }
        public int active { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }
        public List<object> addresses { get; set; }
        public Currency currency { get; set; }
        public object paymentmethod { get; set; }
        public object paymentcondition { get; set; }
        public object shippingmode { get; set; }
        public object employee { get; set; }
        public object price { get; set; }
        public Vatexemption vatexemption { get; set; }
    }

    public class Vatexemption
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }
}
