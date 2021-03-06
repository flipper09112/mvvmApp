﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core;
using tabApp.Core.ViewModels;
using tabApp.UI.Adapters;
using tabApp.UI.Adapters.Home;
using tabApp.UI.Adapters.Swipe;

namespace tabApp.UI
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class HomeFragment : BaseFragment<HomeViewModel>
    {
        private MainActivity _activity;
        private RecyclerView _clientsList;
        private ViewPager _homeViewPager;
        private TabLayout _tabLayout;
        private ClientsListAdapter _clientsAdapter;
        private HomePageViewPagerAdapter _viewPagerAdapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.HomeFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _clientsList = view.FindViewById<RecyclerView>(Resource.Id.clientsList);
            _homeViewPager = view.FindViewById<ViewPager>(Resource.Id.homeViewPager);
            _tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabLayout);

            _clientsList.SetLayoutManager(new LinearLayoutManager(Context));

            SwipeController swipeController = new SwipeController(Context, ViewModel);
            swipeController.AttachToRecyclerView(_clientsList);

            _viewPagerAdapter = new HomePageViewPagerAdapter(ChildFragmentManager);
            _viewPagerAdapter.ViewModel = ViewModel;
            _homeViewPager.Adapter = _viewPagerAdapter;
            _tabLayout.SetupWithViewPager(_homeViewPager, true); 


            return view;
        }

        public override void SetUI()
        {
            _activity.ShowToolbar();

            _clientsAdapter = new ClientsListAdapter(ViewModel.ClientsList, ViewModel.ShowClientPage);
            _clientsList.SetAdapter(_clientsAdapter);
            _viewPagerAdapter.TabsOptions = ViewModel.TabsOptions;
            _viewPagerAdapter.NotifyDataSetChanged();
            SetupTabLayout();

            if(ViewModel.ClientSelected != null)
            {
                int pos = ViewModel.ClientsList.IndexOf(ViewModel.ClientSelected);
                _clientsList.ScrollToPosition(pos);

            }
        }
        private void SetupTabLayout()
        {
            List<string> tabsNames = new List<string>();
            _tabLayout.RemoveAllTabs();
            foreach (var tab in ViewModel?.TabsOptions)
            {
                _tabLayout.AddTab(_tabLayout.NewTab().SetText(tab.Name));
                tabsNames.Add(tab.ToString());
            }
            //_viewPagerAdapter.Title = tabsNames;
        }

        public override void SetupBindings()
        {
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            ViewModel.DeleteClientEvent += DeleteClientEvent;
        }

        public override void CleanBindings()
        {
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            ViewModel.DeleteClientEvent -= DeleteClientEvent;
        }

        private void DeleteClientEvent(object sender, EventArgs e)
        {
            Toast.MakeText(Context, "Delete", ToastLength.Long).Show();
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetUI();
        }
    }

}