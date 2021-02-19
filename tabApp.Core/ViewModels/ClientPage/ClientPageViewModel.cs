using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.DB;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Helpers;
using tabApp.Core.Services.Interfaces.Orders;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.ViewModels
{
    public class ClientPageViewModel : BaseViewModel
    {

        private readonly IChooseClientService _chooseClientService;
        private readonly IGetSpinnerDatesService _getSpinnerDatesService;
        private readonly IOrdersManagerService _ordersManagerService;
        private readonly IAmmountToPayService _ammountToPayService;
        private readonly IClientsManagerService _clientsManagerService;
        private readonly IMvxNavigationService _navigationService;
        private readonly IDBService _dBService;
        private readonly IDialogService _dialogService;
        private readonly IProductsManagerService _productsManagerService;

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
        public string AddExtraButtonText { get; set; }
        public string AddOrderButtonText { get; set; }
        public string EditButtonText { get; set; }
        public string OptionsButtonText { get; set; }

        public MvxCommand PayCommand;
        public MvxCommand AddExtraCommand;
        public MvxCommand AddOrderCommand;
        public MvxCommand ShowDailyOrdersDetailsCommand;
        public MvxCommand ShowExtraOptionsCommand;
        public MvxCommand<ExtraOrder> CancelOrderCommand;
        public MvxCommand ShoeEditPageCommand;
        public MvxCommand ShowDatePickerDialogCommand;

        public ClientPageViewModel(IGetSpinnerDatesService getSpinnerDatesService, 
                                   IChooseClientService chooseClientService,
                                   IOrdersManagerService ordersManagerService,
                                   IAmmountToPayService ammountToPayService,
                                   IClientsManagerService clientsManagerService,
                                   IMvxNavigationService navigationService,
                                   IDBService dBService,
                                   IDialogService dialogService,
                                   IProductsManagerService productsManagerService)
        {

            #region Services
            _chooseClientService = chooseClientService;
            _getSpinnerDatesService = getSpinnerDatesService;
            _ordersManagerService = ordersManagerService;
            _ammountToPayService = ammountToPayService;
            _clientsManagerService = clientsManagerService;
            _navigationService = navigationService;
            _dBService = dBService;
            _dialogService = dialogService;
            _productsManagerService = productsManagerService;
            #endregion

            #region Labels
            SegLabel = "Segunda";
            TerLabel = "Terça";
            QuaLabel = "Quarta";
            QuiLabel = "Quinta";
            SexLabel = "Sexta";
            SabLabel = "Sábado";
            DomLabel = "Domingo";
            ExtLabel = "Extra";
            PayButtonText = "Pagamento";
            AddExtraButtonText = "Extras";
            AddOrderButtonText = "Encomendas";
            EditButtonText = "Editar";
            OptionsButtonText = "Outras Opções";
            #endregion

            PayCommand = new MvxCommand(SetPayment, CanSetPayment);
            AddExtraCommand = new MvxCommand(AddExtra);
            AddOrderCommand = new MvxCommand(AddOrder);
            ShowDailyOrdersDetailsCommand = new MvxCommand(ShowDailyOrdersDetails);
            ShowExtraOptionsCommand = new MvxCommand(ShowExtraOptions);
            CancelOrderCommand = new MvxCommand<ExtraOrder>(CancelOrder);
            ShoeEditPageCommand = new MvxCommand(ShoeEditPage);
            ShowDatePickerDialogCommand = new MvxCommand(ShowDatePickerDialog);
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
                tabs.Add(TabsOptionsEnum.Encomendas);
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
                _chooseClientService.PayTo = value;
                PayCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(DateSelected));
            }
        }


        #region Actions

        private void CancelOrder(ExtraOrder order)
        {
            _dialogService.ShowConfirmDialog("Confirmar o cancelamento da encomenda", "Sim", ConfirmCancelOrder, "Não", order);
        }

        private void ConfirmCancelOrder(object obj)
        {
            IsBusy = true;
            var regist = _clientsManagerService.RemoveExtraOrder(Client, (ExtraOrder)obj); 
            _dBService.SaveClientData(Client);
            _dBService.RemoveRegist((ExtraOrder)obj);
            _dBService.SaveNewRegist(regist);
            IsBusy = false;
            RaisePropertyChanged(nameof(ConfirmCancelOrder));
        }

        public string GetOrderDesc(ExtraOrder obj)
        {
            string details = "";
            foreach(var item in obj.AllItems)
            {
                Product product = _productsManagerService.GetProductById(item.ProductId);
                details += product.Name + " - " + (product.Unity ? item.Ammount.ToString("N0") : item.Ammount.ToString("N2")) + "\n";
            }
            return details;
        }

        private async void ShowExtraOptions()
        {
            await _navigationService.Navigate<OtherOptionsViewModel>();
        }
        private async void ShowDailyOrdersDetails()
        {
            await _navigationService.Navigate<DailysOrdersDescViewModel>();
        }
        private async void AddOrder()
        {
            await _navigationService.Navigate<ClientOrderViewModel>();
        }

        private async void ShoeEditPage()
        {
            await _navigationService.Navigate<EditClientViewModel>();
        }
        private void AddExtra()
        {
            _dialogService.ShowInputDialog("Adicionar Extra", "Sim", AddExtraAction);
        }

        private void AddExtraAction(double extra)
        {
            IsBusy = true;
            var regist = _clientsManagerService.AddExtra(Client, extra);
            _dBService.SaveClientData(Client);
            _dBService.SaveNewRegist(regist);
            IsBusy = false;
            RaisePropertyChanged(nameof(AddExtraAction));
        }

        private void SetPayment()
        {
            _dialogService.ShowConfirmDialog("Confirmar Pagamento?", "Sim", ConfirmPayment);
        }

        private void ShowDatePickerDialog()
        {
            _dialogService.ShowDatePickerDialog(SetDateSelected);
        }

        private void SetDateSelected(DateTime obj)
        {
            DateSelected = obj;
        }

        private bool CanSetPayment()
        {
            return !(Client.PaymentDate.Date == DateSelected.Date && Client.ExtraValueToPay == 0);
        }

        private void ConfirmPayment(bool payExtra)
        {
            IsBusy = true;
            var regist = _clientsManagerService.SetPayment(Client, DateSelected, payExtra
                , payExtra ? _ammountToPayService.Calculate(Client, DateSelected) : _ammountToPayService.Calculate(Client, DateSelected) - Client.ExtraValueToPay);
            _dBService.SaveClientData(Client);
            _dBService.SaveNewRegist(regist);
            IsBusy = false;
            _navigationService.Close(this);
        }
        #endregion

        public override void Appearing()
        {
            DateSelected = SpinnerDates[0];
        }
        public override void DisAppearing()
        {
        }
    }

    public enum TabsOptionsEnum
    {
        Mapa,
        Registo,
        Encomendas
    }
}
