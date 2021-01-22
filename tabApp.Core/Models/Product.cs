using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Models
{
    public class Product
    {
        public string Name { get; }
        public int Id { get; }
        public string ImageReference { get; }
        public bool Unity { get; }
        public ProductTypeEnum ProguctType { get; }
        public double PVP { get; }
        public List<(int Id, double Value)> ReSaleValues { get; }

        public Product(string name, int id, string imageReference, bool unity, 
            ProductTypeEnum proguctType, double pVP/*, List<(int Id, double Value)> reSaleValues*/)
        {
            Name = name;
            Id = id;
            ImageReference = imageReference;
            Unity = unity;
            ProguctType = proguctType;
            PVP = pVP;
            //ReSaleValues = reSaleValues;
        }
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
        None
    }
}
