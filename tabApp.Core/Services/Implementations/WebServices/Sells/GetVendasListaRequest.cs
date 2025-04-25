using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;
using tabApp.Core.Services.Interfaces.WebServices.Sells;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;

namespace tabApp.Core.Services.Implementations.WebServices.Sells
{
    public class GetVendasListaRequest : BaseRequest<GetVendasListaInput, GetVendasListaOutput>, IGetVendasListaRequest
    {
        protected override HttpMethod HttpMethod => HttpMethod.Post;

        protected override string EndPoint => "/sales/find";
    }
}
