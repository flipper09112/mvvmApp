using Android.Bluetooth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace tabApp.Core.Services.Interfaces.Bluetooth
{
    public interface IBluetoothService
    {
        BluetoothSocket Socket { get; }
        BluetoothServerSocket ServerSocket { get; }

        string BTDefaultDevice { get; }
        List<String> GetPairedDevices();
        void SendData(string preview);
        void StartServerSocket();
        void StopServerSocket();
    }
}
