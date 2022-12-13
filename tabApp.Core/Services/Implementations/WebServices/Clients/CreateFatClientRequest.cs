using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;
using tabApp.Core.Services.Interfaces.WebServices.Clients;
using tabApp.Core.Services.Interfaces.WebServices.Clients.DTOs;

namespace tabApp.Core.Services.Implementations.WebServices.Clients
{
    public class CreateFatClientRequest : BaseRequest<CreateFatClientInput, CreateFatClientOutput>, ICreateFatClientRequest
    {
        protected override string EndPoint => "/customers/create";
    }
}
