using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace tabApp.Core.Models
{
    public class Product
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("id")]
        [PrimaryKey]
        public int Id { get; set; }

        [JsonPropertyName("imageReference")]
        public string ImageReference { get; set; }

        [JsonPropertyName("unity")]
        public bool Unity { get; set; }

        [JsonPropertyName("productType")]
        public ProductTypeEnum ProductType { get; set; }

        [JsonPropertyName("pvp")]
        public double PVP { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("costProduct")]
        public double CostProduct { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("discount")]
        public double? Discount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("iva")]
        public int Iva { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonPropertyName("lastChangeDate")]
        public DateTime? LastChangeDate { get; set; }

        [JsonPropertyName("reSaleValues")]
        [OneToMany]
        public List<ReSaleValues> ReSaleValues { get; set; }

        public Product()
        {
            ReSaleValues = new List<ReSaleValues>();
        }

        internal double GetCostValueWithIva()
        {
            if (Discount == null || Discount == 0)
                return CostProduct + (CostProduct * Iva * 0.01);
            else
            {
                var cost = CostProduct - (CostProduct * Discount * 0.01);
                return (double)(cost + (cost * Iva * 0.01));
            }
        }

        internal bool HasCostInfo()
        {
            if (CostProduct == null || CostProduct == 0)
                return false;
            return true;
        }

        internal double GetCostValueWithoutIva()
        {
            return CostProduct;
        }
    }

    [Table("ReSaleValues")]
    public class ReSaleValues
    {
        [PrimaryKey, AutoIncrement]
        public int ReSaleId { get; set; }

        [ForeignKey(typeof(Product))]
        public int ProductId { get; set; }

        public int ClientId { get; set; }

        public double Value { get; set; }
    }

    [Table("PriceChangeDate")]
    public class PriceChangeDate
    {
        [PrimaryKey, AutoIncrement]
        public int PriceChangeDateId { get; set; }

        public DateTime Date { get; set; }
    }
    

    public enum ProductTypeEnum
    {
        Padaria,
        PastelariaIndividual,
        BolosFestivos,
        BolosTradicionais,
        PastelariaIndividualSalgada,
        SemiFrioFamiliar,
        SemiFrioIndividual,
        Sortido,
        Tartes,
        Tortas,
        Outros,
        Embalados,
        None
    }
}
