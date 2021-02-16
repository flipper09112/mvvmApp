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
using tabApp.Core.Services.Interfaces.Bluetooth;
using tabApp.Services.Implementations.Native;

namespace tabApp.Core.Services.Implementations.Bluetooth
{
    public class BluetoothService : IBluetoothService
    {
        public BluetoothServerSocket ServerSocket { get; private set; }
        public BluetoothSocket Socket { get; private set; }

        public string BTDefaultDevice => "MTP-II";

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
                var btdevice = bluetoothAdapter?.BondedDevices.Select(i => i.Name).ToList();
                BluetoothDevice device = (from bd in bluetoothAdapter?.BondedDevices
                                          where bd?.Name == BTDefaultDevice //TODO
                                          select bd).FirstOrDefault();
                try
                {
                    using (BluetoothSocket bluetoothSocket = device?.
                        CreateRfcommSocketToServiceRecord(
                        UUID.FromString("00001101-0000-1000-8000-00805f9b34fb")))
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

        public async void StartServerSocket()
        {
            using (BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter)
            {
                var btdevice = bluetoothAdapter?.BondedDevices.Select(i => i.Name).ToList();
               /*BluetoothDevice device = (from bd in bluetoothAdapter?.BondedDevices
                                          where bd?.Name == BTDefaultDevice //TODO
                                          select bd).FirstOrDefault();*/
                try
                {
                    ServerSocket = bluetoothAdapter.ListenUsingRfcommWithServiceRecord("connect", UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));

                    var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
                    var act = top.Activity;

                    Intent service = new Intent(act, typeof(BluetoothManagerService));

                    act.StartService(service);

                    
                }
                catch (Exception exp)
                {

                }
            }
        }

        public void StopServerSocket()
        {
            Socket?.Close();
        }
    }
}
