using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Models.GlobalOrder;

namespace tabApp.Core.Services.Interfaces.Orders
{
    public interface IOrdersManagerService
    {
        List<(Client Client, ExtraOrder ExtraOrder)> TodayOrders { get; }
        List<(Client Client, ExtraOrder ExtraOrder)> TomorrowOrders { get; }
        List<CakeClientItem> CakesClientsTomorrow { get; }

        double GetValue(int clientId, DailyOrder dailyOrder);
        double GetValue(int clientId, List<DailyOrderDetails> allitems);
        double WeekAmmount(Client client);

        List<ProductAmmount> GetTotalOrder(DateTime dateTime);
        List<ProductAmmount> GetTotalOrder(DateTime startDate, DateTime endDate);
        List<ProductAmmount> GetTotalOrderFromClient(Client client, DateTime dayOfWeek, List<Client> filterClientsList = null);
        List<ProductAmmount> GetTotalOrderWithoutFilter(DateTime dateSelected);
        List<ProductAmmount> GetTotalOrderWithoutFilter(DateTime firstDay, DateTime lastDay);

        List<(Client Client, ExtraOrder ExtraOrder)> GetOrders(DateTime dateSelected);
    }
}
