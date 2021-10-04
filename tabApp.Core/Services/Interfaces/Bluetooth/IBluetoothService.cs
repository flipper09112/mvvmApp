using Android.Bluetooth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Models;

namespace tabApp.Core.Services.Interfaces.Bluetooth
{
    public interface IBluetoothService
    {
        BluetoothServerSocket ServerSocket { get; }
        BluetoothSocket Socket { get; }
        Action LoadingLottie { get; }
        Action ErrorLottie { get; }
        Action FinishedLottie { get; }
        Action IncrementProgressiveBar { get; }
        Action<List<Client>> ReceiveNewClientsData { get; }

        string BTDefaultDevice { get; }
        List<String> GetPairedDevices();
        void SendData(string preview);
        void StartServerSocket(Action disableLottie, Action errorLottie, Action finishedLottie, Action<List<Client>> receiveNewClientsData);
        void StopServerSocket();
        void InitSynchronizeData(Action connectedLottie, Action errorLottie, Action finishedLottie, Action IncrementProgressiveBar);
        Task SendDataAsync(byte[] vs, string pairedDevicesSelected);
        Task SendDataAsync(string vs, string pairedDevicesSelected);
        void Connect(string pairedDevicesSelected);
        void SendData(byte slice, string pairedDevicesSelected);
        void DisConnect(string pairedDevicesSelected);
    }
}
