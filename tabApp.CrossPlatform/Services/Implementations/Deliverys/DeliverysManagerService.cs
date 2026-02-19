using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Helpers;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Deliverys;

namespace tabApp.Core.Services.Implementations.Deliverys
{
    public class DeliverysManagerService : IDeliverysManagerService
    {
        private List<Delivery> _deliveries;

        public List<Delivery> Deliveries => _deliveries ?? new List<Delivery>();

        public Delivery GetDeliveryByNameAndId(string deliveryNameAndId)
        { 
            string id = deliveryNameAndId.Split(new string[] { "(" }, StringSplitOptions.None)[1].Split(')')[0].Trim();

            return Deliveries.Find(item => item.DeliveryId.ToString() == id);
        }

        public Delivery GetDeliveryId(string deliveryId)
        {
            if (SecureStorageHelper.DeliveryIdAdmin == deliveryId)
                return GetAdminDelivery();

            return Deliveries.Find(item => item.DeliveryId.ToString() == deliveryId);
        }

        private Delivery GetAdminDelivery()
        {
            return new Delivery() { DeliveryName = "Admin", DeliveryId = -1 };
        }

        public List<string> GetListNamesAndId()
        {
            var list = new List<string>();

            Deliveries.ForEach(item => list.Add(item.DeliveryName + " (" + item.DeliveryId + ")"));

            return list;
        }

        public void SetDeliverys(List<Delivery> deliveries)
        {
            _deliveries = deliveries;
        }
    }
}
