using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.ViewModels;

namespace tabApp.Core
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IClientsManagerService _clientsManagerService;
        private readonly IChooseClientService _chooseClientService;

        public MvxAsyncCommand<Client> ShowClientPage { get; private set; }

        public HomeViewModel(IMvxNavigationService navigationService, 
                            IClientsManagerService clientsManagerService,
                            IChooseClientService chooseClientService)
        {
            _clientsManagerService = clientsManagerService;
            _navigationService = navigationService;
            _chooseClientService = chooseClientService;

            ShowClientPage = new MvxAsyncCommand<Client>(ShowClientPageAction);
        }

        private async Task ShowClientPageAction(Client clientSelected)
        {
            _chooseClientService.SelectClient(clientSelected);
            await _navigationService.Navigate<ClientPageViewModel>();
        }

        public List<Client> ClientsList
        {
            get
            {
                return _clientsManagerService.ClientsList;
            }
        }

        public override void Appearing()
        {
        }
        public override void DisAppearing()
        {
        }
    }
}
