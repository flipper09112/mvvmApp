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
using System.Linq;
using System.Text;
using tabApp.Core.Services.Interfaces.Bluetooth;

namespace tabApp.Services.Implementations.Native
{
    public class BluetoothManagerService : Service
    {
        private BluetoothSocket socket;
        private BluetoothServerSocket serverSocket;
        private InputStream mmInStream;
        private OutputStream mmOutStream;

        public override IBinder OnBind(Intent intent)
        {

            WaitConnection();
            return null;

        }

        private void WaitConnection()
        {
            var top = Mvx.Resolve<IBluetoothService>();
            socket = top.Socket;
            serverSocket = top.ServerSocket;

            while (true)
            {
                try
                {
                    socket = serverSocket.Accept();
                }
                catch (Exception e)
                {
                    //Debug.WriteLine("Acept server socket bt error");
                    break;
                }

                if (socket != null)
                {
                    // A connection was accepted. Perform work associated with
                    // the connection in a separate thread.
                    ManageMyConnectedSocket(socket);
                    break;
                }
            }
        }

        private void ManageMyConnectedSocket(BluetoothSocket socket)
        {
            InitStreams();

            ReadDate();
        }

        private void ReadDate()
        {
            byte[] mmBuffer = new byte[1024];
            int numBytes; // bytes returned from read()

            // Keep listening to the InputStream until an exception occurs.
            while (true)
            {
                try
                {
                    // Read from the InputStream.
                    numBytes = mmInStream.Read(mmBuffer);
                    int X = 2;
                    // Send the obtained bytes to the UI activity.
                   /* Message readMsg = handler.obtainMessage(
                            MessageConstants.MESSAGE_READ, numBytes, -1,
                            mmBuffer);
                    readMsg.sendToTarget();*/
                }
                catch (IOException e)
                {
                    break;
                }
            }

        }

        private void InitStreams()
        {
            InputStream tmpIn = null;
            OutputStream tmpOut = null;

            // Get the input and output streams; using temp objects because
            // member streams are final.
            try
            {
                tmpIn = ((Android.Runtime.InputStreamInvoker)socket.InputStream).BaseInputStream;
            }
            catch (IOException e)
            {
            }
            try
            {
                tmpOut = ((Android.Runtime.OutputStreamInvoker)socket.OutputStream).BaseOutputStream;
            }
            catch (IOException e)
            {
            }

            mmInStream = tmpIn;
            mmOutStream = tmpOut;
        }
    }
}