using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;
using tabApp.Core.Services.Interfaces.WebServices.Sells;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;

namespace tabApp.Core.Services.Implementations.WebServices.Sells
{
    public class UpdateFatRequest : BaseRequest<CreateSellDocumentInput, CreateSellDocumentOutput>, IUpdateFatRequest
    {
        protected override string EndPoint => "/sales/:id/update";
        protected override bool HasId => true;
    }
}
