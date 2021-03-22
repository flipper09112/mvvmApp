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
        public int Position { get; set; }
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

        [OneToMany]
        public List<DailyOrder> DailyOrders { get; set; }

        public string PhoneNumber { get; set; }
        public DateTime LastChangeDate { get; set; }

        //Extra Params
        [OneToMany]
        public List<Regist> DetailsList { get; set; }

        [OneToMany]
        public List<ExtraOrder> ExtraOrdersList { get; set; }

        //Indirects Params
        #region PARAMS

        [Ignore]
        public DailyOrder SegDailyOrder => DailyOrders.Find(item => item.DayOfWeek == DayOfWeek.Monday);

        [Ignore]
        public DailyOrder TerDailyOrder => DailyOrders.Find(item => item.DayOfWeek == DayOfWeek.Tuesday);

        [Ignore]
        public DailyOrder QuaDailyOrder => DailyOrders.Find(item => item.DayOfWeek == DayOfWeek.Wednesday);

        [Ignore]
        public DailyOrder QuiDailyOrder => DailyOrders.Find(item => item.DayOfWeek == DayOfWeek.Thursday);

        [Ignore]
        public DailyOrder SexDailyOrder => DailyOrders.Find(item => item.DayOfWeek == DayOfWeek.Friday);

        [Ignore]
        public DailyOrder SabDailyOrder => DailyOrders.Find(item => item.DayOfWeek == DayOfWeek.Saturday);

        [Ignore]
        public DailyOrder DomDailyOrder => DailyOrders.Find(item => item.DayOfWeek == DayOfWeek.Sunday);


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
                    InsertOrReplaceAmmounts(SegDailyOrder, dailyOrder.AllItems);
                    break;
                case DayOfWeek.Tuesday:
                    InsertOrReplaceAmmounts(TerDailyOrder, dailyOrder.AllItems);
                    break;
                case DayOfWeek.Wednesday:
                    InsertOrReplaceAmmounts(QuaDailyOrder, dailyOrder.AllItems);
                    break;
                case DayOfWeek.Thursday:
                    InsertOrReplaceAmmounts(QuiDailyOrder, dailyOrder.AllItems);
                    break;
                case DayOfWeek.Friday:
                    InsertOrReplaceAmmounts(SexDailyOrder, dailyOrder.AllItems);
                    break;
                case DayOfWeek.Saturday:
                    InsertOrReplaceAmmounts(SabDailyOrder, dailyOrder.AllItems);
                    break;
                case DayOfWeek.Sunday:
                    InsertOrReplaceAmmounts(DomDailyOrder, dailyOrder.AllItems);
                    break;
            }
        }

        private void InsertOrReplaceAmmounts(DailyOrder dailyOrder, List<DailyOrderDetails> allNewItems)
        {
            DailyOrderDetails clientDailyOrderDetails = null;

            foreach (DailyOrderDetails item in allNewItems)
            {
                clientDailyOrderDetails = dailyOrder.AllItems.Find(dailyOrderItem => dailyOrderItem.ProductId == item.ProductId);

                if(clientDailyOrderDetails == null)
                {
                    dailyOrder.AllItems.Add(item);
                }
                else
                {
                    clientDailyOrderDetails.Ammount = item.Ammount;
                }
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
            SegDailyOrder.AllItems.Clear(); 
            TerDailyOrder.AllItems.Clear(); 
            QuaDailyOrder.AllItems.Clear(); 
            QuiDailyOrder.AllItems.Clear(); 
            SexDailyOrder.AllItems.Clear(); 
            SabDailyOrder.AllItems.Clear(); 
            DomDailyOrder.AllItems.Clear(); 
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
