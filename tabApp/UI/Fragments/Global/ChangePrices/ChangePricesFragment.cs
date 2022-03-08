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
using tabApp.Core.ViewModels.Global.ChangePrices;

namespace tabApp.UI.Fragments.Global.ChangePrices
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class ChangePricesFragment : BaseFragment<ChangePricesViewModel>
    {
        private MainActivity _activity;
        private Button _continueBt;
        private TextView _dateTv;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.ChangePricesFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _continueBt = view.FindViewById<Button>(Resource.Id.continueBt);
            _dateTv = view.FindViewById<TextView>(Resource.Id.dateTv);

            return view;
        }

        public override void SetUI()
        {
            _dateTv.Text = ViewModel.DateSelected.ToString("dd/MM/yyyy");
        }

        public override void SetupBindings()
        {
            _dateTv.Click += DateTvClick;
            _continueBt.Click += ContinueBtClick;
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            ViewModel.GoBack2Times += GoBack2Times;
        }

        public override void CleanBindings()
        {
            _dateTv.Click -= DateTvClick;
            _continueBt.Click -= ContinueBtClick;
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            ViewModel.GoBack2Times -= GoBack2Times;
        }

        private void GoBack2Times(object sender, EventArgs e)
        {
            _activity.SupportFragmentManager.PopBackStack();
            _activity.SupportFragmentManager.PopBackStack();
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetUI();
        }

        private void ContinueBtClick(object sender, EventArgs e)
        {
            ViewModel.ContinueCommand.Execute();
        }

        private void DateTvClick(object sender, EventArgs e)
        {
            ViewModel.DateClickCommand.Execute();
        }
    }
}