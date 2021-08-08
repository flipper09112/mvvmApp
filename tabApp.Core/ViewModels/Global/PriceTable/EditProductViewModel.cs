using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Implementations.Products;
using tabApp.Core.Services.Interfaces.Clients;

namespace tabApp.Core.ViewModels.Global.PriceTable
{
    public class EditProductViewModel : BaseViewModel
    {
        private IChooseProductService _chooseProductService;
        private IClientsManagerService _clientsManagerService;
        private IDataBaseManagerService _dataBaseManagerService;

        private List<(int ClientId, double newValue)> _newValuesList;

        public EventHandler GoBack;
        public MvxCommand SaveChangesCommand;
        public MvxCommand<(int ClientId, string newValue)> ChangeValueCommand;
        public Product ProductSelected => _chooseProductService.Product; 
        public EditProductViewModel(IChooseProductService chooseProductService,
                                    IClientsManagerService clientsManagerService,
                                    IDataBaseManagerService dataBaseManagerService)
        {
            _chooseProductService = chooseProductService;
            _clientsManagerService = clientsManagerService;
            _dataBaseManagerService = dataBaseManagerService;

            ChangeValueCommand = new MvxCommand<(int ClientId, string newValue)>(SaveNewValue);
            SaveChangesCommand = new MvxCommand(SaveChanges, CanSaveChanges);

            _newValuesList = new List<(int ClientId, double newValue)>();
        }

        public double GetValue(int clientId, double productSelected)
        {
            var newValue = _newValuesList.Find(item => item.ClientId == clientId);

            if (newValue.ClientId == 0)
                return productSelected;
            else
                return newValue.newValue;
        }

        private void SaveNewValue((int ClientId, string newValue) newValue)
        {
            var value = _newValuesList.Find(item => item.ClientId == newValue.ClientId);
            int pos = _newValuesList.IndexOf(value);

            if(value == default((int ClientId, double newValue)))
            {
                double newAmmount = -1;
                var parsed = double.TryParse(newValue.newValue, NumberStyles.Number, CultureInfo.InvariantCulture, out newAmmount);
                if (parsed)
                    _newValuesList.Add((newValue.ClientId, newAmmount));
            }
            else
            {
                double newAmmount = -1;
                var parsed = double.TryParse(newValue.newValue, NumberStyles.Number, CultureInfo.InvariantCulture, out newAmmount);
                if (parsed)
                    _newValuesList[pos] = (value.ClientId, newAmmount);
            }
            RaisePropertyChanged(nameof(SaveChangesCommand));
            SaveChangesCommand.RaiseCanExecuteChanged();
        }

        private void SaveChanges()
        {
            foreach((int ClientId, double newValue) newValue in _newValuesList)
            {
                if(newValue.ClientId == -1)
                {
                    ProductSelected.PVP = newValue.newValue;
                }
                else
                {
                    var clientPrice = ProductSelected.ReSaleValues.Find(item => item.ClientId == newValue.ClientId);
                    clientPrice.Value = newValue.newValue;
                }
            }
            _dataBaseManagerService.SaveProduct(ProductSelected);
            GoBack?.Invoke(null, null);
        }

        private bool CanSaveChanges()
        {
            return _newValuesList.Count != 0;
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }

        public string GetClientName(int clientId)
        {
            return _clientsManagerService.GetClientById(clientId).Name;
        }
    }
}
