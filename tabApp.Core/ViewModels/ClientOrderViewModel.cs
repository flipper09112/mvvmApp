using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Interfaces.Dialogs;

namespace tabApp.Core.ViewModels
{
    public class ClientOrderViewModel : BaseViewModel
    {
        private readonly IChooseClientService _chooseClientService;
        private readonly IDialogService _dialogService;
        private readonly IMvxNavigationService _navigationService;

        private DateTime _dateTime;
        private bool? _isTotal;
        private List<Product> _orderProducts = new List<Product>();
        public MvxCommand SelectDateCommand { get; set; }
        public MvxCommand AddProductCommand { get; set; }
        public MvxCommand SaveNewOrderCommand { get; set; }

        public ClientOrderViewModel(IChooseClientService chooseClientService,
                                    IDialogService dialogService,
                                    IMvxNavigationService navigationService)
        {
            _chooseClientService = chooseClientService;
            _dialogService = dialogService;
            _navigationService = navigationService;

            SelectDateCommand = new MvxCommand(SelectDate);
            SaveNewOrderCommand = new MvxCommand(SaveNewOrder, CanSaveNewOrder);
            AddProductCommand = new MvxCommand(AddProduct);
        }

        private bool CanSaveNewOrder()
        {
            return DateSelected.Date > DateTime.Today && _isTotal != null && OrderProducts.Count > 0;
        }

        private void SaveNewOrder()
        {
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
        public List<Product> OrderProducts
        {
            get
            {
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
            DateSelected = DateTime.Today;
        }
    }
}
