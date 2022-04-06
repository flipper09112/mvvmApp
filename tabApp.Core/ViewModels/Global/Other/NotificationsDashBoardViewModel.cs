using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Models.Notifications;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Notifications;
using tabApp.Core.Services.Interfaces.Orders;
using tabApp.Core.Services.Interfaces.Products;
using tabApp.Core.ViewModels;

namespace tabApp.UI.Fragments.Global.Other
{
    public class NotificationsDashBoardViewModel : BaseViewModel
    {
        private IMvxNavigationService _navigationService;
        private IOrdersManagerService _ordersManagerService;
        private IDialogService _dialogService;
        private INotificationsManagerService _notificationsManagerService;
        private IClientsManagerService _clientsManagerService;
        private IProductsManagerService _productsManagerService;
        private IDataBaseManagerService _dataBaseManagerService;
        private IChooseClientService _chooseClientService;

        public NotificationsType NotificationsTypeSelected { get; set; }
        public List<NotificationsType> NotificationsTypesList { get; set; }
        public MvxCommand<ExtraOrder> AddExtraFromOrderCommand { get; private set; }
        public MvxCommand DateSelectCommand { get; private set; }
        public MvxCommand<NotificationsType> SelectFilterCommand { get; private set; }
        
        public NotificationsDashBoardViewModel(IMvxNavigationService navigationService,
                                               IOrdersManagerService ordersManagerService,
                                               IDialogService dialogService,
                                               INotificationsManagerService notificationsManagerService,
                                               IClientsManagerService clientsManagerService,
                                               IProductsManagerService productsManagerService,
                                               IDataBaseManagerService dataBaseManagerService,
                                               IChooseClientService chooseClientService)
        {
            _navigationService = navigationService;
            _ordersManagerService = ordersManagerService;
            _dialogService = dialogService;
            _notificationsManagerService = notificationsManagerService;
            _clientsManagerService = clientsManagerService;
            _productsManagerService = productsManagerService;
            _dataBaseManagerService = dataBaseManagerService;
            _chooseClientService = chooseClientService;

            AddExtraFromOrderCommand = new MvxCommand<ExtraOrder>(AddExtraFromOrder);
            DateSelectCommand = new MvxCommand(DateSelect);
            SelectFilterCommand = new MvxCommand<NotificationsType>(SelectFilter);
        }

        private void SelectFilter(NotificationsType notificationsType)
        {
            if (notificationsType == NotificationsTypeSelected)
                return;

            NotificationsTypeSelected = notificationsType; 
            NotificationListItems = GetList();
        }

        private void DateSelect()
        {
            _dialogService.ShowDatePickerDialog(selectDateAction, false);
        }

        private void selectDateAction(DateTime date)
        {
            DateSelected = date;
        }

        private void AddExtraFromOrder(ExtraOrder order)
        {
            _dialogService.ShowConfirmDialog("Confirmar a adicao do extra desta encomenda", "Sim", AddOrderExtra, "Não", order);
        }

        public void AddOrderExtra(object obj)
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
                var dayAmmount = _ordersManagerService.GetValue(extraOrder.Id, _clientsManagerService.GetTodayDailyOrder(client, extraOrder.OrderDay.DayOfWeek));
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

            _chooseClientService.SelectClient(_clientsManagerService.GetClientById(extraOrder.ClientId));
            RaisePropertyChanged(nameof(AddOrderExtra));

            IsBusy = false;
        }

        private DateTime _dateSelected = DateTime.Today;
        public DateTime DateSelected
        {
            get => _dateSelected;
            set
            {
                _dateSelected = value;
                NotificationListItems = GetList();
                RaisePropertyChanged(nameof(DateSelected));
            }
        }

        private List<NotificationItem<INotificationItem>> _notificationListItems;
        public List<NotificationItem<INotificationItem>> NotificationListItems
        {
            get => _notificationListItems;

            set
            {
                _notificationListItems = value;
                RaisePropertyChanged(nameof(NotificationListItems));
            }
        }


        public override void Appearing()
        {
            NotificationsTypesList = GetNotificationsTypesList();
            NotificationsTypeSelected = NotificationsTypesList[0];

            NotificationListItems = GetList();
        }

        private List<NotificationsType> GetNotificationsTypesList()
        {
            var list = new List<NotificationsType>();

            list.Add(new NotificationsType() { Name = "Todas", Type = NotificationsTypeSpinnerEnum.All });
            list.Add(new NotificationsType() { Name = "Encomendas", Type = NotificationsTypeSpinnerEnum.Orders });

            foreach (NotificationTypeEnum foo in Enum.GetValues(typeof(NotificationTypeEnum)))
            {
                if (foo == NotificationTypeEnum.None)
                    continue;

                list.Add(new NotificationsType() { Name = foo.ToString(), Type = NotificationsTypeSpinnerEnum.Notifications, TypeNotification = foo });
            }

            return list;
        }

        private List<NotificationItem<INotificationItem>> GetList()
        {
            var list = new List<NotificationItem<INotificationItem>>();

            if(NotificationsTypeSelected.Type == NotificationsTypeSpinnerEnum.All || NotificationsTypeSelected.Type == NotificationsTypeSpinnerEnum.Orders)
            {
                foreach (var item in _ordersManagerService.GetOrders(DateSelected))
                {
                    var order = new InternalClassOrder()
                    {
                        Client = item.Client,
                        ExtraOrder = item.ExtraOrder
                    };

                    list.Add(new NotificationItem<INotificationItem>()
                    {
                        Data = order
                    });
                }
            }
            if (NotificationsTypeSelected.Type == NotificationsTypeSpinnerEnum.All || NotificationsTypeSelected.Type == NotificationsTypeSpinnerEnum.Notifications)
            {
                foreach (var item in _notificationsManagerService.AllNotifications.Where(item => item.AlertDay.Date == DateSelected.Date))
                {
                    var not = new InternalNotification()
                    {
                        Notification = item
                    };

                    if(NotificationsTypeSelected.Type == NotificationsTypeSpinnerEnum.All || NotificationsTypeSelected.TypeNotification == item.NotificationType)
                    {
                        list.Add(new NotificationItem<INotificationItem>()
                        {
                            Data = not
                        });
                    }
                }
            }

            return list;
        }
        public Client GetClient(int clientId)
        {
            return _clientsManagerService.ClientsList.Find(item => item.Id == clientId);
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
    }

    public class NotificationsType
    {
        public string Name { get; set; }
        public NotificationsTypeSpinnerEnum Type { get; set; }
        public NotificationTypeEnum TypeNotification { get; set; }
    }

    public enum NotificationsTypeSpinnerEnum
    {
        All,
        Orders,
        Notifications
    }

    public class InternalClassOrder : INotificationItem
    {
        public Client Client { get; set; }

        public ExtraOrder ExtraOrder { get; set; }

    }

    public class InternalNotification : INotificationItem
    {
        public Core.Models.Notifications.Notification Notification { get; set; }
    }

    public interface INotificationItem { }

    public class NotificationItem<T>
    {
        public T Data { get; set; }
    }
}