using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;
using tabApp.Core.Services.Interfaces.WebServices.Products;
using tabApp.Core.Services.Interfaces.WebServices.Products.DTOs;

namespace tabApp.Core.Services.Implementations.WebServices.Products
{
    public class UpdateFatProductRequest : BaseRequest<UpdateFatProductInput, UpdateFatProductOutput>, IUpdateFatProductRequest
    {
        protected override string EndPoint => "/items/:id/update";

        protected override bool HasId => true;
    }
}
