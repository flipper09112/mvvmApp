using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace tabApp.Core.Services.Interfaces.WebServices.Bases
{
    public interface IBaseRequest<Tinput, Toutput> where Tinput : BaseInput where Toutput : BaseOutput
    {
        Task<Toutput> SendAsync(Tinput input);
    }
}
