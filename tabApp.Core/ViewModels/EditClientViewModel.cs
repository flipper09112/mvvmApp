using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;

namespace tabApp.Core.ViewModels
{
    public class EditClientViewModel : BaseViewModel
    {
        private readonly IChooseClientService _chooseClientService;

        public MvxCommand SaveChangesCommand;
        public EditClientViewModel(IChooseClientService chooseClientService)
        {
            _chooseClientService = chooseClientService;

            SaveChangesCommand = new MvxCommand(SaveChanges, CanSaveChanges);
        }

        private bool CanSaveChanges()
        {            
            foreach(var item in _profileItems)
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue))
                    return true;
            }
            return false;
        }

        private void SaveChanges()
        {

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
        public override void Appearing()
        {
            ProfileItems = GetProfileItems();
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
                RefreshSaveCommand = SaveChangesCommand
            });
            items.Add(new ClientProfileField()
            {
                IsInt = false,
                IconName = "ic_nickname",
                Name = "Alcunha",
                Value = Client.SubName,
                RefreshSaveCommand = SaveChangesCommand
            });
            items.Add(new ClientProfileField()
            {
                IsInt = false,
                IconName = "ic_pin_location",
                Name = "Morada",
                Value = Client.Address.AddressDesc,
                RefreshSaveCommand = SaveChangesCommand
            });
            items.Add(new ClientProfileField()
            {
                IsInt = true,
                IconName = "ic_door",
                Name = "Número da porta",
                Value = Client.Address.NumberDoor.ToString(),
                RefreshSaveCommand = SaveChangesCommand
            });
            items.Add(new ClientProfileField()
            {
                IsInt = true,
                IconName = "ic_phone",
                Name = "Telemóvel",
                Value = "Indisponivel",
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

