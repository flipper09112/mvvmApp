using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;
using tabApp.Core.Services.Interfaces.WebServices.Sells;
using tabApp.Core.Services.Interfaces.WebServices.Admin;
using tabApp.Core.Services.Interfaces.WebServices.Admin.DTOs;
using System.Net.Http;

namespace tabApp.Core.Services.Implementations.WebServices.Admin
{
    internal class GetVehiclesRequest : BaseRequest<GetVehiclesInput, GetVehiclesOutput>, IGetVehiclesRequest
    {
        protected override HttpMethod HttpMethod => HttpMethod.Post;

        protected override string EndPoint => "/administration/vehicles/list";
    }
}
