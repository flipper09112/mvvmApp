using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.CardView.Widget;
using AndroidX.RecyclerView.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Global.MonthBills;
using tabApp.Helpers;
using tabApp.UI.Adapters.Global;
using tabApp.UI.Adapters.OtherOptions;

namespace tabApp.UI.Fragments.Global.MonthBills
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class MonthBillsHomeFragment : BaseFragment<MonthBillsHomeViewModel>
    {
        private MainActivity _activity;
        private RecyclerView _clientsRv;
        private CardView _printButton;
        private Spinner _spinner;
        private PrintAccountSpinnerAdapter _spinnerAdapter;
        private MonthBillsClientsAdapter _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.MonthBillsHomeFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _clientsRv = view.FindViewById<RecyclerView>(Resource.Id.clientsRv);
            _printButton = view.FindViewById<CardView>(Resource.Id.printButton); 
            _spinner = view.FindViewById<Spinner>(Resource.Id.spinner);

            _clientsRv.SetLayoutManager(new LinearLayoutManager(Context));
            return view;
        }

        public override void SetUI()
        {
            _activity.HideToolbar();

            SetList();
            SetSpinner();
        }

        private void SetSpinner()
        {
            if (_spinnerAdapter != null) return;
            _spinnerAdapter = new PrintAccountSpinnerAdapter(ViewModel.PairedDevices);
            _spinner.Adapter = _spinnerAdapter;
        }

        private void SetList()
        {
            if (_adapter != null) return;
            _adapter = new MonthBillsClientsAdapter(ViewModel.MonthClients);
            _clientsRv.SetAdapter(_adapter);
        }

        public override void SetupBindings()
        {
            _printButton.Click += PrintButtonClick;
            _spinner.ItemSelected += SpinnerItemSelected;
            ViewModel.PrintLogo += PrintLogo;
        }
        public async Task<bool> PrintLogo()
        {
            try
            { 
                await PrintPhotoAsync(PrinterHelper.GetLogo(Resources, Resource.Drawable.logo_240));
                return true;
            }
            catch (Exception e)
            {
            }

            return false;
        }

        private static char ESC_CHAR = (char)0x1B;
        private static char GS = (char)0x1D;
        private static byte[] LINE_FEED = new byte[] { 0x0A };
        private static byte[] CUT_PAPER = new byte[] { (byte)GS, 0x56, 0x00 };
        private static byte[] INIT_PRINTER = new byte[] { (byte)ESC_CHAR, 0x40 };
        private static byte[] SELECT_BIT_IMAGE_MODE = { 0x1B, 0x2A, 33 };
        private static byte[] SET_LINE_SPACE_24 = new byte[] { (byte)ESC_CHAR, 0x33, 24 };
        private static byte[] SET_LINE_SPACE_30 = new byte[] { (byte)ESC_CHAR, 0x33, 30 };

        private async Task PrintPhotoAsync(int[,] vs)
        {
            await PrintImageAsync(vs);
        }

        private async Task PrintImageAsync(int[,] pixels)
        {
            // Set the line spacing at 24 (we'll print 24 dots high)
            await ViewModel.BluetoothService.SendDataAsync(SET_LINE_SPACE_24, ViewModel.PairedDevicesSelected);
            for (int y = 0; y < pixels.Length; y += 24)
            {
                // Like I said before, when done sending data, 
                // the printer will resume to normal text printing
                await ViewModel.BluetoothService.SendDataAsync(SELECT_BIT_IMAGE_MODE, ViewModel.PairedDevicesSelected);

                // Set nL and nH based on the width of the image
                await ViewModel.BluetoothService.SendDataAsync(new byte[]{(byte)(0x00ff &  PrinterHelper.Width)
                             , (byte)((0xff00 & PrinterHelper.Width) >> 8)}, ViewModel.PairedDevicesSelected);

                for (int x = 0; x < PrinterHelper.Width; x++)
                {
                    // for each stripe, recollect 3 bytes (3 bytes = 24 bits)
                    try
                    {
                        await ViewModel.BluetoothService.SendDataAsync(RecollectSlice(y, x, pixels), ViewModel.PairedDevicesSelected);

                    }
                    catch(Exception e)
                    {
                        var xxx = 23;
                    }
                }

                // Do a line feed, if not the printing will resume on the same line
                await ViewModel.BluetoothService.SendDataAsync(LINE_FEED, ViewModel.PairedDevicesSelected);
            }
            //await ViewModel.BluetoothService.SendDataAsync(SET_LINE_SPACE_24, ViewModel.PairedDevicesSelected);
            await ViewModel.BluetoothService.SendDataAsync(LINE_FEED, ViewModel.PairedDevicesSelected);
            await ViewModel.BluetoothService.SendDataAsync(LINE_FEED, ViewModel.PairedDevicesSelected);
            await ViewModel.BluetoothService.SendDataAsync(LINE_FEED, ViewModel.PairedDevicesSelected);
            await ViewModel.BluetoothService.SendDataAsync(LINE_FEED, ViewModel.PairedDevicesSelected);
        }

        private byte[] RecollectSlice(int y, int x, int[,] img)
        {
            byte[] slices = new byte[] { 0, 0, 0 };
            for (int yy = y, i = 0; yy < y + 24 && i < 3; yy += 8, i++)
            {
                byte slice = 0;
                for (int b = 0; b < 8; b++)
                {
                    int yyy = yy + b;
                    if (yyy >= img.Length)
                    {
                        continue;
                    }
                    int col = img[yyy, x];
                    bool v = shouldPrintColor(col);
                    slice |= (byte)((v ? 1 : 0) << (7 - b));
                }
                slices[i] = slice;
            }

            return slices;
        }

        private bool shouldPrintColor(int col)
        {
            int threshold = 127;
            int a, r, g, b, luminance;
            a = (col >> 24) & 0xff;
            if (a != 0xff)
            {// Ignore transparencies
                return false;
            }
            r = (col >> 16) & 0xff;
            g = (col >> 8) & 0xff;
            b = col & 0xff;

            luminance = (int)(0.299 * r + 0.587 * g + 0.114 * b);

            return luminance < threshold;
        }

        public override void CleanBindings()
        {
            _adapter = null; _spinnerAdapter = null;
            _printButton.Click -= PrintButtonClick;
            _spinner.ItemSelected -= SpinnerItemSelected;
            ViewModel.PrintLogo -= PrintLogo;
        }

        private void SpinnerItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ViewModel.PairedDevicesSelected = ViewModel.PairedDevices[e.Position];
        }

        private void PrintButtonClick(object sender, EventArgs e)
        {
            ViewModel.PrintCommand.Execute();
        }

    }
}