using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Models.Faturation;

namespace tabApp.Core.Services.Interfaces
{
    public interface IFileService
    {
        bool HasFile(string fileName);
        byte[] GetFile(string fileName);
        Task<bool> GetWebFile(string url, string fileName);
        object SaveFile(string fileName, byte[] data, bool overwrite = false);
        void DeleteFile(string dataBaseName);
        void ShowPdfExternalApp(TrasnportationDoc documentSelected);
        void ClearAllFilesFromPath();
    }
}
