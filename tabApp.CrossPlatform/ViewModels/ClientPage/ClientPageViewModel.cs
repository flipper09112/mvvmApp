using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Implementations.DB;
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
        private readonly IDataBaseManagerService _dataBaseManagerService;
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
        public MvxCommand<ExtraOrder> AddExtraFromOrderCommand;
        public MvxCommand ShowDatePickerDialogCommand; 

        public ClientPageViewModel(IGetSpinnerDatesService getSpinnerDatesService, 
                                   IChooseClientService chooseClientService,
                                   IOrdersManagerService ordersManagerService,
                                   IAmmountToPayService ammountToPayService,
                                   IClientsManagerService clientsManagerService,
                                   IMvxNavigationService navigationService,
                                   IDialogService dialogService,
                                   IProductsManagerService productsManagerService,
                                   IDataBaseManagerService dataBaseManagerService)
        {

            #region Services
            _chooseClientService = chooseClientService;
            _getSpinnerDatesService = getSpinnerDatesService;
            _ordersManagerService = ordersManagerService;
            _ammountToPayService = ammountToPayService;
            _clientsManagerService = clientsManagerService;
            _navigationService = navigationService;
            _dataBaseManagerService = dataBaseManagerService;
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
            AddExtraFromOrderCommand = new MvxCommand<ExtraOrder>(AddExtraFromOrder);
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
                List<TabsOptionsEnum> tabs = new List<TabsOptionsEnum>
                {
                    TabsOptionsEnum.Mapa,
                    TabsOptionsEnum.Registo,
                    TabsOptionsEnum.Encomendas
                };
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

        private void AddExtraFromOrder(ExtraOrder order)
        {
            _dialogService.ShowConfirmDialog("Confirmar a adicao do extra desta encomenda", "Sim", AddOrderExtra, "Não", order);
        }

        private void AddOrderExtra(object obj)
        {
            IsBusy = true;
            var extraOrder = (ExtraOrder)obj;

            Regist regist;
            if (!extraOrder.IsTotal)
            {
                var ammount = _ordersManagerService.GetValue(extraOrder.Id, extraOrder.AllItems);
                Client.AddExtra(ammount);
                extraOrder.AmmountedAdded = true;

                regist = new Regist()
                {
                    DetailRegistDay = DateTime.Today,
                    Info = "Adicionado um extra de " + ammount.ToString("C") + " referente a uma encomenda de dia " + extraOrder.OrderDay.ToString("dd/MM/yyyy"),
                    ClientId = extraOrder.Id,
                    DetailType = DetailTypeEnum.AddExtra
                };
            }
            else
            {
                var orderAmmount = _ordersManagerService.GetValue(extraOrder.Id, extraOrder.AllItems);
                var dayAmmount = GetDayAmmount(Client, extraOrder);
                var ammount = orderAmmount - dayAmmount; 
                Client.AddExtra(ammount);
                extraOrder.AmmountedAdded = true;

                regist = new Regist()
                {
                    DetailRegistDay = DateTime.Today,
                    Info = "Adicionado um extra de " + ammount.ToString("C") + " (removido o valor do dia " + dayAmmount.ToString("C") + ") referente a uma encomenda de dia " + extraOrder.OrderDay.ToString("dd/MM/yyyy"),
                    ClientId = extraOrder.Id,
                    DetailType = DetailTypeEnum.AddExtra
                };
            }

            _dataBaseManagerService.SaveClient(Client, regist);
            _dataBaseManagerService.UpdateOrder(extraOrder);
            _dialogService.ShowSuccessChangeSnackBar("Adicionado extra com sucesso");
            RaisePropertyChanged(nameof(AddOrderExtra));
            IsBusy = false;
        }

        private double GetDayAmmount(Client client, ExtraOrder extraOrder)
        {
            List<(DateTime Date, int days)> days = new List<(DateTime Date, int days)>()
            {
                (DateTime.ParseExact("12/24/2022 07:00:00", "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture), 3),
                (DateTime.ParseExact("12/31/2022 07:00:00", "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture), 3),
            };

            foreach (var date in days)
            {
                if (date.Date.Day == extraOrder.OrderDay.Day
                    && date.Date.Month == extraOrder.OrderDay.Month)
                {
                    string daysLabel = "";
                    for (int i = 1; i < date.days; i++)
                    {
                        daysLabel += (date.Date.AddDays(i).ToString("dd/MM") + "\n");
                    }

                    _dialogService.Show("Dia de dobra", "O extra foi adicionado tendo em consideração que nos seguintes dias não se trabalha!\n" + daysLabel);

                    double value = 0;
                    for (int i = 0; i < date.days; i++)
                    {
                        value += _ordersManagerService.GetValue(extraOrder.Id,
                                                                _clientsManagerService.GetTodayDailyOrder(client,
                                                                                                          extraOrder.OrderDay.AddDays(i).DayOfWeek));
                    }

                    return value;
                }
            }

            return _ordersManagerService.GetValue(extraOrder.Id, _clientsManagerService.GetTodayDailyOrder(client, extraOrder.OrderDay.DayOfWeek));
        }

        private void CancelOrder(ExtraOrder order)
        {
            _dialogService.ShowConfirmDialog("Confirmar o cancelamento da encomenda", "Sim", ConfirmCancelOrder, "Não", order);
        }

        private void ConfirmCancelOrder(object obj)
        {
            IsBusy = true;
            var regist = _clientsManagerService.RemoveExtraOrder(Client, (ExtraOrder)obj);
            _dataBaseManagerService.RemoveExtraOrder(Client, (ExtraOrder)obj, regist);
            IsBusy = false;
            RaisePropertyChanged(nameof(ConfirmCancelOrder));
        }

        public string GetOrderDesc(ExtraOrder obj)
        {
            string details = "";
            foreach(var item in obj.AllItems)
            {
                Product product = _productsManagerService.GetProductById(item.ProductId);

                if (product == null)
                    details += "Produto nao encontrado - " + item.Ammount.ToString("N3") + "\n";
                else
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
            _dataBaseManagerService.SaveClient(Client, regist);
            IsBusy = false;
            RaisePropertyChanged(nameof(AddExtraAction));
        }

        private void SetPayment()
        {
            if(Client.ExtraOrdersList.Any(order => order.OrderDay.Date > Client.PaymentDate.Date && order.OrderDay.Date <= DateSelected.Date && !(order.AmmountedAdded ?? false)))
            {
                _dialogService.ShowErrorDialog("Alerta", 
                                               "Existem encomendas que não foram adicionadas ao extra!\nPoderão ter sido adicionadas manualmente.\nPretende continuar com o processo de pagamento?",
                                               ContinuePayment);
                return;
            }
            _dialogService.ShowConfirmDialog("Confirmar Pagamento?", "Sim", ConfirmPayment);
        }

        private void ContinuePayment()
        {
            _dialogService.ShowConfirmDialog("Confirmar Pagamento?", "Sim", ConfirmPayment);
        }

        private void ShowDatePickerDialog()
        {
            _dialogService.ShowDatePickerDialog(SetDateSelected, false);
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
            _dataBaseManagerService.SaveClient(Client, regist);
            IsBusy = false;
            _navigationService.Close(this);
        }
        #endregion

        public override void Appearing()
        {
            DateSelected = SpinnerDates[0];

            var outDatedProductsList = _productsManagerService.CheckProductsNotUpdated(Client);
            if (outDatedProductsList.Count > 0)
            {
                List<string> prods = outDatedProductsList.Select(x => x.Name + "\n").ToList();

                _dialogService.ShowErrorDialog("Produtos não atualizados", "O cliente contém produtos que nao tem os preços atualizados: \n\n" + prods.Aggregate((a, b) => a + b) + "\n");
            }
        }
        public override void DisAppearing()
        {
        }

        public bool IsLojaClient(int clientId)
        {
            return _clientsManagerService.ClientsList.Find(client => client.Id == clientId).PaymentType == PaymentTypeEnum.Loja;
        }
    }

    public enum TabsOptionsEnum
    {
        Mapa,
        Registo,
        Encomendas
    }
}
