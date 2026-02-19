using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Interfaces.Deliverys
{
    public interface IDeliverysManagerService
    {
        List<Delivery> Deliveries { get; }

        void SetDeliverys(List<Delivery> deliveries);
        List<string> GetListNamesAndId();
        Delivery GetDeliveryByNameAndId(string deliveryNameAndId);
        Delivery GetDeliveryId(string deliveryId);
    }
}
