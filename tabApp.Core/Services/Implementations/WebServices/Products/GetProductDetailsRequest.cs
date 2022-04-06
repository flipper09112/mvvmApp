using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Services.Interfaces.WebServices.Products;
using tabApp.Core.Services.Interfaces.WebServices.Products.DTOs;

namespace tabApp.Core.Services.Implementations.WebServices.Products
{
    public class GetProductDetailsRequest : BaseRequest<GetProductDetailsInput, GetProductDetailsOutput>, IGetProductDetailsRequest
    {
        protected override string ApiMethod => "/api/Products/GetProductDetails";

        public Task<GetProductDetailsOutput> Send(GetProductDetailsInput input)
        {
            return base.Send(input, HttpMethod.Get);
        }
    }
}
