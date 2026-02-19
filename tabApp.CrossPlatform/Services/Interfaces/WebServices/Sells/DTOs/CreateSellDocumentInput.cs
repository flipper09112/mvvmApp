using iText.StyledXmlParser.Jsoup.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;
using tabApp.Core.ViewModels.Global;

namespace tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs
{
    public class CreateSellDocumentInput : BaseInput
    {
        [JsonProperty("issue_date")]
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public DateTime IssueDate { get; set; }

        [JsonProperty("document_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DocumentTypeEnum DocumentType { get; set; }

        [JsonProperty("customer")]
        public string CustomerId { get; set; }

        [JsonProperty("vat_number")]
        public string VatNumber { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("vat_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public VatTypeEnum VatType { get; set; }

        [JsonProperty("vehicle")]
        public string Vehicle { get; set; }

        [JsonProperty("waybill_shipping_date")]
        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd HH:mm")]
        public DateTime WaybillShippingDate { get; set; }

        [JsonProperty("waybill_global")]
        public bool WaybillGlobal { get; set; }

        //[JsonProperty("location_origin")]
        //public string LocationOrigin { get; set; }

        [JsonProperty("cargo_location")]
        public string CargoLocation { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StatusEnum Status { get; set; }

        [JsonProperty("payment_method")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentMethodEnum PaymentMethod { get; set; }

        [JsonProperty("items")]
        public List<FatItem> Items { get; set; }
    }

    public enum PaymentMethodEnum
    {
        [EnumMember(Value = "Numerário")]
        Numerario,
        [EnumMember(Value = "Transferência bancária")]
        TransferenciaBancaria
    }

    public class FatItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("details")]
        public string Details { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }

        [JsonProperty("discount")]
        public string Discount { get; set; }

        [JsonProperty("vat")]
        public string Vat { get; set; }

        [JsonProperty("vat_exemption")]
        public string VatExemption { get; set; }

    }
    public enum StatusEnum
    {
        Rascunho,
        Terminado
    }

    public enum VatTypeEnum
    {
        [EnumMember(Value = "Debitar IVA")]
        DebitarIVA,
        [EnumMember(Value = "IVA incluído")]
        IVAincluído,
        [EnumMember(Value = "Não fazer nada")]
        Nãofazernada
    }
    public enum DocumentTypeEnum
    {
        [EnumMember(Value = "Factura")]
        Factura,
        [EnumMember(Value = "Factura Recibo")]
        FacturaRecibo,
        [EnumMember(Value = "Factura Simplificada")]
        FacturaSimplificada,
        [EnumMember(Value = "Nota de Crédito")]
        NotadeCrédito,
        [EnumMember(Value = "Nota de Débito")]
        NotadeDébito,
        [EnumMember(Value = "Factura Pró-forma")]
        FacturaPróForma,
        [EnumMember(Value = "Orçamento")]
        Orçamento,
        [EnumMember(Value = "Encomenda")]
        Encomenda,
        [EnumMember(Value = "Guia de Transporte")]
        GuiadeTransporte,
        [EnumMember(Value = "Guia de Remessa")]
        GuiadeRemessa,
        [EnumMember(Value = "Guia de Consignação")]
        GuiadeConsignação,
        [EnumMember(Value = "Guia de Devolução")]
        GuiadeDevolução
    }

    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}
