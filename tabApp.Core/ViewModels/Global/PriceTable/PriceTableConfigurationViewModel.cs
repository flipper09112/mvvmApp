using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.ViewModels.Bases;

namespace tabApp.Core.ViewModels.Global.PriceTable
{
    public class PriceTableConfigurationViewModel : BaseOptionsListViewModel
    {
        private IMvxNavigationService _navigationService;

        public MvxCommand AddProductCommand;

        public PriceTableConfigurationViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            AddProductCommand = new MvxCommand(AddProductPage);

            Options = new List<Models.Option>();
            Options.Add(new Models.Option(AddProductCommand, "Adicionar Produto", "ic_add"));
        }

        private async void AddProductPage()
        {
            await _navigationService.Navigate<AddProductViewModel>();
        }
    }
}
