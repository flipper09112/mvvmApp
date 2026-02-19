using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.ViewModels;

namespace tabApp.Core
{
    public class InitDailyViewModel : BaseViewModel
    {
        private IChooseClientService _chooseClientService;
        private IDataBaseManagerService _dataBaseManagerService;
        private IMvxNavigationService _navigationService;

        public EventHandler GoBack;
        public MvxCommand ActivateCommand;
        public InitDailyViewModel(IChooseClientService chooseClientService,
                                  IDataBaseManagerService dataBaseManagerService,
                                  IMvxNavigationService navigationService)
        {
            _chooseClientService = chooseClientService;
            _dataBaseManagerService = dataBaseManagerService;
            _navigationService = navigationService;

            ActivateCommand = new MvxCommand(Activate);
        }

        private void Activate()
        {
            Client.UpdateActive(true);
            _dataBaseManagerService.SaveClient(Client, new Regist() { 
                DetailRegistDay = DateTime.Now,
                Info = "Cliente voltou a ativar o serviço de entrega ao domicilio",
                DetailType = DetailTypeEnum.Inativate
            });
            GoBack?.Invoke(null, null);
        }

        public Client Client => _chooseClientService.ClientSelected;

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
}