using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Models.Faturation;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.WebServices.Admin.DTOs;
using tabApp.Core.Services.Interfaces.WebServices.Admin;

namespace tabApp.Core.Services.Implementations.Faturation.Helpers
{
    public class Administration
    {
        private IDialogService _dialogService;
        private IGetVehiclesRequest _getVehiclesRequest;

        public Administration(IDialogService dialogService, IGetVehiclesRequest getVehiclesRequest)
        {
            _dialogService = dialogService;
            _getVehiclesRequest = getVehiclesRequest;
        }

        public async Task<List<Car>> GetVehicles()
        {
            List<Car> cars = new List<Car>();

            await Task.Delay(2000);
            /*var response = await _getVehiclesRequest.SendAsync(new GetVehiclesInput());

            if (!response.Success)
            {
                _dialogService.ShowErrorDialog(string.Empty, response.Error);
                return cars;
            }

            response.data.ForEach(car => cars.Add(new Car()
            {
                Plate = car.license_plate,
                Id = car.id,
                Name = car.name
            }));*/

            cars.Add(new Car()
            {
                Plate = "93-OR-81",
                Name = "Caddy Maxi",
                Id = 85069
            });

            cars.Add(new Car()
            {
                Plate = "35-IG-67",
                Name = "Caddy Normal",
                Id = 85070
            });

            return cars;
        }
    }
}
