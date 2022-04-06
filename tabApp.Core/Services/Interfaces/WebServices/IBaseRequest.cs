using System.Threading.Tasks;

namespace tabApp.Core.Services.Interfaces.WebServices
{
    public interface IBaseRequest<Tinput, Toutput> where Tinput : BaseInput where Toutput : BaseOutput
    {
        Task<Toutput> Send(Tinput input);
    }
}