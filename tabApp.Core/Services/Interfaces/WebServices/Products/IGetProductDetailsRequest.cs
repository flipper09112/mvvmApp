using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Products.DTOs;

namespace tabApp.Core.Services.Interfaces.WebServices.Products
{
    public interface IGetProductDetailsRequest : IBaseRequest<GetProductDetailsInput, GetProductDetailsOutput>
    {
    }
}
