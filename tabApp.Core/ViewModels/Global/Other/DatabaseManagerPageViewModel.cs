using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Interfaces;

namespace tabApp.Core.ViewModels.Global.Other
{
    public class DatabaseManagerPageViewModel : BaseViewModel
    {
        private IDataBaseManagerService _dataBaseManagerService;
        private IFileService _fileService;

        public MvxCommand SendDatabaseCommand;
        public MvxCommand RestoreDatabaseCommand;

        public EventHandler GoBack2Times;
        public EventHandler UpdateDownloadPercentage;

        public DatabaseManagerPageViewModel(IDataBaseManagerService dataBaseManagerService,
                                            IFileService fileService)
        {
            _dataBaseManagerService = dataBaseManagerService;
            _fileService = fileService;

            RestoreDatabaseCommand = new MvxCommand(RestoreDatabase);
            SendDatabaseCommand = new MvxCommand(SendDatabase);
        }

        private async void RestoreDatabase()
        {
            IsBusy = true;
            _fileService.DeleteFile(DataBaseManagerService.DataBaseName);
            await _dataBaseManagerService.LoadDataBase(UpdateDownloadPercentage);
            _dataBaseManagerService.DBRestored = true;
            GoBack2Times?.Invoke(null, null);
            IsBusy = false;
        }

        private void SendDatabase()
        {
            _dataBaseManagerService.SaveAllDocs();
            GoBack2Times?.Invoke(null, null);
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
}
