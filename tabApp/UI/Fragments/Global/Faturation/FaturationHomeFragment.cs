using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.CardView.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Global;
using tabApp.Core.ViewModels.Global.Faturation;

namespace tabApp.UI.Fragments.Global.Faturation
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class FaturationHomeFragment : BaseFragment<FaturationHomeViewModel>
    {
        private CardView _trasnportationDocs;
        private CardView _faturationDocs;
        private ImageView _faturalusaIcon;
        private MainActivity _activity;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.FaturationHomeFragment, container, false);

            _trasnportationDocs = view.FindViewById<CardView>(Resource.Id.trasnportationDocs);
            _faturationDocs = view.FindViewById<CardView>(Resource.Id.faturationDocs);
            _faturalusaIcon = view.FindViewById<ImageView>(Resource.Id.faturalusaIcon);

            _activity = ParentActivity as MainActivity;

            return view;
        }

        public override void SetUI()
        {
            _activity.HideMenu();
            _activity.HideToolbar();
        }

        public override void SetupBindings()
        {
            _trasnportationDocs.Click += TrasnportationDocsClick;
            _faturationDocs.Click += FaturationDocsClick;
            _faturalusaIcon.Click += FaturalusaIconClick;
        }

        public override void CleanBindings()
        {
            _trasnportationDocs.Click -= TrasnportationDocsClick;
            _faturationDocs.Click -= FaturationDocsClick;
            _faturalusaIcon.Click -= FaturalusaIconClick;
        }

        private void FaturalusaIconClick(object sender, EventArgs e)
        {
            ViewModel.ShowFaturaLusaCommand.Execute();
        }

        private void FaturationDocsClick(object sender, EventArgs e)
        {
            ViewModel.ShowFaturationPageCommand.Execute();
        }

        private void TrasnportationDocsClick(object sender, EventArgs e)
        {
            ViewModel.ShowTransportationDocsPageCommand.Execute();
        }
    }
}