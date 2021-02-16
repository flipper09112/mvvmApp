using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels.Global.Bt;

namespace tabApp.UI.Fragments.Global.Bt
{
    public class BtIncomingFragment : BaseFragment<BtIncomingViewModel>
    {
        private MainActivity _activity;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.BtIncomingFragment, container, false);

            _activity = ParentActivity as MainActivity;

            return view;
        }

        public override void CleanBindings()
        {
        }

        public override void SetUI()
        {
        }

        public override void SetupBindings()
        {
        }
        public void Connect()
        {
            ViewModel.StartServer();
        }
    }
}