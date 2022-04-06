using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Implementations.Clients
{
    public interface IChooseClientService
    {
        Client ClientSelected { get; }
        void SelectClient(Client clientSelected);

        DateTime PayTo { get; set; }
    }
}
