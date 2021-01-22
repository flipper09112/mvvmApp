using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Interfaces.Helpers;

namespace tabApp.Core.ViewModels
{
    public class ClientPageViewModel : BaseViewModel
    {

        private readonly IChooseClientService _chooseClientService;
        private readonly IGetSpinnerDatesService _getSpinnerDatesService;
        public string ExtLabel { get; set; }
        public string TerLabel { get; set; }
        public string QuaLabel { get; set; }
        public string QuiLabel { get; set; }
        public string SexLabel { get; set; }
        public string SabLabel { get; set; }
        public string DomLabel { get; set; }
        public string SegLabel { get; set; }
        public string PayButtonText { get; set; }

        public ClientPageViewModel(IGetSpinnerDatesService getSpinnerDatesService, IChooseClientService chooseClientService)
        {
            _chooseClientService = chooseClientService;
            _getSpinnerDatesService = getSpinnerDatesService;

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

        public string PayDate => string.Format("Pago até dia: {0}", Client.PaymentDate.ToString("dd/MM/yyyy"));

        public List<TabsOptionsEnum> TabsOptions { 
            get
            {
                List<TabsOptionsEnum> tabs = new List<TabsOptionsEnum>();
                tabs.Add(TabsOptionsEnum.Mapa);
                tabs.Add(TabsOptionsEnum.Registo);
                return tabs;
            }
        }

        public List<DateTime> SpinnerDates => _getSpinnerDatesService.GetListDatesToPay(Client);

        public string AmmountToPay { 
            get
            {
                return "0,00€";
            }
        }


        public override void AppearingAsync()
        {
        }
    }

    public enum TabsOptionsEnum
    {
        Mapa,
        Registo
    }
}
