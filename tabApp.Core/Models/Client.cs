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
        public DateTime PaymentDate { get; private set; }
        public PaymentTypeEnum PaymentType { get; }
        public bool Active { get; }
        public double ExtraValueToPay { get; private set; }
        public List<DailyOrder> DailyOrders { get; }

        //Extra Params
        public List<Regist> DetailsList { get; private set; }
        public List<ExtraOrder> ExtraOrdersList { get; private set; }

        //Indirects Params
        #region PARAMS
        public DailyOrder SegDailyOrder => DailyOrders[0];
        public DailyOrder TerDailyOrder => DailyOrders[1];
        public DailyOrder QuaDailyOrder => DailyOrders[2];
        public DailyOrder QuiDailyOrder => DailyOrders[3];
        public DailyOrder SexDailyOrder => DailyOrders[4];
        public DailyOrder SabDailyOrder => DailyOrders[5];
        public DailyOrder DomDailyOrder => DailyOrders[6];

        #endregion

        public Client(int id, string name, string subName, Address address, DateTime paymentDate, PaymentTypeEnum paymentType, bool active, double extraValue, List<DailyOrder> dailyOrders)
        {
            Id = id;
            Name = name;
            SubName = subName;
            Address = address;
            PaymentDate = paymentDate;
            PaymentType = paymentType;
            Active = active;
            ExtraValueToPay = extraValue;
            DailyOrders = dailyOrders;

            DetailsList = new List<Regist>();
            ExtraOrdersList = new List<ExtraOrder>();
        }

        #region sets
        internal void SetNewRegist(Regist detail)
        {
            DetailsList.Add(detail);
            DetailsList.Sort((x, y) => DateTime.Compare(x.DetailRegistDay, y.DetailRegistDay));
            DetailsList.Reverse();
        }
        internal void SetNewOrder(ExtraOrder extraOrder)
        {
            ExtraOrdersList.Add(extraOrder);
            ExtraOrdersList.Sort((x, y) => DateTime.Compare(y.OrderDay, x.OrderDay));
        }
        internal void SetPaymentDate(DateTime dateSelected, bool payExtra)
        {
            PaymentDate = dateSelected;
            if (payExtra)
                ExtraValueToPay = 0;
        }

        #endregion
        internal void AddExtra(double extra)
        {
            ExtraValueToPay += extra;
        }



        internal void RemoveOrder(ExtraOrder order)
        {
            ExtraOrdersList.Remove(order);
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
