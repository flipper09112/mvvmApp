using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Interfaces.Orders
{
    public interface IOrdersManagerService
    {
        List<(Client Client, ExtraOrder ExtraOrder)> TodayOrders { get; }
        List<(Client Client, ExtraOrder ExtraOrder)> TomorrowOrders { get; }

        double GetValue(int clientId, DailyOrder dailyOrder);
        double WeekAmmount(Client client);
        List<ProductAmmount> GetTotalOrder(DateTime dateTime);
    }
}
