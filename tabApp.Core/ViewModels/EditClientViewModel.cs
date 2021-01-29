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

        public EditClientViewModel(IChooseClientService chooseClientService)
        {
            _chooseClientService = chooseClientService;
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
        public override void Appearing()
        {
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
