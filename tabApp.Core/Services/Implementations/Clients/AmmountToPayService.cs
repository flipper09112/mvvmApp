﻿using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Orders;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.Services.Implementations.Clients
{
    public class AmmountToPayService : IAmmountToPayService
    {
        private readonly IProductsManagerService _productsManagerService;
        private readonly IOrdersManagerService _ordersManagerService;

        public AmmountToPayService(IProductsManagerService productsManagerService,
                                   IOrdersManagerService ordersManagerService)
        {
            _productsManagerService = productsManagerService;
            _ordersManagerService = ordersManagerService;
        }

        public double Calculate(Client client, DateTime payTo)
        {
            switch(client.PaymentType)
            {
                case PaymentTypeEnum.Diario:
                    return _ordersManagerService.GetValue(client.Id, GetOrderDay(client));
                case PaymentTypeEnum.Semanal:
                    return CalculateWeekValue(client, payTo);
                case PaymentTypeEnum.Mensal:
                    return CalculateMensalValue(client, payTo);
                case PaymentTypeEnum.JuntaDias:
                    return CalculateJuntaDiasValue(client, payTo);
                case PaymentTypeEnum.Loja:
                    return -1;
                default:
                    return -1;
            }
        }

        private double CalculateJuntaDiasValue(Client client, DateTime payTo)
        {
            double ammount = 0;
            DateTime temp = client.PaymentDate;
            temp = temp.AddDays(1);

            while ((temp - payTo.AddDays(1)).TotalDays != 0)
            {
                if (temp.DayOfWeek == DayOfWeek.Monday)
                {
                    ammount += _ordersManagerService.GetValue(client.Id, client.SegDailyOrder);
                }
                else if (temp.DayOfWeek == DayOfWeek.Tuesday)
                {
                    ammount += _ordersManagerService.GetValue(client.Id, client.TerDailyOrder);
                }
                else if (temp.DayOfWeek == DayOfWeek.Wednesday)
                {
                    ammount += _ordersManagerService.GetValue(client.Id, client.QuaDailyOrder);
                }
                else if (temp.DayOfWeek == DayOfWeek.Thursday)
                {
                    ammount += _ordersManagerService.GetValue(client.Id, client.QuiDailyOrder);
                }
                else if (temp.DayOfWeek == DayOfWeek.Friday)
                {
                    ammount += _ordersManagerService.GetValue(client.Id, client.SexDailyOrder);
                }
                else if (temp.DayOfWeek == DayOfWeek.Saturday)
                {
                    ammount += _ordersManagerService.GetValue(client.Id, client.SabDailyOrder);
                }
                else if (temp.DayOfWeek == DayOfWeek.Sunday)
                {
                    ammount += _ordersManagerService.GetValue(client.Id, client.DomDailyOrder);
                }
                temp = temp.AddDays(1);
            }

            return ammount + client.ExtraValueToPay;
        }

        private double CalculateMensalValue(Client client, DateTime payTo)
        {
            double ammount = 0;
            DateTime temp = client.PaymentDate;

            if (payTo.Date == temp.Date)
                 return 0 + client.ExtraValueToPay;

            if (!(temp.Day == 1))
                temp = temp.AddDays(1);
            
            while ((temp - payTo.AddDays(1)).TotalDays != 0)
            {
                if (temp.DayOfWeek == DayOfWeek.Monday)
                {
                    ammount += _ordersManagerService.GetValue(client.Id, client.SegDailyOrder); 
                }
                else if (temp.DayOfWeek == DayOfWeek.Tuesday)
                {
                    ammount += _ordersManagerService.GetValue(client.Id, client.TerDailyOrder);
                }
                else if (temp.DayOfWeek == DayOfWeek.Wednesday)
                {
                    ammount += _ordersManagerService.GetValue(client.Id, client.QuaDailyOrder);
                }
                else if (temp.DayOfWeek == DayOfWeek.Thursday)
                {
                    ammount += _ordersManagerService.GetValue(client.Id, client.QuiDailyOrder);
                }
                else if (temp.DayOfWeek == DayOfWeek.Friday)
                {
                    ammount += _ordersManagerService.GetValue(client.Id, client.SexDailyOrder);
                }
                else if (temp.DayOfWeek == DayOfWeek.Saturday)
                {
                    ammount += _ordersManagerService.GetValue(client.Id, client.SabDailyOrder);
                }
                else if (temp.DayOfWeek == DayOfWeek.Sunday)
                {
                    ammount += _ordersManagerService.GetValue(client.Id, client.DomDailyOrder);
                }
                temp = temp.AddDays(1);
            }

            return ammount + client.ExtraValueToPay;
        }

        private double CalculateWeekValue(Client client, DateTime payTo)
        {
            int diffDays = (int)Math.Abs((client.PaymentDate - payTo).TotalDays);

            if (diffDays == 0)
                return 0 + client.ExtraValueToPay;

            int diffweeks = diffDays / 7 + 1;
            if (diffDays % 7 == 0)
                diffweeks -= 1;

            double weekAmmount = _ordersManagerService.WeekAmmount(client);

            return weekAmmount * diffweeks + client.ExtraValueToPay;
        }

        private DailyOrder GetOrderDay(Client client)
        {
            switch(DateTime.Today.DayOfWeek)
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