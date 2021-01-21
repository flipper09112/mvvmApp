using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;

namespace tabApp.Core.ViewModels
{
    public class ClientPageViewModel : BaseViewModel
    {

        private readonly IChooseClientService _chooseClientService;

        public ClientPageViewModel(IChooseClientService chooseClientService)
        {
            _chooseClientService = chooseClientService;
        }

        public Client Client => _chooseClientService.ClientSelected;

        public override void AppearingAsync()
        {
        }
    }
}
