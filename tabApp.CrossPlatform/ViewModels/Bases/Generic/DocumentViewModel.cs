using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using tabApp.Core.Services.Interfaces;
using tabApp.Core.Services.Interfaces.Faturation;

namespace tabApp.Core.ViewModels.Bases.Generic
{
    public class DocumentViewModel : BaseViewModel
    {
        private IFaturationService _faturationService;
        private IFileService _fileService;

        public string DocUrl { get; }
        public MvxCommand OpenExternalAppCommand { get; set; }
        public MvxCommand PosPrinterCommand { get; set; }

        public DocumentViewModel(IFaturationService faturationService,
                                 IFileService fileService)
        {
            _faturationService = faturationService;
            _fileService = fileService;

            OpenExternalAppCommand = new MvxCommand(OpenExternalApp);
            PosPrinterCommand = new MvxCommand(PosPrinter);

            DocUrl = _faturationService.DocumentSelected.DocumentUrl;
        }

        private async void PosPrinter()
        {
            /*
            var url = await _faturationService.TrasnportationsDocs.GetDocumentPos(_faturationService.DocumentSelected);

            if (url == null) return;

            _fileService.ShowPdfExternalApp(new Models.Faturation.TrasnportationDoc()
            {
                DocumentUrl = url,
                ID = _faturationService.DocumentSelected.ID,
                Name = "POS_" + _faturationService.DocumentSelected.Name,
            });
            */
        }

        private void OpenExternalApp()
        {
            _fileService.ClearAllFilesFromPath();
            _fileService.ShowPdfExternalApp(_faturationService.DocumentSelected);
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
}
