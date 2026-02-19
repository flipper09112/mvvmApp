using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;
using tabApp.Core.Services.Interfaces.WebServices.Sells;
using tabApp.Core.Services.Interfaces.WebServices.Clients.DTOs;
using tabApp.Core.Services.Interfaces.WebServices.Clients;
using System.Threading.Tasks;
using System.Net.Http;

namespace tabApp.Core.Services.Implementations.WebServices.Clients
{
    public class GetClientRequest : BaseRequest<GetClientInput, GetClientOutput>, IGetClientRequest
    {
        protected override HttpMethod HttpMethod => HttpMethod.Post;

        protected override string EndPoint => "/customers/find";
    }
}
