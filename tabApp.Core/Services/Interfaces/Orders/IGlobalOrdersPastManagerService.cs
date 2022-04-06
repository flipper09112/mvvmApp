using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Models.GlobalOrder;

namespace tabApp.Core.Services.Interfaces.Orders
{
    public interface IGlobalOrdersPastManagerService
    {
        List<GlobalOrderRegist> GlobalOrderRegists { get; set; }
        void SetGlobalOrders(List<GlobalOrderRegist> globalOrderRegists);
        List<ProductAmmount> GetOrderFromDay(DateTime dateSelected);
        DateTime? MinDateRegist();
        bool HasRegistThisDay(DateTime today);
        GlobalOrderRegist UpdateTotalOrder(List<ProductAmmount> productsList, DateTime dateSelected);
    }
}
