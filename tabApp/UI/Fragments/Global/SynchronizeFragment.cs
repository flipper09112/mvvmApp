using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Global;
using tabApp.UI.Adapters.OtherOptions;

namespace tabApp.UI.Fragments.Global
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class SynchronizeFragment : BaseFragment<SynchronizeViewModel>
    {
        private MainActivity _activity;
        private Button _incomingButton;
        private Button _outcomingButton;
        private Spinner _spinner;
        private PrintAccountSpinnerAdapter _spinnerAdapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.SynchronizeFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _incomingButton = view.FindViewById<Button>(Resource.Id.incomingButton);
            _outcomingButton = view.FindViewById<Button>(Resource.Id.outcomingButton);
            _spinner = view.FindViewById<Spinner>(Resource.Id.spinner);

            _spinnerAdapter = new PrintAccountSpinnerAdapter(ViewModel.PairedDevices);
            _spinner.Adapter = _spinnerAdapter;

            return view;
        }

        public override void CleanBindings()
        {
            _activity.ShowMenu();

            _incomingButton.Click -= IncomingButtonClick;
            _outcomingButton.Click -= OutcomingButtonClick;
        }

        public override void SetUI()
        {
        }

        public override void SetupBindings()
        {
            _activity.HideToolbar();
            _activity.HideMenu();

            _incomingButton.Click += IncomingButtonClick;
            _outcomingButton.Click += OutcomingButtonClick;
        }

        private void OutcomingButtonClick(object sender, EventArgs e)
        {
        }

        private void IncomingButtonClick(object sender, EventArgs e)
        {
        }
    }
}