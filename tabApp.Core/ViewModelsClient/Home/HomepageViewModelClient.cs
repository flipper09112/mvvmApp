using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModelsClient.Catalog;
using tabApp.Core.ViewModelsClient.Info;

namespace tabApp.Core.ViewModelsClient
{
    public class HomepageViewModelClient : BaseViewModel
    {
        public List<HomepageItem> HomepageItems { get; set; }

        private IMvxNavigationService _navigationService;

        public MvxCommand ShowCatalogCommand { get; }
        public MvxCommand ShowInfoCommand { get; }

        public HomepageViewModelClient(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        
            ShowCatalogCommand = new MvxCommand(ShowCatalog);
            ShowInfoCommand = new MvxCommand(ShowInfo);
        }

        private async void ShowInfo()
        {
            await _navigationService.Navigate<InfoViewModel>();
        }

        private async void ShowCatalog()
        {
            await _navigationService.Navigate<CatalogViewModel>();
        }

        public override void Appearing()
        {
            HomepageItems = GetItems();
        }

        public override void DisAppearing()
        {
           
        }

        private List<HomepageItem> GetItems()
        {
            var list = new List<HomepageItem>();
            list.Add(new HomepageItem()
            {
                Name = "Catálogo",
                IconName = "catalog256_24878",
                Command = ShowCatalogCommand
            });
            list.Add(new HomepageItem()
            {
                Name = "Informação",
                IconName = "ic_info",
                Command = ShowInfoCommand
            });
            return list;
        }
    }

    public class HomepageItem
    {
        public string Name { get; set; }
        public string IconName { get; set; }
        public MvxCommand Command { get; internal set; }
    }
}
