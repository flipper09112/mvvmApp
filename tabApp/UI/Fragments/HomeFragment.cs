using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core;
using tabApp.Core.ViewModels;

namespace tabApp.UI
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, false)]
    [Register("tabApp.droid.Fragments.HomeFragment")]
    public class HomeFragment : BaseFragment<HomeViewModel>
    {
        private RecyclerView _clientsList;
        private RecyclerView _ordersList;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.HomeFragment, container, false);

            _clientsList = view.FindViewById<RecyclerView>(Resource.Id.clientsList);
            _ordersList = view.FindViewById<RecyclerView>(Resource.Id.ordersList);

            return view;
        }

        public override void SetUI()
        {
        }
    }
}