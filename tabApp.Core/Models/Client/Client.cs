using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace tabApp.Core.Models
{
    [Serializable]
    public class Client /*: ISerializable*/
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string SubName { get; private set; }
        public Address Address { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public DateTime? StartDayStopService { get; set; }
        public DateTime? LastDayStopService { get; set; }
        public PaymentTypeEnum PaymentType { get; private set; }
        public bool Active { get; private set; }
        public double ExtraValueToPay { get; private set; }
        public List<DailyOrder> DailyOrders { get; private set; }
        public string PhoneNumber { get; private set; }
        public DateTime LastChangeDate { get; set; }

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

        public Client(int id, string name, string subName, Address address, DateTime paymentDate, DateTime? startDayStopService, DateTime? lastDayStopService, PaymentTypeEnum paymentType, bool active, double extraValue, List<DailyOrder> dailyOrders, string phoneNumber, DateTime lastChangeDate)
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
            PhoneNumber = phoneNumber;
            LastChangeDate = lastChangeDate;
            StartDayStopService = startDayStopService;
            LastDayStopService = lastDayStopService;

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

    [Serializable]
    public class Address
    {
        public string AddressDesc { get; private set; }
        public int NumberDoor { get; private set; }
        public string Coordenadas { get; private set; }

        public string Lat => Coordenadas.Split(',')[0] + "," + Coordenadas.Split(',')[1];
        public string Lgt => Coordenadas.Split(',')[2] + "," + Coordenadas.Split(',')[3];

        public Address(string addressDesc, int door, string coord)
        {
            AddressDesc = addressDesc;
            NumberDoor = door;
            Coordenadas = coord;
        }

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
