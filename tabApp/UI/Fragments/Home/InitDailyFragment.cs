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
using tabApp.Core;
using tabApp.Core.ViewModels;

namespace tabApp.UI.Fragments.Home
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class InitDailyFragment : BaseFragment<InitDailyViewModel>
    {
        private MainActivity _activity;
        private Button _activateButton;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.InitDailyFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _activateButton = view.FindViewById<Button>(Resource.Id.button);

            return view;
        }

        public override void CleanBindings()
        {
            ViewModel.GoBack -= GoBack;
            _activateButton.Click -= ActivateButtonClick;
        }

        public override void SetUI()
        {
            _activity.HideToolbar();
        }

        public override void SetupBindings()
        {
            ViewModel.GoBack += GoBack;
            _activateButton.Click += ActivateButtonClick;
        }

        private void GoBack(object sender, EventArgs e)
        {
            _activity.SupportFragmentManager.PopBackStack();
        }

        private void ActivateButtonClick(object sender, EventArgs e)
        {
            ViewModel.ActivateCommand.Execute(null);
        }
    }
}