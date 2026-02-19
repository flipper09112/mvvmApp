using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Interfaces.Helpers
{
    public interface IGetSpinnerDatesService
    {
        List<DateTime> GetListDatesToPay(Client client);
    }
}
