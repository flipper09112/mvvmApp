using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Interfaces.Orders
{
    public interface IOrdersManagerService
    {
        double GetValue(int clientId, DailyOrder dailyOrder);
        double WeekAmmount(Client client);
    }
}
