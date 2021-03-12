using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace tabApp.Core.Models
{
    [Table("Client")]
    [Serializable]
    public class Client
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubName { get; set; }

        [OneToOne]
        public Address Address { get; set; }

        [ForeignKey(typeof(Address))]
        public int AddressId { get; set; }

        public DateTime PaymentDate { get; set; }
        public DateTime? StartDayStopService { get; set; }
        public DateTime? LastDayStopService { get; set; }
        public PaymentTypeEnum PaymentType { get; set; }
        public bool Active { get; set; }
        public double ExtraValueToPay { get; set; }

        [Ignore]
        public List<DailyOrder> DailyOrders { get; set; }

        public string PhoneNumber { get; set; }
        public DateTime LastChangeDate { get; set; }

        //Extra Params
        [OneToMany]
        public List<Regist> DetailsList { get; set; }

        [Ignore]
        public List<ExtraOrder> ExtraOrdersList { get; set; }

        //Indirects Params
        #region PARAMS

        [Ignore]
        public DailyOrder SegDailyOrder => DailyOrders[0];

        [Ignore]
        public DailyOrder TerDailyOrder => DailyOrders[1];

        [Ignore]
        public DailyOrder QuaDailyOrder => DailyOrders[2];

        [Ignore]
        public DailyOrder QuiDailyOrder => DailyOrders[3];

        [Ignore]
        public DailyOrder SexDailyOrder => DailyOrders[4];

        [Ignore]
        public DailyOrder SabDailyOrder => DailyOrders[5];

        [Ignore]
        public DailyOrder DomDailyOrder => DailyOrders[6];

        #endregion

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

        #region Update

        internal void UpdatePhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }
        internal void UpdateLocation(string newLocation)
        {
            Address.SetCoordenadas(newLocation);
        }

        internal void UpdateName(string newValue)
        {
            Name = newValue;
        }

        internal void UpdateSubName(string newValue)
        {
            SubName = newValue;
        }

        internal void UpdateAddressDesc(string newValue)
        {
            Address.UpdateAddressDesc(newValue);
        }

        internal void UpdateNumberDoor(int newValue)
        {
            Address.UpdateAddressNumberDoor(newValue);
        }

        internal void UpdatePaymentDate(DateTime dateTime)
        {
            PaymentDate = dateTime;
        }

        internal void UpdateExtraValueToPay(double v)
        {
            ExtraValueToPay = v;
        }

        internal void UpdateDailyOrder(DailyOrder dailyOrder, DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Monday:
                    DailyOrders[0] = dailyOrder;
                    break;
                case DayOfWeek.Tuesday:
                    DailyOrders[1] = dailyOrder;
                    break;
                case DayOfWeek.Wednesday:
                    DailyOrders[2] = dailyOrder;
                    break;
                case DayOfWeek.Thursday:
                    DailyOrders[3] = dailyOrder;
                    break;
                case DayOfWeek.Friday:
                    DailyOrders[4] = dailyOrder;
                    break;
                case DayOfWeek.Saturday:
                    DailyOrders[5] = dailyOrder;
                    break;
                case DayOfWeek.Sunday:
                    DailyOrders[6] = dailyOrder;
                    break;
            }
        }

        internal void UpdateActive(bool active)
        {
            Active = active;
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

        public byte[] ObjectToByteArray()
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, this);
                return ms.ToArray();
            }
        }

        internal void ResetDailyOrders()
        {
            DailyOrders[0] = new DailyOrder(DayOfWeek.Monday, new List<(int ProductId, double Ammount)>());
            DailyOrders[1] = new DailyOrder(DayOfWeek.Tuesday, new List<(int ProductId, double Ammount)>());
            DailyOrders[2] = new DailyOrder(DayOfWeek.Wednesday, new List<(int ProductId, double Ammount)>());
            DailyOrders[3] = new DailyOrder(DayOfWeek.Thursday, new List<(int ProductId, double Ammount)>());
            DailyOrders[4] = new DailyOrder(DayOfWeek.Friday, new List<(int ProductId, double Ammount)>());
            DailyOrders[5] = new DailyOrder(DayOfWeek.Saturday, new List<(int ProductId, double Ammount)>());
            DailyOrders[6] = new DailyOrder(DayOfWeek.Sunday, new List<(int ProductId, double Ammount)>());
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

    [Table("Address")]
    [Serializable]
    public class Address
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string AddressDesc { get; set; }
        public int NumberDoor { get; set; }
        public string Coordenadas { get; set; }

        [Ignore]
        public string Lat => Coordenadas.Split(',')[0] + "," + Coordenadas.Split(',')[1];
        [Ignore]
        public string Lgt => Coordenadas.Split(',')[2] + "," + Coordenadas.Split(',')[3];

        internal void SetCoordenadas(string newLocation)
        {
            Coordenadas = newLocation;
        }

        internal void UpdateAddressDesc(string newValue)
        {
            AddressDesc = newValue;
        }

        internal void UpdateAddressNumberDoor(int newValue)
        {
            NumberDoor = newValue;
        }
    }
}
