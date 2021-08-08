using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Helpers;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Orders;

namespace tabApp.Core.ViewModels.Global.Other
{
    public class HomeFinancialsViewModel : BaseViewModel
    {
        private readonly IOrdersManagerService _ordersManagerService;
        private readonly IDialogService _dialogService;
        private readonly IClientsManagerService _clientsManagerService;

        public List<ProductAmmount> ProductsList { get; set; }

        public MvxCommand SelectDateCommand { get; set; }

        public HomeFinancialsViewModel(IOrdersManagerService ordersManagerService,
                                       IDialogService dialogService,
                                       IClientsManagerService clientsManagerService)
        {
            _ordersManagerService = ordersManagerService;
            _dialogService = dialogService;
            _clientsManagerService = clientsManagerService;

            SelectDateCommand = new MvxCommand(SelectDate);
        }

        public DateTime _dateSelected = DateTime.Today;
        public DateTime DateSelected
        {
            get
            {
                return _dateSelected;
            }
            set
            {
                _dateSelected = value; 
                ProductsList = _ordersManagerService.GetTotalOrder(_dateSelected);
                RaisePropertyChanged(nameof(DateSelected));
            }
        }

        public string SaldoValue
        {
            get
            {
                double count = GetInValue() - GetOutValue(); 
                return count.ToString("C");
            }
        }

        public string OutValue
        {
            get
            {
                double count = GetOutValue();
                return count.ToString("C");
            }
        }

        public string InValue 
        { 
            get
            {
                double count = GetInValue();
                return count.ToString("C");
            }
        }

        private double GetInValue()
        {
            double count = 0;

            foreach (Client client in _clientsManagerService.ClientsList)
            {
                if (!client.Active) continue;

                ExtraOrder order = _clientsManagerService.HasOrderThisDate(client, DateSelected);

                if (order == null)
                {
                    count += _ordersManagerService.GetValue(client.Id, ClientHelper.GetDailyOrder(DateSelected.DayOfWeek, client));
                }
                else
                {
                    if (order.IsTotal)
                    {
                        count += _ordersManagerService.GetValue(client.Id, order.AllItems);
                    }
                    else
                    {
                        count += _ordersManagerService.GetValue(client.Id, ClientHelper.GetDailyOrder(DateSelected.DayOfWeek, client));
                        count += _ordersManagerService.GetValue(client.Id, order.AllItems);
                    }
                }
            }

            return count;
        }
        private double GetOutValue()
        {
            return 0;
        }

        private void SelectDate()
        {
            _dialogService.ShowDatePickerDialog(SelectDateAction, false);
        }

        private void SelectDateAction(DateTime obj)
        {
            DateSelected = obj;
        }

        public override void Appearing()
        {
            ProductsList = _ordersManagerService.GetTotalOrder(DateSelected);
        }

        public override void DisAppearing()
        {
        }
    }
}
