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
using tabApp.Core.ViewModels.Global.PriceTable;

namespace tabApp.UI.Fragments.Global.PriceTable
{
    public class EditProductFragment : BaseFragment<EditProductViewModel>
    {
        private MainActivity _activity;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.EditProductFragment, container, false);

            _activity = ParentActivity as MainActivity;

            return view;
        }

        public override void SetUI()
        {
        }

        public override void SetupBindings()
        {
        }
        public override void CleanBindings()
        {
        }
    }
}