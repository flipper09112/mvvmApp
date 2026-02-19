using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Helpers;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.DB;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.ViewModels.ClientPage
{
    public class AddStoreRegistViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IChooseClientService _chooseClientService;
        private readonly IProductsManagerService _productsManagerService;
        private readonly IAddProductToOrderService _addProductToOrderService;
        private readonly IMvxNavigationService _navigationService;
        private readonly IClientsManagerService _clientsManagerService;
        private readonly IDataBaseManagerService _dataBaseManagerService;

        private bool _firstTime = true;

        public MvxCommand ShowCalendarPickerCommand;
        public MvxCommand SaveRegistCommand; 
        public MvxCommand AddProductCommand;
        public MvxCommand SelectDayCommand;

        public EventHandler GoBack2Times;

        public AddStoreRegistViewModel(IChooseClientService chooseClientService, IDialogService dialogService
                                        , IProductsManagerService productsManagerService, IAddProductToOrderService addProductToOrderService,
                                        IMvxNavigationService navigationService, IClientsManagerService clientsManagerService,
                                        IDataBaseManagerService dataBaseManagerService)
        {
            _dialogService = dialogService;
            _chooseClientService = chooseClientService;
            _productsManagerService = productsManagerService;
            _addProductToOrderService = addProductToOrderService;
            _navigationService = navigationService;
            _clientsManagerService = clientsManagerService;
            _dataBaseManagerService = dataBaseManagerService;

            ShowCalendarPickerCommand = new MvxCommand(ShowCalendarPicker);
            AddProductCommand = new MvxCommand(AddProduct);
            SelectDayCommand = new MvxCommand(SelectDay);
            SaveRegistCommand = new MvxCommand(SaveRegist, CanSaveRegist);
        }

        private void SelectDay()
        {
            _dialogService.ShowDatePickerDialog(ChooseDateAction, false);
        }

        private void ChooseDateAction(DateTime date)
        {
            _itemsList.Clear();
            foreach (var item in ClientHelper.GetDailyOrder(date.DayOfWeek, _chooseClientService.ClientSelected).AllItems)
            {
                Product prod = _productsManagerService.GetProductById(item.ProductId);
                _itemsList.Add(new ProductAmmount()
                {
                    Product = prod,
                    Ammount = item.Ammount
                });
            }

            DateSelectedDailyOrder = date;
        }

        private async void AddProduct()
        {
            await _navigationService.Navigate<ChooseProductViewModel>();
        }

        private async void SaveRegist()
        {
            IsBusy = true;

            List<DailyOrderDetails> items = new List<DailyOrderDetails>();
            ItemsList.ForEach(product => {
                if (product.Ammount > 0)
                    items.Add(new DailyOrderDetails() { ProductId = product.Product.Id, Ammount = product.Ammount });
            });

            var order = new ExtraOrder() {
                ClientId = _chooseClientService.ClientSelected.Id,
                OrderRegistDay = DateTime.Today,
                OrderDay = DateSelected,
                AllItems = items,
                IsTotal = true,
                StoreOrder = true
            }; 

            _clientsManagerService.AddNewOrder(_chooseClientService.ClientSelected, order);
            _dataBaseManagerService.SaveClient(_chooseClientService.ClientSelected, order);

            IsBusy = false;

            GoBack2Times?.Invoke(null, null);
            //await _navigationService.Close(this);
        }

        private bool CanSaveRegist()
        {
            if (_clientsManagerService.ClientHasExtraOrderThisDay(_chooseClientService.ClientSelected, DateSelected))
                return false;

            if(_itemsList.Count > 0)
            {
                foreach(var item in _itemsList)
                {
                    if (item.Ammount > 0)
                        return true;
                }
            }
            return false;
        }

        private void ShowCalendarPicker()
        {
            _dialogService.ShowDatePickerDialog(SetDate, false);
        }

        private void SetDate(DateTime obj)
        {
            DateSelected = obj;
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
                RaisePropertyChanged(nameof(DateSelected));
            }
        }

        public DateTime _dateSelectedDailyOrder = DateTime.Today;
        public DateTime DateSelectedDailyOrder
        {
            get
            {
                return _dateSelectedDailyOrder;
            }
            set
            {
                _dateSelectedDailyOrder = value;
                RaisePropertyChanged(nameof(DateSelectedDailyOrder));
            }
        }

        public List<ProductAmmount> _itemsList = new List<ProductAmmount>();
        public List<ProductAmmount> ItemsList
        {
            get
            {
                return _itemsList;
            }
            set
            {
                _itemsList = value;
            }
        }

        public override void Appearing()
        {
            GetDayItems();
            if(_addProductToOrderService.ProductsSelected.Count > 0)
            {
                foreach(var product in _addProductToOrderService.ProductsSelected)
                {
                    if (_itemsList.Find(item => item.Product.Id == product.Id) != null)
                        continue;

                    _itemsList.Add(new ProductAmmount()
                    {
                        Product = product,
                        Ammount = 0
                    });
                }
                _addProductToOrderService.Clear();
            }
            _firstTime = false;
        }

        private void GetDayItems()
        {
            if (_firstTime)
            {
                foreach(var item in ClientHelper.GetDailyOrder(DateTime.Today.DayOfWeek, _chooseClientService.ClientSelected).AllItems)
                {
                    Product prod = _productsManagerService.GetProductById(item.ProductId);
                    _itemsList.Add(new ProductAmmount() { 
                        Product = prod,
                        Ammount = item.Ammount
                    });
                }
            }
        }

        public override void DisAppearing()
        {
        }
    }
}
