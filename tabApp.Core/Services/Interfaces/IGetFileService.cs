using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace tabApp.Core.Services.Interfaces
{
    public interface IGetFileService
    {
        Task<byte[]> GetUrlDownload(string nameFile);
    }
}
