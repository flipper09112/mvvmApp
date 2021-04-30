using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Bases;
using tabApp.UI.Adapters;

namespace tabApp.UI.Fragments.Bases
{
    public partial class BaseOptionsListFragment<T> : BaseFragment
    {
        protected MainActivity _activity;
        protected GridView _otherOptionsGrid;
        protected OtherOptionsGridAdapter _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _activity = ParentActivity as MainActivity;
            View view = inflater.Inflate(Resource.Layout.OtherOptionsFragment, container, false);

            _otherOptionsGrid = view.FindViewById<GridView>(Resource.Id.otherOptionsGrid);

            return view;
        }

        public override void SetUI()
        {
            _activity.HideToolbar();
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            _adapter = new OtherOptionsGridAdapter(((BaseOptionsListViewModel)ViewModel).Options);
            _otherOptionsGrid.SetAdapter(_adapter);
        }

        public override void CleanBindings()
        {
        }

        public override void SetupBindings()
        {
        }
    }
}