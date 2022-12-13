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
            var response = await _getVehiclesRequest.SendAsync(new GetVehiclesInput());

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
            }));

            return cars;
        }
    }
}
