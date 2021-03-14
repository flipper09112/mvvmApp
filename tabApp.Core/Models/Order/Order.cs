using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace tabApp.Core.Models
{
    [Table("DailyOrder")]
    [Serializable]
    public class DailyOrder 
    {
        [OneToMany(CascadeOperations = CascadeOperation.All)]  
        public List<DailyOrderDetails> AllItems { get; set; }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Client))]
        public int ClientId { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public DailyOrder()
        {
        }
    }

    [Table("ExtraOrder")]
    [Serializable]
    public class ExtraOrder
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Client))]
        public int ClientId { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<DailyOrderDetails> AllItems { get; set; }

        public DetailTypeEnum DetailType = DetailTypeEnum.Order;
        public DateTime OrderDay { get; set; }
        public DateTime OrderRegistDay { get; set; }
        public bool StoreOrder { get; set; }
        public bool IsTotal { get; set; }

        /*public ExtraOrder(int clientId, DateTime orderRegistDay, DateTime orderDay, List<(int ProductId, double Ammount)> allItems, bool isTotal, bool storeOrder)
        {
            AllItems = allItems;
            IsTotal = isTotal;
            ClientId = clientId;
            OrderRegistDay = orderRegistDay;
            OrderDay = orderDay;
            StoreOrder = storeOrder;
        }*/

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
