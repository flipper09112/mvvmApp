using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.ClientPage.OtherOptions;

namespace tabApp.UI.Fragments.ClientPage.OtherOptions
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class PrintAccountFragment : BaseFragment<PrintAccountViewModel>
    {
        private MainActivity _activity;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.PrintAccountFragment, container, false);

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
    }
}