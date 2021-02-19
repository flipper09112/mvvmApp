using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace tabApp.Core.Models
{
    [Serializable]
    public abstract class Order /*: ISerializable*/
    {
        public List<(int ProductId, double Ammount)> AllItems { get; protected set; }

        public bool IsTotal { get; protected set; }

        protected Order(List<(int ProductId, double Ammount)> allItems, bool isTotal)
        {
            AllItems = allItems;
            IsTotal = isTotal;
        }

      /*  public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            List<int> productsIds = new List<int>();
            List<double> ammounts = new List<double>();
            AllItems.ForEach(item => productsIds.Add(item.ProductId));
            AllItems.ForEach(item => ammounts.Add(item.Ammount));

            info.AddValue("AllItemsInt", productsIds.ToArray());
            info.AddValue("AllItemsDouble", ammounts.ToArray());
            info.AddValue("IsTotal", IsTotal);
        }
        protected Order(SerializationInfo info, StreamingContext context)
        {
            IsTotal = (bool)info.GetValue("IsTotal", typeof(bool));

            List<int> intList = new List<int>((int[])info.GetValue("AllItemsInt", typeof(int[])));
            List<double> doubleList = new List<double>((double[])info.GetValue("AllItemsDouble", typeof(double[])));

            for(int i = 0; i < intList.Count; i++)
            {
                AllItems = new List<(int ProductId, double Ammount)>();
                AllItems.Add((intList[i], doubleList[i]));
            }
        }*/
    }

    [Serializable]
    public class DailyOrder : Order
    {
        public DayOfWeek DayOfWeek;

        public DailyOrder(DayOfWeek dayOfWeek, List<(int ProductId, double Ammount)> allItems, bool isTotal = true) : base(allItems, isTotal)
        {
            DayOfWeek = dayOfWeek;
        }

       /* public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("DayOfWeek", DayOfWeek);
            //info.AddValue("IsTotal", IsTotal);
           // info.AddValue("AllItems", AllItems);
        }

        protected DailyOrder(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            DayOfWeek = (DayOfWeek)info.GetValue("DayOfWeek", typeof(DayOfWeek));
        }*/
    }

    [Serializable]
    public class ExtraOrder : Order
    {
        public DetailTypeEnum DetailType = DetailTypeEnum.Order;
        public DateTime OrderDay { get; }
        public int ClientId { get; }
        public DateTime OrderRegistDay { get; }
        public bool StoreOrder { get; }

        public ExtraOrder(int clientId, DateTime orderRegistDay, DateTime orderDay, List<(int ProductId, double Ammount)> allItems, bool isTotal, bool storeOrder) : base(allItems, isTotal)
        {
            ClientId = clientId;
            OrderRegistDay = orderRegistDay;
            OrderDay = orderDay;
            StoreOrder = storeOrder;
        }

        public override bool Equals(object obj)
        {
            ExtraOrder order = obj as ExtraOrder;
            if(order != null)
            {
                bool same = order.AllItems.Count == AllItems.Count && !order.AllItems.Except(AllItems).Any();
                if (order.ClientId == this.ClientId
                    && same
                    && OrderDay == order.OrderDay
                    && OrderRegistDay == order.OrderRegistDay
                    && this.IsTotal == order.IsTotal
                    && this.StoreOrder == order.StoreOrder)
                    return true;
            }
            return false;
        }
    }
}
