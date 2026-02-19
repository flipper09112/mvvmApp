using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;
using tabApp.Core.Services.Interfaces.WebServices.Products.DTOs;

namespace tabApp.Core.Services.Interfaces.WebServices.Products
{
    public interface IUpdateFatProductRequest : IBaseRequest<UpdateFatProductInput, UpdateFatProductOutput>
    {
    }
}
