using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Models.GlobalOrder;
using tabApp.Core.Services.Interfaces.Orders;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.Services.Implementations.Orders
{
    class GlobalOrdersPastManagerService : IGlobalOrdersPastManagerService
    {
        public List<GlobalOrderRegist> GlobalOrderRegists { get; set; }
        public List<ProductAmmount> GetOrderFromDay(DateTime dateSelected)
        {
            return GlobalOrderRegists.Find(item => item.OrderRegistDate.Date == dateSelected)?.ItemsList;
        }

        public bool HasRegistThisDay(DateTime today)
        {
            return GlobalOrderRegists.Find(item => item.OrderRegistDate.Date == today) == null ? false : true;
        }

        public DateTime? MinDateRegist()
        {
            DateTime date = DateTime.MaxValue;

            GlobalOrderRegists.ForEach(item =>
            {
                if (item.OrderRegistDate.Date < date.Date)
                    date = item.OrderRegistDate.Date;
            });

            return date;
        }

        public void SetGlobalOrders(List<GlobalOrderRegist> globalOrderRegists)
        {
            GlobalOrderRegists = globalOrderRegists;
        }

        public GlobalOrderRegist UpdateTotalOrder(List<ProductAmmount> productsList, DateTime dateSelected)
        {
            var order = GlobalOrderRegists.Find(item => item.OrderRegistDate.Date == dateSelected);

            order.JsonData = JsonConvert.SerializeObject(productsList);

            return order;
        }
    }
}
