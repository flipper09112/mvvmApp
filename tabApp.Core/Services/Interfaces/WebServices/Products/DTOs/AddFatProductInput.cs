using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;

namespace tabApp.Core.Services.Interfaces.WebServices.Products.DTOs
{
    public class AddFatProductInput : BaseInput
    {
        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("vat")]
        public int Vat { get; set; }

        [JsonProperty("details_show_print")]
        public bool DetailsShowPrint { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FatProductTypeEnum Type { get; set; }

        [JsonProperty("prices")]
        public string Prices { get; internal set; }
    }

    public class FatPrices
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("discount")]
        public double Discount { get; set; }
    }

    public enum FatProductTypeEnum
    {
        [EnumMember(Value = "Embalagens")]
        Embalagens,
        [EnumMember(Value = "Matérias primas")]
        MatériasPrimas,
        [EnumMember(Value = "Mercadorias")]
        Mercadorias,
        [EnumMember(Value = "Produtos acabados e intermédios")]
        ProdutosAcabadosIntermédios,
        [EnumMember(Value = "Serviços")]
        Serviços,
        [EnumMember(Value = "Subprodutos")]
        Subprodutos,
        [EnumMember(Value = "Transporte")]
        Transporte,
        [EnumMember(Value = "Vasilhame")]
        Vasilhame
    }
}
