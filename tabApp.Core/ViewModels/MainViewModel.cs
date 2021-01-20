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
        private readonly ISaveFileService _saveFileService;

        public MainViewModel(ISaveFileService saveFileService, IGetFileService getFileService, IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
            _saveFileService = saveFileService;
            _getFileService = getFileService;

            ShowHomePage = new MvxAsyncCommand(async () => await _navigationService.Navigate<HomeViewModel>());
        }

        public MvxAsyncCommand ShowHomePage { get; private set; }

        public override async void AppearingAsync()
        {
            if(!_saveFileService.HasFile("MeuArquivoXLS.xls"))
            {
                _saveFileService.SaveFile("MeuArquivoXLS.xls", await _getFileService.GetUrlDownload("MeuArquivoXLS.xls"));
            }
        }
    }
}
