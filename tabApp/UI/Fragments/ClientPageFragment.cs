using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels;

namespace tabApp.UI.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, false)]
    public class ClientPageFragment : BaseFragment<ClientPageViewModel>
    {
        private TextView _clientName;
        private TextView _payDate;
        private Spinner _spinnerDates;
        private TextView _ammountToPay;
        private Button _payButton;
        private ViewPager _viewPager;
        private TabLayout _tabLayout;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.ClientPageFragment, container, false);

            _clientName = view.FindViewById<TextView>(Resource.Id.clientName);
            _payDate = view.FindViewById<TextView>(Resource.Id.payDate);
            _spinnerDates = view.FindViewById<Spinner>(Resource.Id.spinnerDates);
            _ammountToPay = view.FindViewById<TextView>(Resource.Id.ammountToPay);
            _payButton = view.FindViewById<Button>(Resource.Id.payButton);
            _viewPager = view.FindViewById<ViewPager>(Resource.Id.viewPager);
            _tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabLayout);

            return view;
        }

        public override void SetUI()
        {
            _clientName.Text = ViewModel.Client.Name;
        }
    }
}