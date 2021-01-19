using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces;

namespace tabApp.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IGetFileService _getFileService;

        public MainViewModel(IGetFileService getFileService, IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
            _getFileService = getFileService;

            ShowHomePage = new MvxAsyncCommand(async () => await _navigationService.Navigate<HomeViewModel>());
        }

        public MvxAsyncCommand ShowHomePage { get; private set; }

        public override void Appearing()
        {
            _getFileService.GetUrlDownload("MeuArquivoXLS.xls");
        }
    }
}
