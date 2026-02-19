using Spire.Pdf.Fields;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;
using tabApp.Core.Services.Interfaces.WebServices.Sells;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;

namespace tabApp.Core.Services.Implementations.WebServices.Sells
{
    public class DownloadFatRequest : BaseRequest<DownloadFatInput, DownloadFatOutput>, IDownloadFatRequest
    {
        protected override string EndPoint => "/sales/:id/download";
        protected override bool HasId => true;
    }
}
