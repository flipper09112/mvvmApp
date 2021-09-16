using Android.Bluetooth;
using Android.Content;
using Java.Util;
using MvvmCross;
using MvvmCross.Platforms.Android;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Bluetooth;
using tabApp.Services.Implementations.Native;

namespace tabApp.Core.Services.Implementations.Bluetooth
{
    public class BluetoothService : IBluetoothService
    {
        private string UIID = "00001101-0000-1000-8000-00805f9b34fb";
        private BluetoothSocket _bluetoothSocket;

        public BluetoothServerSocket ServerSocket { get; private set; }
        public BluetoothSocket Socket { get; private set; }

        public string BTDefaultDevice => "MTP-II";

        public Action LoadingLottie { get; private set; }

        public Action ErrorLottie { get; private set; }

        public Action FinishedLottie { get; private set; }
        public Action IncrementProgressiveBar { get; private set; }
        public Action<List<Client>> ReceiveNewClientsData { get; private set; }

        #region Print
        public List<string> GetPairedDevices()
        {
            List<string> item = new List<string>();

            BluetoothAdapter mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            var pairedDevices = mBluetoothAdapter.BondedDevices;

            foreach (BluetoothDevice bt in pairedDevices)
                item.Add(bt.Name);

            return item;
        }

        public void SendData(string preview)
        {
            using (BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter)
            {
                BluetoothDevice device = GetDevice(bluetoothAdapter, BTDefaultDevice);
                try
                {
                    using (BluetoothSocket bluetoothSocket = device?.
                        CreateRfcommSocketToServiceRecord(
                        UUID.FromString(UIID)))
                    {
                        bluetoothSocket?.Connect();
                        byte[] buffer = Encoding.UTF8.GetBytes(preview);
                        bluetoothSocket?.OutputStream.Write(buffer, 0, buffer.Length);
                        bluetoothSocket.Close();
                    }
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }
        public async Task SendDataAsync(string preview, string pairedDevicesSelected)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(preview);
            await _bluetoothSocket?.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        }

        public async Task SendDataAsync(byte[] preview, string pairedDevicesSelected)
        {
            await _bluetoothSocket?.OutputStream.WriteAsync(preview, 0, preview.Length);
        }

        private BluetoothDevice GetDevice(BluetoothAdapter bluetoothAdapter, string bTDefaultDevice)
        {
            var btdevice = bluetoothAdapter?.BondedDevices.Select(i => i.Name).ToList();
            return (from bd in bluetoothAdapter?.BondedDevices
                    where bd?.Name == bTDefaultDevice 
                    select bd).FirstOrDefault();
        }
        #endregion

        public async void StartServerSocket(Action disableLottie, Action errorLottie, Action finishedLottie, Action<List<Client>> receiveNewClientsData)
        {
            LoadingLottie = disableLottie;
            ErrorLottie = errorLottie;
            FinishedLottie = finishedLottie;
            ReceiveNewClientsData = receiveNewClientsData;

            using (BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter)
            {
                var btdevice = bluetoothAdapter?.BondedDevices.Select(i => i.Name).ToList();

                try
                {
                    ServerSocket = bluetoothAdapter.ListenUsingRfcommWithServiceRecord("connect", UUID.FromString(UIID));

                    var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
                    var act = top.Activity;

                    Intent service = new Intent(act, typeof(BluetoothManagerService));
                    service.PutExtra("serverType", true);

                    act.StartService(service);

                }
                catch (Exception exp)
                {

                }
            }
        }

        public void StopServerSocket()
        {
            ServerSocket?.Close();
            var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            var act = top.Activity;
            Intent service = new Intent(act, typeof(BluetoothManagerService));
            service.PutExtra("serverType", true);

            act.StopService(service);
        }

        public void InitSynchronizeData(Action connectedLottie, Action errorLottie, Action finishedLottie, Action incrementProgressiveBar)
        {
            LoadingLottie = connectedLottie;
            ErrorLottie = errorLottie;
            FinishedLottie = finishedLottie;
            IncrementProgressiveBar = incrementProgressiveBar;

            using (BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter)
            {
                BluetoothDevice device = GetDevice(bluetoothAdapter, "HUAWEI MediaPad T5"/*"Galaxy Tab A de Filipe"*/); //TODO
                Socket = device.CreateRfcommSocketToServiceRecord(UUID.FromString(UIID));

                var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
                var act = top.Activity;

                Intent service = new Intent(act, typeof(BluetoothManagerService));
                service.PutExtra("serverType", false);

                act.StartService(service);
            }
        }

        public void Connect(string pairedDevicesSelected)
        {
            using (BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter)
            {
                BluetoothDevice device = GetDevice(bluetoothAdapter, pairedDevicesSelected);
                try
                {
                    _bluetoothSocket = device?.
                        CreateRfcommSocketToServiceRecord(
                        UUID.FromString(UIID));

                    _bluetoothSocket?.Connect();
                    
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }

        public void SendData(byte slice, string pairedDevicesSelected)
        {
            _bluetoothSocket?.OutputStream.WriteByte(slice);
        }

        public void DisConnect(string pairedDevicesSelected)
        {
            _bluetoothSocket?.Close();
        }
    }
}
