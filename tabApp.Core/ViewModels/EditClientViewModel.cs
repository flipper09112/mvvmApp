using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Interfaces.DB;
using tabApp.Core.Services.Interfaces.Dialogs;

namespace tabApp.Core.ViewModels
{
    public class EditClientViewModel : BaseViewModel
    {
        private readonly IChooseClientService _chooseClientService;
        private readonly IDialogService _dialogService;
        private readonly IDBManagerService _dBManagerService;
        protected readonly IMvxNavigationService _navService;

        public MvxCommand SaveChangesCommand;

        public EditClientViewModel(IChooseClientService chooseClientService, IDialogService dialogService, IDBManagerService dBManagerService, IMvxNavigationService navService)
        {
            _chooseClientService = chooseClientService;
            _dialogService = dialogService;
            _dBManagerService = dBManagerService;
            _navService = navService;

            SaveChangesCommand = new MvxCommand(SaveChanges, CanSaveChanges);
        }

        private bool CanSaveChanges()
        {
            if (_newLocation != null && !_newLocation.Equals(Client.Address.Coordenadas))
                return true;

            foreach (var item in _profileItems)
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue))
                    return true;
            }

            foreach (var item in _adminItems)
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue))
                    return true;
            }

            return false;
        }

        private void SaveChanges()
        {
            bool isValid = false;
            string toRegist = "";
            if (_newLocation != null && !_newLocation.Equals(Client.Address.Coordenadas))
            {
                toRegist += "Localização alterada de " + Client.Address.Coordenadas + " para " + _newLocation + "\n";
                Client.UpdateLocation(_newLocation);
            }

            foreach (var item in _profileItems)
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue))
                {
                    toRegist += "Alteração de '" + item.Name + "'" + " antes: " + item.Value + " depois: " + item.NewValue + "\n";
                    isValid = SaveRegist(item);
                }
            }
            if (!isValid) return;
            foreach (var item in _adminItems)
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue))
                {
                    toRegist += "Alteração de '" + item.Name + "'" + " antes: " + item.Value + " depois: " + item.NewValue + "\n";
                    SaveRegist(item);
                }
            }
            if (!isValid) return;

            _dBManagerService.SaveClient(Client, toRegist);
            _navService.Close(this);
        }

        private bool SaveRegist(ClientProfileField item)
        {
            try
            {
                switch (item.Type)
                {
                    case nameof(Client.Name):
                        Client.UpdateName(item.NewValue);
                        break;
                    case nameof(Client.SubName):
                        Client.UpdateSubName(item.NewValue);
                        break;
                    case nameof(Client.Address.AddressDesc):
                        Client.UpdateAddressDesc(item.NewValue);
                        break;
                    case nameof(Client.Address.NumberDoor):
                        Client.UpdateNumberDoor(int.Parse(item.NewValue));
                        break;
                    case nameof(Client.PaymentDate):
                        Client.UpdatePaymentDate(DateTime.Parse(item.NewValue));
                        break;
                    case nameof(Client.ExtraValueToPay):
                        Client.UpdateExtraValueToPay(double.Parse(item.NewValue));
                        break;
                    default:
                        _dialogService.ShowConfirmDialog("Alguma coisa correu mal", "Este parametro nao foi salvo (" + nameof(Client.ExtraValueToPay) + ")", null);
                        break;
                }
                return true;
            } catch (Exception e)
            {
                _dialogService.ShowConfirmDialog("Alguma coisa correu mal", "Algum parametro invalido", null);
                return false;
            }
        }

        private string _newLocation;
        public string NewLocation
        {
            get
            {
                if (_newLocation == null)
                    return Client.Address.Coordenadas;
                return _newLocation;
            }
            set
            {
                _newLocation = value;
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }
        public Client Client => _chooseClientService.ClientSelected;
        public List<EditFieldsEnum> TabsOptions
        {
            get
            {
                List<EditFieldsEnum> items = new List<EditFieldsEnum>();
                items.Add(EditFieldsEnum.Profile);
                items.Add(EditFieldsEnum.Localização);
                items.Add(EditFieldsEnum.Admin);
                return items;
            }
        }
        private List<ClientProfileField> _profileItems;
        public List<ClientProfileField> ProfileItems
        {
            get
            {
                return _profileItems;
            }
            set
            {
                _profileItems = value;
                RaisePropertyChanged(nameof(ProfileItems));
            }
        }
        private List<ClientProfileField> _adminItems;
        public List<ClientProfileField> AdminItems
        {
            get
            {
                return _adminItems;
            }
            set
            {
                _adminItems = value;
                RaisePropertyChanged(nameof(AdminItems));
            }
        }
        public override void Appearing()
        {
            ProfileItems = GetProfileItems();
            AdminItems = GetAdminItems();
        }

        private List<ClientProfileField> GetProfileItems()
        {
            List<ClientProfileField> items = new List<ClientProfileField>();
            items.Add(new ClientProfileField()
            {
                IsInt = false,
                IconName = "profileicon",
                Name = "Nome",
                Value = Client.Name,
                Type = nameof(Client.Name),
                RefreshSaveCommand = SaveChangesCommand
            });
            items.Add(new ClientProfileField()
            {
                IsInt = false,
                IconName = "ic_nickname",
                Name = "Alcunha",
                Value = Client.SubName,
                Type = nameof(Client.SubName),
                RefreshSaveCommand = SaveChangesCommand
            });
            items.Add(new ClientProfileField()
            {
                IsInt = false,
                IconName = "ic_pin_location",
                Name = "Morada",
                Value = Client.Address.AddressDesc,
                Type = nameof(Client.Address.AddressDesc),
                RefreshSaveCommand = SaveChangesCommand
            });
            items.Add(new ClientProfileField()
            {
                IsInt = true,
                IconName = "ic_door",
                Name = "Número da porta",
                Value = Client.Address.NumberDoor.ToString(),
                Type = nameof(Client.Address.NumberDoor),
                RefreshSaveCommand = SaveChangesCommand
            });
            items.Add(new ClientProfileField()
            {
                IsInt = true,
                IconName = "ic_phone",
                Name = "Telemóvel",
                Value = "Indisponivel",
                Type = nameof(Client),
                RefreshSaveCommand = SaveChangesCommand
            });
            return items;
        }

        private List<ClientProfileField> GetAdminItems()
        {
            List<ClientProfileField> items = new List<ClientProfileField>();
            items.Add(new ClientProfileField()
            {
                IsDate = true,
                IconName = "ic_calendar",
                Name = "Data do último pagamento",
                Value = Client.PaymentDate.ToString("dd/MM/yyyy"),
                Type = nameof(Client.PaymentDate),
                RefreshSaveCommand = SaveChangesCommand
            });
            items.Add(new ClientProfileField()
            {
                IsDouble = true,
                IconName = "ic_money",
                Name = "Valor de extras a pagar",
                Value = Client.ExtraValueToPay.ToString("N2"),
                Type = nameof(Client.ExtraValueToPay),
                RefreshSaveCommand = SaveChangesCommand
            });
            
            return items;
        }

        public override void DisAppearing()
        {
        }
    }

    public enum EditFieldsEnum
    {
        Profile,
        Localização,
        Admin
    }
}

