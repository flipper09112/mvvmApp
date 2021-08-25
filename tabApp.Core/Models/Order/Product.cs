using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Models
{
    [Table("Product")]
    public class Product
    {
        public string Name { get; set; }

        [PrimaryKey]
        public int Id { get; set; }
        public string ImageReference { get; set; }
        public bool Unity { get; set; }
        public ProductTypeEnum ProductType { get; set; }
        public double PVP { get; set; }
        public double CostProduct { get; set; }
        public double Discount { get; set; }
        public int Iva { get; set; }

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
                return cost + (cost * Iva * 0.01);
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
