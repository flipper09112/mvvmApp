using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;

namespace tabApp.Core.Services.Interfaces.WebServices.Sells
{
    public interface IDuplicateWayBillRequest : IBaseRequest<DuplicateWayBillInput, DuplicateWayBillOutput>
    {
    }
}
