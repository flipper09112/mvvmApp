using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Helpers
{
    public static class ClientHelper
    {
        public static DailyOrder GetDailyOrder(DayOfWeek dayOfWeek, Client client)
        {
            switch(dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return client.SegDailyOrder;
                case DayOfWeek.Tuesday:
                    return client.TerDailyOrder;
                case DayOfWeek.Wednesday:
                    return client.QuaDailyOrder;
                case DayOfWeek.Thursday:
                    return client.QuiDailyOrder;
                case DayOfWeek.Friday:
                    return client.SexDailyOrder;
                case DayOfWeek.Saturday:
                    return client.SabDailyOrder;
                case DayOfWeek.Sunday:
                    return client.DomDailyOrder;
                default:
                    return null;
            }
        }
    }
}
