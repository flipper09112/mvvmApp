using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Helpers;
using tabApp.Core.Models;
using tabApp.Core.Models.Notifications;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Implementations.Faturation.Helpers;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Deliverys;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Notifications;
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
        private readonly INotificationsManagerService _notificationsManagerService;
        private readonly IDataBaseManagerService _dataBaseManagerService;
        private readonly IDeliverysManagerService _deliverysManagerService;
        public EventHandler DeleteClientEvent;
        public EventHandler UpdateOrderList;
        public EventHandler UpdateAllTabs;
        public EventHandler ShowOptionsLongPress; 
        public MvxAsyncCommand<Client> ShowClientPage { get; private set; }
        public MvxCommand<Client> LongClickClient { get; private set; }
        public MvxCommand<int> DeleteClientCommand { get; private set; }
        public MvxCommand<int> StopDailysClientCommand { get; private set; }
        public MvxCommand AddNewClientBeforeCommand { get; private set; }
        public MvxCommand AddNewClientAfterCommand { get; private set; }
        public MvxCommand ShowRemainingProductsCommand { get; private set; }
        public MvxCommand ShowRemainingProductsTomorrowCommand { get; private set; }
        
        public MvxCommand<ExtraOrder> AddExtraFromOrderCommand { get; private set; }

        public HomeViewModel(IMvxNavigationService navigationService, 
                            IClientsManagerService clientsManagerService,
                            IChooseClientService chooseClientService,
                            IClientsListFilterService clientsListFilterService,
                            IOrdersManagerService ordersManagerService,
                            IProductsManagerService productsManagerService,
                            IDialogService dialogService,
                            INotificationsManagerService notificationsManagerService,
                            IDataBaseManagerService dataBaseManagerService,
                            IDeliverysManagerService deliverysManagerService)
        {
            _clientsManagerService = clientsManagerService;
            _navigationService = navigationService;
            _chooseClientService = chooseClientService;
            _clientsListFilterService = clientsListFilterService;
            _ordersManagerService = ordersManagerService;
            _productsManagerService = productsManagerService;
            _dialogService = dialogService;
            _notificationsManagerService = notificationsManagerService;
            _dataBaseManagerService = dataBaseManagerService;
            _deliverysManagerService = deliverysManagerService;

            ShowClientPage = new MvxAsyncCommand<Client>(ShowClientPageAction);
            DeleteClientCommand = new MvxCommand<int>(DeleteClient);
            StopDailysClientCommand = new MvxCommand<int>(StopDailysClient);
            LongClickClient = new MvxCommand<Client>(LongClickClientAction);
            AddNewClientBeforeCommand = new MvxCommand(AddNewClientBefore);
            AddNewClientAfterCommand = new MvxCommand(AddNewClientAfter);
            AddExtraFromOrderCommand = new MvxCommand<ExtraOrder>(AddExtraFromOrder);
            ShowRemainingProductsCommand = new MvxCommand(ShowRemainingProducts);
            ShowRemainingProductsTomorrowCommand = new MvxCommand(ShowRemainingProductsTomorrow);
        }
        
        private void ShowRemainingProductsTomorrow()
        {
            _dialogService.Show("Produtos necessários (dia seguinte)", GetRemainingProducts(_clientSelectedLongPress, DateTime.Today.AddDays(1)));
        }

        private void ShowRemainingProducts()
        {
            _dialogService.Show("Produtos necessários", GetRemainingProducts(_clientSelectedLongPress));
        }

        private void AddExtraFromOrder(ExtraOrder order)
        {
            _dialogService.ShowConfirmDialog("Confirmar a adicao do extra desta encomenda", "Sim", AddOrderExtra, "Não", order);
        }
        private void AddOrderExtra(object obj)
        {
            IsBusy = true;
            var extraOrder = (ExtraOrder)obj;
            Client client = _clientsManagerService.ClientsList.Find(cli => cli.Id == extraOrder.ClientId);

            Regist regist;
            if (!extraOrder.IsTotal)
            {
                var ammount = _ordersManagerService.GetValue(extraOrder.Id, extraOrder.AllItems);
                client.AddExtra(ammount);
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
                var dayAmmount = GetDayAmmount(client, extraOrder);
                var ammount = orderAmmount - dayAmmount;
                client.AddExtra(ammount);
                extraOrder.AmmountedAdded = true;

                regist = new Regist()
                {
                    DetailRegistDay = DateTime.Today,
                    Info = "Adicionado um extra de " + ammount.ToString("C") + " (removido o valor do dia " + dayAmmount.ToString("C") + ") referente a uma encomenda de dia " + extraOrder.OrderDay.ToString("dd/MM/yyyy"),
                    ClientId = extraOrder.Id,
                    DetailType = DetailTypeEnum.AddExtra
                };
            }

            _dataBaseManagerService.SaveClient(client, regist);
            _dataBaseManagerService.UpdateOrder(extraOrder);
            _dialogService.ShowSuccessChangeSnackBar("Adicionado extra com sucesso");
            UpdateOrderList?.Invoke(null, null);

            _chooseClientService.SelectClient(_clientsManagerService.GetClientById(extraOrder.ClientId));
            RaisePropertyChanged(nameof(AddOrderExtra));

            IsBusy = false;
        }

        private double GetDayAmmount(Client client, ExtraOrder extraOrder)
        {
            List<(DateTime Date, int days)> days = new List<(DateTime Date, int days)>()
            {
                (DateTime.Parse("12/24/2022 07:00:00"), 3),
                (DateTime.Parse("12/31/2022 07:00:00"), 3),
            };

            foreach(var date in days)
            {
                if(date.Date.Day == extraOrder.OrderDay.Day
                    && date.Date.Month == extraOrder.OrderDay.Month)
                {
                    string daysLabel = "";
                    for(int i = 1; i < date.days; i++)
                    {
                        daysLabel += (date.Date.AddDays(i).ToString("dd/MM") + "\n");
                    }

                    _dialogService.Show("Dia de dobra", "O extra foi adicionado tendo em consideração que nos seguintes dias não se trabalha!\n" + daysLabel);

                    double value = 0;
                    for(int i = 0 ; i < date.days; i++)
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

        public List<SecondaryOptions> GetTabsOptions()
        {
            TabsOptions = GetSecondaryOptions();
            return TabsOptions;
        }

        private async void AddNewClientAfter()
        {
            var newClient = AddNewTemplateClient(AddClientsTypesEnum.After);

            _chooseClientService.SelectClient(newClient);
            await _navigationService.Navigate<EditClientViewModel>();
        }

        private async void AddNewClientBefore()
        {
            var newClient = AddNewTemplateClient(AddClientsTypesEnum.Before);

            _chooseClientService.SelectClient(newClient);
            await _navigationService.Navigate<EditClientViewModel>();
        }

        private void FixNewClientPositions(AddClientsTypesEnum addClientsType)
        {
            int pos = _clientsManagerService.ClientsList.IndexOf(_clientSelectedLongPress);
            if (pos == -1) return;

            List<Client> list;
            if (addClientsType == AddClientsTypesEnum.Before)
            {
                list = _clientsManagerService.ClientsList.GetRange(pos, _clientsManagerService.ClientsList.Count - pos);
            }
            else if (addClientsType == AddClientsTypesEnum.After)
            {
                list = _clientsManagerService.ClientsList.GetRange(pos + 1, _clientsManagerService.ClientsList.Count - pos - 1);
            }
            else
            {
                list = null;
            }
            
            foreach(Client client in list)
            {
                client.Position = client.Position + 1;
                _dataBaseManagerService.SaveClient(client, regist: null);
            }
        }

        private Client AddNewTemplateClient(AddClientsTypesEnum addClientsType)
        {
            var newClient = new Client()
            {
                Id = _clientsManagerService.GetNewId(),
                Position = addClientsType == AddClientsTypesEnum.Before ? _clientSelectedLongPress.Position : _clientSelectedLongPress.Position + 1,
                Name = "Default",
                SubName = "Sem alcunha definida",
                Address = new Address()
                {
                    AddressDesc = "Sem morada definida",
                    NumberDoor = 0,
                    Coordenadas = "null"
                },
                PaymentDate = DateTime.Today,
                PaymentType = PaymentTypeEnum.Diario,
                Active = true,
                ExtraValueToPay = 0,
                DailyOrders = CreateEmptyDailyyOrder(),
                PhoneNumber = "Sem numero",
                LastChangeDate = DateTime.Today,
                DetailsList = new List<Regist>(),
                ExtraOrdersList = new List<ExtraOrder>(),
                Delivery = _deliverysManagerService.Deliveries[0]
            };

            FixNewClientPositions(addClientsType);
            _clientsManagerService.AddNewClient(newClient);
            _dataBaseManagerService.InsertClient(
                newClient, 
                new Regist() { 
                    DetailRegistDay = DateTime.Today,
                    Info = "Criação da ficha do cliente",
                    DetailType = DetailTypeEnum.NewClient
                }
            );

            return newClient;
        }

        private List<DailyOrder> CreateEmptyDailyyOrder()
        {
            var dailyOrder = new List<DailyOrder>();

            foreach (DayOfWeek dayOfWeek in (DayOfWeek[])Enum.GetValues(typeof(DayOfWeek)))
            {
                dailyOrder.Add(new DailyOrder()
                {
                    AllItems = new List<DailyOrderDetails>(),
                    DayOfWeek = dayOfWeek
                });
            }

            return dailyOrder;
        }

        private void LongClickClientAction(Client client)
        {
            _clientSelectedLongPress = client;
            ShowOptionsLongPress?.Invoke(null, null);
        }

        private async void StopDailysClient(int arg)
        {
            _chooseClientService.SelectClient(ClientsList[arg]);
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
                List<Client> clients;
                if (_clientsManagerService.DeliveryId == SecureStorageHelper.DeliveryIdAdmin)
                    clients = _clientsManagerService.ClientsList;
                else
                    clients = _clientsManagerService.ClientsList?.FindAll(item => item.DeliveryId.ToString() == _clientsManagerService.DeliveryId);

                if (_clientsListFilterService.HasFilter)
                {
                    return _clientsListFilterService.FilterClients(clients);
                }

                return clients;
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

        private List<LongPressItem> _longPressItemsList;
        private Client _clientSelectedLongPress;

        public List<LongPressItem> LongPressItemsList
        {
            get
            {
                return _longPressItemsList;
            }
            set
            {
                _longPressItemsList = value;
            }
        }

        public override void Appearing()
        {
            TabsOptions = GetSecondaryOptions();
            LongPressItemsList = GetLongPressItemsList();

            if(_dataBaseManagerService.DBRestored)
            {
                _dataBaseManagerService.DBRestored = false;
                UpdateAllTabs?.Invoke(null, null);
            }
        }

        private List<LongPressItem> GetLongPressItemsList()
        {
            var longPressItemsList = new List<LongPressItem>();
            longPressItemsList.Add(new LongPressItem() { 
                Name = "Adicionar Cliente\n(posição anterior)",
                Command = AddNewClientBeforeCommand
            }); 
            longPressItemsList.Add(new LongPressItem()
            {
                Name = "Adicionar Cliente\n(posição seguinte)",
                Command = AddNewClientAfterCommand
            });
            longPressItemsList.Add(new LongPressItem()
            {
                Name = "Produtos necessários",
                Command = ShowRemainingProductsCommand
            });
            longPressItemsList.Add(new LongPressItem()
            {
                Name = "Produtos necessários\n(Dia seguinte)",
                Command = ShowRemainingProductsTomorrowCommand
            });
            return longPressItemsList;
        }

        private List<SecondaryOptions> GetSecondaryOptions()
        {
            List<SecondaryOptions> items = new List<SecondaryOptions>();
            items.Add(new OrdersPage("Encomendas", _ordersManagerService.TodayOrders?.FindAll(item => ApplyDeliveryFilter(item.Client.Delivery))));
            items.Add(new NotificationsPage("Notificações",
                _notificationsManagerService.TodayNotifications?.FindAll(item => ApplyDeliveryFilter(_clientsManagerService.GetClientById(item.ClientId).Delivery))));
            items.Add(new SecondaryOptions("Localização"));
            return items;
        }

        private bool ApplyDeliveryFilter(Delivery delivery)
        {
            if (_clientsManagerService.DeliveryId == SecureStorageHelper.DeliveryIdAdmin)
                return true;

            else
                return _clientsManagerService.DeliveryId == delivery.DeliveryId.ToString();
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

        public string GetRemainingProducts(Client client, DateTime? date = null)
        {
            string txt = "";
            var list = _ordersManagerService.GetTotalOrderFromClient(client, date ?? DateTime.Today, ClientsList);

            foreach (var item in list)
            {
                txt += item.Product.Name + " - " + item.Ammount.ToString("N2") + "\n";
            }

            return txt;
        }

        public Client GetClient(int clientId)
        {
            return _clientsManagerService.ClientsList.Find(item => item.Id == clientId);
        }
    }

    public enum AddClientsTypesEnum
    {
        After,
        Before
    }

    public class LongPressItem
    {
        public string Name { get; set; }
        public LongPressItemType Type { get; set; }
        public MvxCommand Command { get; set; }
    }

    public enum LongPressItemType
    {
        Sell, 
        Buy
    }

    public class SecondaryOptions
    {
        public string Name { get; }
        public int Count { get; set; }

        public SecondaryOptions(string name)
        {
            Name = name;
            Count = 0;
        }
    }

    public class OrdersPage : SecondaryOptions
    {
        public List<(Client Client, ExtraOrder ExtraOrder)> Value { get; }

        public OrdersPage(string name, List<(Client Client, ExtraOrder ExtraOrder)> value) : base(name)
        {
            Value = value;
            Count = Value?.Count ?? 0;
        }
    }
    public class NotificationsPage : SecondaryOptions
    {
        public List<Notification> Value { get; }

        public NotificationsPage(string name, List<Notification> value) : base(name)
        {
            Value = value;
            Count = Value?.Count ?? 0;
        }
    }
    
}
