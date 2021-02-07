using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Services.Interfaces.Bluetooth
{
    public interface IBluetoothService
    {
        string BTDefaultDevice { get; }
        List<String> GetPairedDevices();
        void SendData(string preview);
    }
}
