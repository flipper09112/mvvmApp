using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Services.Interfaces.WebServices;

namespace tabApp.Core.Services.Implementations.WebServices.Products
{
    public class GetProductsRequest : BaseRequest<GetProductsInput, GetProductsOutput> , IGetProductsRequest
    {
        protected override string ApiMethod => "/api/Products/GetProducts";

        public Task<GetProductsOutput> Send(GetProductsInput input)
        {
            return base.Send(input, HttpMethod.Get);
        }
    }
}
