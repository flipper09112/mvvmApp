using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Services.Interfaces
{
    public interface ISaveFileService
    {
        bool HasFile(string fileName);
        void SaveFile(string fileName, byte[] data);
    }
}
