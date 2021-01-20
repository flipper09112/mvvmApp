using System;
using System.Collections.Generic;

using System.Text;

namespace tabApp.Core.Services.Interfaces
{
    public interface IFileService
    {
        bool HasFile(string fileName);
        byte[] GetFile(string fileName);
        void SaveFile(string fileName, byte[] data);
    }
}
