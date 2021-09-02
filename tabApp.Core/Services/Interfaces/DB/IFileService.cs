using Java.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Services.Interfaces
{
    public interface IFileService
    {
        bool HasFile(string fileName);
        byte[] GetFile(string fileName);
        File SaveFile(string fileName, byte[] data, bool overwrite = false);
        void DeleteFile(string dataBaseName);
    }
}
