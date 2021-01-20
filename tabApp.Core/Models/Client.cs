using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Models
{
    public class Client
    {
        public int Id { get; }
        public string Name { get; }
        public string SubName { get; }
        public Address Address { get; }
        public DateTime PaymentDate { get; }
        public PaymentTypeEnum PaymentType { get; }
        public bool Active { get; }
        public double ExtraValueToPay { get; }

        public Client(int id, string name, string subName, Address address, DateTime paymentDate, PaymentTypeEnum paymentType, bool active, double extraValue)
        {
            Id = id;
            Name = name;
            SubName = subName;
            Address = address;
            PaymentDate = paymentDate;
            PaymentType = paymentType;
            Active = active;
            ExtraValueToPay = extraValue;
        }
    }

    public enum PaymentTypeEnum
    {
        Diario,
        Semanal,
        Mensal,
        Loja,
        JuntaDias,
        None
    }

    public class Address
    {
        public string AddressDesc { get; }
        public int NumberDoor { get; }
        public string Coordenadas { get; }
        public Address(string addressDesc, int door, string coord)
        {
            AddressDesc = addressDesc;
            NumberDoor = door;
            Coordenadas = coord;
        }
    }
}
