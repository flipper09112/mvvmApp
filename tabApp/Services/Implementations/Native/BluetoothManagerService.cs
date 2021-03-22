using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using MvvmCross;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Bluetooth;

namespace tabApp.Services.Implementations.Native
{
    [Service]
    public class BluetoothManagerService : IntentService
    {
        private BluetoothServerSocket serverSocket;
        private static InputStream mmInStream;
        private static OutputStream mmOutStream;
        private static BluetoothSocket socket;
        private static List<Client> newClientsData = new List<Client>();

        private static byte[] mmBuffer = new byte[100000];
        private static int numBytes; // bytes returned from read()

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        protected override void OnHandleIntent(Intent intent)
        {
            if (intent.GetBooleanExtra("serverType", false))
                WaitConnection();
            else
                Connect();
        }

        private void Connect()
        {
            var btService = Mvx.Resolve<IBluetoothService>();
            socket = btService.Socket;

            while (true)
            {
                try
                {
                    socket.Connect();
                }
                catch (Exception e)
                {
                    btService.ErrorLottie?.Invoke();
                    break;
                }

                if (socket != null)
                {
                    btService.LoadingLottie?.Invoke();
                    InitStreams();
                    break;
                }
            }
        }


        private void WaitConnection()
        {
            var btService = Mvx.Resolve<IBluetoothService>();
            serverSocket = btService.ServerSocket;

            while (true)
            {
                try
                {
                    socket = serverSocket.Accept();
                    newClientsData?.Clear();
                }
                catch (Exception e)
                {
                    btService.ErrorLottie?.Invoke();
                    break;
                }

                if (socket != null)
                {
                    btService.LoadingLottie?.Invoke();
                    ManageMyConnectedSocket(socket);
                    break;
                }
            }
        }

        private void ManageMyConnectedSocket(BluetoothSocket socket)
        {
            InitStreams();
            ReadDateAsync();
        }

        private async System.Threading.Tasks.Task ReadDateAsync()
        {
            var btService = Mvx.Resolve<IBluetoothService>();
            serverSocket = btService.ServerSocket;

            while (true)
            {
                try
                {
                    while (!socket.InputStream.CanRead || !socket.InputStream.IsDataAvailable())
                    {
                    }
                    Thread.Sleep(250);
                    numBytes = socket.InputStream.Read(mmBuffer);

                    Client client = ReadClient(mmBuffer, numBytes);
                    newClientsData.Add(client);
                    int id = client.Id;
                    Write("read client");
                }
                catch (Exception e)
                {
                    BinaryFormatter bformatter = new BinaryFormatter();
                    MemoryStream ms = new MemoryStream();

                    ms.Write(mmBuffer, 0, numBytes);
                    ms.Position = 0;

                    try
                    {
                        string eof = (string) bformatter.Deserialize(ms);

                        if (eof.Equals("end"))
                        {
                            serverSocket.Close();
                            btService.ReceiveNewClientsData?.Invoke(newClientsData);
                            btService.FinishedLottie?.Invoke();
                            return;
                        }

                    } catch(Exception ex)
                    {
                        btService.ErrorLottie?.Invoke();
                        return;
                    }
                }
            }
        }

        private Client ReadClient(byte[] mmBuffer, int numBytes)
        {
            BinaryFormatter bformatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            ms.Write(mmBuffer, 0, numBytes);
            ms.Position = 0;

            return (Client)bformatter.Deserialize(ms);
        }

        private void InitStreams()
        {
            InputStream tmpIn = null;
            OutputStream tmpOut = null;

            try
            {
                tmpIn = ((Android.Runtime.InputStreamInvoker)socket.InputStream).BaseInputStream;
            }
            catch (Exception e)
            {
            }
            try
            {
                tmpOut = ((Android.Runtime.OutputStreamInvoker)socket.OutputStream).BaseOutputStream;
            }
            catch (Exception e)
            {
            }

            mmInStream = tmpIn;
            mmOutStream = tmpOut;
        }

        private static void Write(string txt)
        {
            try
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bformatter.Serialize(ms, txt);
                mmOutStream.Write(ms.ToArray());
                //mmOutStream.Flush();
            }
            catch (Exception e)
            {

            }
        } 

        internal static void Write(List<Client> client)
        {
            var btService = Mvx.Resolve<IBluetoothService>();

            byte[] mmBuffer = new byte[50];
            try
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();

                for(int i = 0; i < client.Count; i++)
                {
                    bformatter.Serialize(ms, client[i]);
                    mmOutStream.Write(ms.ToArray());
                    mmOutStream.Flush();
                    ms = new MemoryStream();

                    btService.IncrementProgressiveBar?.Invoke();
                    mmInStream.Read(mmBuffer);
                }
                Write("end");
                btService.FinishedLottie?.Invoke();
            }
            catch (Exception e)
            {
                btService.ErrorLottie?.Invoke();
            }
        }
    }
}