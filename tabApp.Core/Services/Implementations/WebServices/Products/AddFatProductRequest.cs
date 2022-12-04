using Spire.Pdf.Fields;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices.Bases;
using tabApp.Core.Services.Interfaces.WebServices.Products;
using tabApp.Core.Services.Interfaces.WebServices.Products.DTOs;

namespace tabApp.Core.Services.Implementations.WebServices.Products
{
    public class AddFatProductRequest : BaseRequest<AddFatProductInput, AddFatProductOutput>, IAddFatProductRequest
    {
        protected override HttpMethod HttpMethod => HttpMethod.Post;

        protected override string EndPoint => "/items/create";
    }
}
