using System;
using System.Collections.Generic;
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

        public ExtraOrder(int clientId, DateTime orderRegistDay, DateTime orderDay, List<(int ProductId, double Ammount)> allItems, bool isTotal) : base(allItems, isTotal)
        {
            ClientId = clientId;
            OrderRegistDay = orderRegistDay;
            OrderDay = orderDay;
        }
    }
}
