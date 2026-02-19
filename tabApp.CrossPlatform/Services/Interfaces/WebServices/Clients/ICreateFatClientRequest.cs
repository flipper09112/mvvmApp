using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;
using tabApp.Core.Services.Interfaces.WebServices.Clients.DTOs;

namespace tabApp.Core.Services.Interfaces.WebServices.Clients
{
    public interface ICreateFatClientRequest : IBaseRequest<CreateFatClientInput, CreateFatClientOutput>
    {
    }
}
