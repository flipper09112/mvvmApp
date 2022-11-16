using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using tabApp.Core.Services.Interfaces.Faturation;

namespace tabApp.Core.ViewModels.Bases.Generic
{
    public class DocumentViewModel : BaseViewModel
    {
        private IFaturationService _faturationService;

        public string DocUrl { get; }

        public DocumentViewModel(IFaturationService faturationService)
        {
            _faturationService = faturationService;

            DocUrl = _faturationService.DocumentSelected.DocumentUrl;
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
}
