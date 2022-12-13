using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Models.Faturation;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.WebServices.Clients.DTOs;
using tabApp.Core.Services.Interfaces.WebServices.Clients;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Helpers;
using System.Globalization;
using System.Linq;

namespace tabApp.Core.Services.Implementations.Faturation.Helpers
{
    public class Clients
    {
        private IDialogService _dialogService;
        private IGetClientRequest _getClientRequest;
        private ICreateFatClientRequest _createFatClientRequest;
        private IClientsManagerService _clientsManagerService;

        public Clients(IDialogService dialogService, 
                       IGetClientRequest getClientRequest,
                       ICreateFatClientRequest createFatClientRequest,
                       IClientsManagerService clientsManagerService)
        {
            _dialogService = dialogService;
            _getClientRequest = getClientRequest;
            _createFatClientRequest = createFatClientRequest;
            _clientsManagerService = clientsManagerService;
        }

        public async Task<List<FatClient>> GetClient(string id)
        {
            List<FatClient> clientsList = new List<FatClient>();

            var response = await _getClientRequest.SendAsync(new GetClientInput()
            {
                Value = id,
                SearchIn = ClientFieldEnum.Code
            });

            if (!response.Success)
            {
                _dialogService.ShowErrorDialog(string.Empty, response.Error);
                return clientsList;
            }

            response.data.ForEach(client => clientsList.Add(new FatClient()
            {
                Id = client.id,
                Name = client.name,
                NIF = client.vat_number,
                Address = client.address,
                Country = client.country,
                PostalCode = client.postal_code,
                City = client.city
            }));

            return clientsList;
        }

        internal async Task<FatClient> ValidateClient(FatClient client)
        {
            var c = await GetClient(client.Id.ToString());

            if (c.Count == 0) 
            {
                /*if(string.IsNullOrEmpty(client.Address) || client.Address.Equals("Sem morada definida"))
                {
                    var appClient = _clientsManagerService.GetClientById(client.Id);

                    if (appClient.Address.HasCoord)
                    {
                        var location = await GeoLocationHelper.GetAddress(double.Parse(appClient.Address.Lat.Replace(",", ".")), 
                                                                          double.Parse(appClient.Address.Lgt.Replace(",", ".")));

                        //client.City = location.address.city;
                    }
                }*/

                var response = await _createFatClientRequest.SendAsync(new CreateFatClientInput()
                {
                    Code = client.Id.ToString(),
                    Name = client.Name,
                    VatNumber = client.NIF,
                    Country = "Portugal",
                    Address = client.Address,
                    //City = "",
                    //PostalCode = "",
                    Type = FatClientTypeEnum.Particular,
                    VatType = Interfaces.WebServices.Sells.DTOs.VatTypeEnum.IVAincluído
                });

                if (!response.Success)
                {
                    _dialogService.ShowErrorDialog(string.Empty, response.Error);
                    return null;
                }
            }
            else
            {
                return c.First();
            }

            return null;
        }
    }
}
