using Android.Runtime;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Helpers;
using tabApp.Core.Models;
using tabApp.Core.Models.GlobalOrder;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Orders;

namespace tabApp.Core.ViewModels.Global.Other.Finance
{
    public class StatsViewModel : BaseViewModel
    {
        private IGlobalOrdersPastManagerService _globalOrdersPastManagerService;
        private IClientsManagerService _clientsManagerService;
        private IOrdersManagerService _ordersManagerService;

        public StatsViewModel(IGlobalOrdersPastManagerService globalOrdersPastManagerService,
                              IClientsManagerService clientsManagerService,
                              IOrdersManagerService ordersManagerService)
        {
            _globalOrdersPastManagerService = globalOrdersPastManagerService;
            _clientsManagerService = clientsManagerService;
            _ordersManagerService = ordersManagerService;
        }

        public List<GlobalOrderRegist> ProductsList { get; private set; }

        public override void Appearing()
        {
            var list = _globalOrdersPastManagerService.GlobalOrderRegists;
            list.Sort((a, b) => Math.Sign(a.OrderRegistDate.Date.Ticks - b.OrderRegistDate.Date.Ticks));
            ProductsList = list;
        }

        public override void DisAppearing()
        {
        }

        public float GetValue(List<ProductAmmount> itemsList, GetValueEnum getValueType, DateTime orderRegistDate)
        {
            double count = 0;

            if (getValueType == GetValueEnum.GetValueOut)
            {
                foreach (var item in itemsList)
                {
                    count += item.Product.GetCostValueWithIva() * item.Ammount;
                }
            }
            else if (getValueType == GetValueEnum.GetValueIn)
            {
                foreach (Client client in _clientsManagerService.ClientsList)
                {
                    if (!client.Active) continue;

                    ExtraOrder order = _clientsManagerService.HasOrderThisDate(client, orderRegistDate);

                    if (order == null)
                    {
                        count += _ordersManagerService.GetValue(client.Id, ClientHelper.GetDailyOrder(orderRegistDate.DayOfWeek, client));
                    }
                    else
                    {
                        if (order.IsTotal)
                        {
                            count += _ordersManagerService.GetValue(client.Id, order.AllItems);
                        }
                        else
                        {
                            count += _ordersManagerService.GetValue(client.Id, ClientHelper.GetDailyOrder(orderRegistDate.DayOfWeek, client));
                            count += _ordersManagerService.GetValue(client.Id, order.AllItems);
                        }
                    }
                }
            }

            return (float)count;
        }

        public float GetValueDiff(List<ProductAmmount> itemsList, DateTime orderRegistDate)
        {
            return GetValue(itemsList, GetValueEnum.GetValueIn, orderRegistDate) - GetValue(itemsList, GetValueEnum.GetValueOut, orderRegistDate);
        }
    }

    public enum GetValueEnum
    {
        GetValueIn,
        GetValueOut
    }
}
