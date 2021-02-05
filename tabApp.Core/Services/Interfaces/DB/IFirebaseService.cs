using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace tabApp.Core.Services.Interfaces
{
    public interface IFirebaseService
    {
        Task<byte[]> GetUrlDownload(string nameFile);
        Task SaveFile(string nameFile, byte[] fileBytes);
    }
}
