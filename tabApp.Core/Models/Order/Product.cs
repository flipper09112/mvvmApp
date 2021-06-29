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

        [OneToMany]
        public List<ReSaleValues> ReSaleValues { get; set; }

        public Product()
        {
            ReSaleValues = new List<ReSaleValues>();
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
