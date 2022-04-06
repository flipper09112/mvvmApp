using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Interfaces.Clients
{
    public interface IAmmountToPayService
    {
        double Calculate(Client client, DateTime payTo);
        double CalculateUntilDate(Client client, DateTime endDate);
        double CalculateBetweenDates(Client client, DateTime startDate, DateTime endDate);
    }
}
