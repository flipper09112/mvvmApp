using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tabApp.Core.Models
{
    public abstract class Order
    {
        public List<(int ProductId, double Ammount)> AllItems { get; }

        public bool IsTotal { get; }

        protected Order(List<(int ProductId, double Ammount)> allItems, bool isTotal)
        {
            AllItems = allItems;
            IsTotal = isTotal;
        }
    }

    public class DailyOrder : Order
    {
        public DayOfWeek DayOfWeek;

        public DailyOrder(DayOfWeek dayOfWeek, List<(int ProductId, double Ammount)> allItems, bool isTotal = true) : base(allItems, isTotal)
        {
            DayOfWeek = dayOfWeek;
        }
    }

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
