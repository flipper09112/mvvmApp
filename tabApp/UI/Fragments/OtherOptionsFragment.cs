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

namespace tabApp.UI.Fragments
{
    public class OtherOptionsFragment : BaseFragment<OtherOptionsViewModel>
    {
        private GridLayout _otherOptionsGrid;

        public OtherOptionsFragment()
        {
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.OtherOptionsFragment, container, false);

            _otherOptionsGrid = view.FindViewById<GridLayout>(Resource.Id.otherOptionsGrid);

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