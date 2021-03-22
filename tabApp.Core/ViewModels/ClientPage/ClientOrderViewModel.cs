using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.DB;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.ViewModels
{
    public class ClientOrderViewModel : BaseViewModel
    {
        private readonly IChooseClientService _chooseClientService;
        private readonly IClientsManagerService _clientsManagerService;
        private readonly IDataBaseManagerService _dataBaseManagerService;
        private readonly IDialogService _dialogService;
        private readonly IMvxNavigationService _navigationService;
        private readonly IAddProductToOrderService _addProductToOrderService;

        private DateTime _dateTime;
        private bool? _isTotal;
        public MvxCommand SelectDateCommand { get; set; }
        public MvxCommand AddProductCommand { get; set; }
        public MvxCommand SaveNewOrderCommand { get; set; }

        public ClientOrderViewModel(IChooseClientService chooseClientService,
                                    IDialogService dialogService,
                                    IMvxNavigationService navigationService,
                                    IAddProductToOrderService addProductToOrderService,
                                    IClientsManagerService clientsManagerService,
                                    IDataBaseManagerService dataBaseManagerService)
        {
            _chooseClientService = chooseClientService;
            _dialogService = dialogService;
            _navigationService = navigationService;
            _addProductToOrderService = addProductToOrderService;
            _clientsManagerService = clientsManagerService;
            _dataBaseManagerService = dataBaseManagerService;

            SelectDateCommand = new MvxCommand(SelectDate);
            SaveNewOrderCommand = new MvxCommand(SaveNewOrder, CanSaveNewOrder);
            AddProductCommand = new MvxCommand(AddProduct);
        }

        private bool CanSaveNewOrder()
        {
            return DateSelected.Date > DateTime.Today && _isTotal != null && HasProducts();
        }

        private bool HasProducts()
        {
            foreach(var item in OrderProducts)
            {
                if (item.Ammount > 0)
                    return true;
            }
            return false;
        }

        private async void SaveNewOrder()
        {
            IsBusy = true;

            if (!HasProducts())
                return;

            List<DailyOrderDetails> items = new List<DailyOrderDetails>();
            OrderProducts.ForEach(product => {
                if (product.Ammount > 0)
                    items.Add(new DailyOrderDetails() { ProductId = product.Product.Id, Ammount = product.Ammount });
            });

            var order = new ExtraOrder()
            {
                ClientId = _chooseClientService.ClientSelected.Id,
                OrderRegistDay = DateTime.Today,
                OrderDay = DateSelected,
                AllItems = items,
                IsTotal = (bool)IsTotal,
                StoreOrder = false
            };

            _clientsManagerService.AddNewOrder(_chooseClientService.ClientSelected, order);
            _dataBaseManagerService.SaveClient(_chooseClientService.ClientSelected, order);

            IsBusy = false;

            await _navigationService.Close(this);
        }

        private void SelectDate()
        {
            _dialogService.ShowDatePickerDialog(SelectDateAction);
        }

        private void SelectDateAction(DateTime selectedDate)
        {
            DateSelected = selectedDate;
        }
        public Client Client => _chooseClientService.ClientSelected;

        public bool? IsTotal
        {
            get
            {
                return _isTotal;
            }
            set
            {
                _isTotal = value;
                SaveNewOrderCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(IsTotal));
            }
        }

        private List<ProductAmmount> _orderProducts = new List<ProductAmmount>();
        public List<ProductAmmount> OrderProducts { 
            get
            {
                _addProductToOrderService.ProductsSelected.ForEach(product => { 
                    if(_orderProducts.Find(item => item.Product.Id == product.Id) == null)
                    {
                        _orderProducts.Add(new ProductAmmount() { Product = product, Ammount = 0});
                    }
                });
                _addProductToOrderService.Clear();
                return _orderProducts;
            }
        } 

        public DateTime DateSelected
        {
            get
            {
                return _dateTime;
            }
            set
            {
                _dateTime = value;
                SaveNewOrderCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(DateSelected));
            }
        }

        private async void AddProduct()
        {
            await _navigationService.Navigate<ChooseProductViewModel>();
        }

        public override void Appearing()
        {
            if(DateSelected == DateTime.MinValue)
                DateSelected = DateTime.Today;
        }
        public override void DisAppearing()
        {
            _addProductToOrderService.Clear();
        }
    }
}
