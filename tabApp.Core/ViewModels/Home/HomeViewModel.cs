using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Orders;
using tabApp.Core.Services.Interfaces.Products;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Home;

namespace tabApp.Core
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IClientsManagerService _clientsManagerService;
        private readonly IChooseClientService _chooseClientService;
        private readonly IClientsListFilterService _clientsListFilterService;
        private readonly IOrdersManagerService _ordersManagerService;
        private readonly IProductsManagerService _productsManagerService;
        private readonly IDialogService _dialogService;


        public EventHandler DeleteClientEvent;
        public MvxAsyncCommand<Client> ShowClientPage { get; private set; }
        public MvxCommand<int> DeleteClientCommand { get; private set; }
        public MvxCommand<int> StopDailysClientCommand { get; private set; }

        public HomeViewModel(IMvxNavigationService navigationService, 
                            IClientsManagerService clientsManagerService,
                            IChooseClientService chooseClientService,
                            IClientsListFilterService clientsListFilterService,
                            IOrdersManagerService ordersManagerService,
                            IProductsManagerService productsManagerService,
                            IDialogService dialogService)
        {
            _clientsManagerService = clientsManagerService;
            _navigationService = navigationService;
            _chooseClientService = chooseClientService;
            _clientsListFilterService = clientsListFilterService;
            _ordersManagerService = ordersManagerService;
            _productsManagerService = productsManagerService;
            _dialogService = dialogService;

            ShowClientPage = new MvxAsyncCommand<Client>(ShowClientPageAction);
            DeleteClientCommand = new MvxCommand<int>(DeleteClient);
            StopDailysClientCommand = new MvxCommand<int>(StopDailysClient);
        }

        private async void StopDailysClient(int arg)
        {
            _chooseClientService.SelectClient(_clientsManagerService.ClientsList[arg]);
            if(_clientsManagerService.ClientsList[arg].Active)
            {
                await _navigationService.Navigate<StopDailyViewModel>();
            }else
            {
                await _navigationService.Navigate<InitDailyViewModel>();
            }
        }

        private async void DeleteClient(int arg)
        {
            _chooseClientService.SelectClient(ClientsList[arg]);
            await _navigationService.Navigate<DeleteClientViewModel>();
        }

        public Client ClientSelected => _chooseClientService.ClientSelected;
        private async Task ShowClientPageAction(Client clientSelected)
        {
            _chooseClientService.SelectClient(clientSelected);
            await _navigationService.Navigate<ClientPageViewModel>();
        }

        public List<SecondaryOptions> RefreshTabOptions()
        {
            return GetSecondaryOptions();
        }

        public string GetClientDailyOrderDesc(Client client)
        {
            string txt = "";

            foreach (var item in _clientsManagerService.GetTodayDailyOrder(client, DateTime.Today.DayOfWeek).AllItems)
            {
                txt += _productsManagerService.GetProductById(item.ProductId).Name + " - " + item.Ammount.ToString("N2") + "\n";
            }

            return txt;
        }

        public bool CheckClientHasExtraOrderToday(Client client)
        {
            return _clientsManagerService.ClientHasExtraOrderThisDay(client, DateTime.Today);
        }

        public List<Client> ClientsList
        {
            get
            {
                if(_clientsListFilterService.HasFilter)
                {
                    return _clientsListFilterService.FilterClients(_clientsManagerService.ClientsList);
                }

                return _clientsManagerService.ClientsList;
            }
        }

        private List<SecondaryOptions> _tabsOptions;
        public List<SecondaryOptions> TabsOptions
        {
            get
            {
                return _tabsOptions;
            }
            set
            {
                _tabsOptions = value;
               //RaisePropertyChanged(nameof(TabsOptions));
            }
        }

        public override void Appearing()
        {
            TabsOptions = GetSecondaryOptions();
        }

        private List<SecondaryOptions> GetSecondaryOptions()
        {
            List<SecondaryOptions> items = new List<SecondaryOptions>();
            items.Add(new OrdersPage("Encomendas", _ordersManagerService.TodayOrders));
            items.Add(new SecondaryOptions("Localização"));
            return items;
        }
        public string GetOrderDesc(ExtraOrder obj)
        {
            string details = "";
            foreach (var item in obj.AllItems)
            {
                Product product = _productsManagerService.GetProductById(item.ProductId);
                details += product.Name + " - " + (product.Unity ? item.Ammount.ToString("N0") : item.Ammount.ToString("N2")) + "\n";
            }
            return details;
        }

        public override void DisAppearing()
        {
        }

        public string GetRemainingProducts(double latitude, double longitude)
        {
            string txt = "";
            Client client = _clientsManagerService.GetClosestClient(latitude, longitude);

            var list = _ordersManagerService.GetTotalOrderFromClient(client, DateTime.Today);

            foreach (var item in list)
            {
                txt += item.Product.Name + " - " + item.Ammount.ToString("N2") + "\n";
            }

            return txt;
        }
    }

    public class SecondaryOptions
    {
        public string Name { get; }

        public SecondaryOptions(string name)
        {
            Name = name;
        }
    }

    public class OrdersPage : SecondaryOptions
    {
        public List<(Client Client, ExtraOrder ExtraOrder)> Value { get; }

        public OrdersPage(string name, List<(Client Client, ExtraOrder ExtraOrder)> value) : base(name)
        {
            Value = value;
        }
    }
}
