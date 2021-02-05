using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.ViewModels.ClientPage;

namespace tabApp.Core.ViewModels
{
    public class OtherOptionsViewModel : BaseViewModel
    {
        private readonly IChooseClientService _chooseClientService;
        private readonly IMvxNavigationService _navigationService;

        public MvxCommand InsertNewRegistCommand;

        public OtherOptionsViewModel(IChooseClientService chooseClientService, IMvxNavigationService navigationService)
        {
            _chooseClientService = chooseClientService;
            _navigationService = navigationService;

            InsertNewRegistCommand = new MvxCommand(InsertNewRegist);

        }

        private async void InsertNewRegist()
        {
            await _navigationService.Navigate<AddStoreRegistViewModel>();
        }

        public List<Option> Options
        {
            get
            {
                List<Option> options = new List<Option>();

                options.Add(new Option(null, "Imprimir Conta", "ic_printer"));
                options.Add(new Option(null, "Alterar Quantidade", "ic_change"));
                if(_chooseClientService.ClientSelected.PaymentType == PaymentTypeEnum.Loja)
                    options.Add(new Option(InsertNewRegistCommand, "Inserir despesa do dia", "ic_insert"));

                return options;
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
