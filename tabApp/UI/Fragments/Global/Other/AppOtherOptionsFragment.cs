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
using tabApp.Core.ViewModels.Global.Other;
using tabApp.UI.Adapters;

namespace tabApp.UI.Fragments.Global.Other
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class AppOtherOptionsFragment : BaseFragment<AppOtherOptionsViewModel>
    {
        private MainActivity _activity;
        private GridView _otherOptionsGrid;
        private OtherOptionsGridAdapter _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _activity = ParentActivity as MainActivity;
            
            View view = inflater.Inflate(Resource.Layout.AppOtherOptionsFragment, container, false);

            _otherOptionsGrid = view.FindViewById<GridView>(Resource.Id.otherOptionsGrid);

            return view;
        }

        public override void CleanBindings()
        {
            _activity.ShowMenu();
        }

        public override void SetUI()
        {
            UpdateGrid();
        }
        private void UpdateGrid()
        {
            _adapter = new OtherOptionsGridAdapter(ViewModel.Options);
            _otherOptionsGrid.SetAdapter(_adapter);
        }

        public override void SetupBindings()
        {
            _activity.HideMenu();
            _activity.HideToolbar();
        }
    }
}