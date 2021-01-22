using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Helpers;
using tabApp.Core.Services.Interfaces.Orders;

namespace tabApp.Core.ViewModels
{
    public class ClientPageViewModel : BaseViewModel
    {

        private readonly IChooseClientService _chooseClientService;
        private readonly IGetSpinnerDatesService _getSpinnerDatesService;
        private readonly IOrdersManagerService _ordersManagerService;
        private readonly IAmmountToPayService _ammountToPayService;

        private DateTime _dateSelected;

        public string ExtLabel { get; set; }
        public string TerLabel { get; set; }
        public string QuaLabel { get; set; }
        public string QuiLabel { get; set; }
        public string SexLabel { get; set; }
        public string SabLabel { get; set; }
        public string DomLabel { get; set; }
        public string SegLabel { get; set; }
        public string PayButtonText { get; set; }

        public ClientPageViewModel(IGetSpinnerDatesService getSpinnerDatesService, 
                                   IChooseClientService chooseClientService,
                                   IOrdersManagerService ordersManagerService,
                                   IAmmountToPayService ammountToPayService)
        {
            _chooseClientService = chooseClientService;
            _getSpinnerDatesService = getSpinnerDatesService;
            _ordersManagerService = ordersManagerService;
            _ammountToPayService = ammountToPayService;

            SegLabel = "Segunda";
            TerLabel = "Terça";
            QuaLabel = "Quarta";
            QuiLabel = "Quinta";
            SexLabel = "Sexta";
            SabLabel = "Sábado";
            DomLabel = "Domingo";
            ExtLabel = "Extra";
            PayButtonText = "Pagamento";
        }

        public Client Client => _chooseClientService.ClientSelected;
        public List<DateTime> SpinnerDates => _getSpinnerDatesService.GetListDatesToPay(Client);
        public string PayDate => string.Format("Pago até dia: {0}", Client.PaymentDate.ToString("dd/MM/yyyy"));
        public string SegValue => _ordersManagerService.GetValue(Client.Id, Client.SegDailyOrder).ToString("C");
        public string TerValue => _ordersManagerService.GetValue(Client.Id, Client.TerDailyOrder).ToString("C");
        public string QuaValue => _ordersManagerService.GetValue(Client.Id, Client.QuaDailyOrder).ToString("C");
        public string QuiValue => _ordersManagerService.GetValue(Client.Id, Client.QuiDailyOrder).ToString("C");
        public string SexValue => _ordersManagerService.GetValue(Client.Id, Client.SexDailyOrder).ToString("C");
        public string SabValue => _ordersManagerService.GetValue(Client.Id, Client.SabDailyOrder).ToString("C");
        public string DomValue => _ordersManagerService.GetValue(Client.Id, Client.DomDailyOrder).ToString("C");
        public string ExtValue => Client.ExtraValueToPay.ToString("C");
        public string AmmountToPay => _ammountToPayService.Calculate(Client, DateSelected).ToString("C");

        public List<TabsOptionsEnum> TabsOptions { 
            get
            {
                List<TabsOptionsEnum> tabs = new List<TabsOptionsEnum>();
                tabs.Add(TabsOptionsEnum.Mapa);
                tabs.Add(TabsOptionsEnum.Registo);
                return tabs;
            }
        }

        public DateTime DateSelected
        {
            get
            {
                return _dateSelected;
            }
            set
            {
                _dateSelected = value;
                RaisePropertyChanged(nameof(DateSelected));
            }
        }

        public override void Appearing()
        {
            DateSelected = SpinnerDates[0];
        }
    }

    public enum TabsOptionsEnum
    {
        Mapa,
        Registo
    }
}
