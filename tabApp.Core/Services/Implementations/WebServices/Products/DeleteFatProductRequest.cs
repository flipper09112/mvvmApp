using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;
using tabApp.Core.Services.Interfaces.WebServices.Products;
using tabApp.Core.Services.Interfaces.WebServices.Products.DTOs;

namespace tabApp.Core.Services.Implementations.WebServices.Products
{
    public class DeleteFatProductRequest : BaseRequest<DeleteFatProductInput, DeleteFatProductOutput>, IDeleteFatProductRequest
    {
        protected override string EndPoint => "/items/:id/delete";

        protected override bool HasId => true;
    }
}
