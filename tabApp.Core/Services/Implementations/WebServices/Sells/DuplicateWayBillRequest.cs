using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;
using tabApp.Core.Services.Interfaces.WebServices.Sells;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;

namespace tabApp.Core.Services.Implementations.WebServices.Sells
{
    internal class DuplicateWayBillRequest : BaseRequest<DuplicateWayBillInput, DuplicateWayBillOutput>, IDuplicateWayBillRequest
    {
        protected override string EndPoint => "/sales/:id/duplicate";
        protected override bool HasId => true;
    }
}
