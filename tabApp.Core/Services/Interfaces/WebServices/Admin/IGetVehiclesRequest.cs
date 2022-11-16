using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Admin.DTOs;
using tabApp.Core.Services.Interfaces.WebServices.Bases;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;

namespace tabApp.Core.Services.Interfaces.WebServices.Admin
{
    public interface IGetVehiclesRequest : IBaseRequest<GetVehiclesInput, GetVehiclesOutput>
    {
    }
}
