

using Android.Support.V4.App;
using Android.Views;
using System;
using System.Collections.Generic;
using tabApp.Core;
using tabApp.UI.Fragments.Home.ViewPager;

namespace tabApp.UI.Adapters.Home
{
    public class HomePageViewPagerAdapter : FragmentStatePagerAdapter
    {
        public List<SecondaryOptions> TabsOptions;
        public HomePageOrdersFragment encomenda;
        private FragmentManager _fm;

        public override int Count => TabsOptions?.Count ?? 0;
        public HomeViewModel ViewModel { get; internal set; }

        public HomePageViewPagerAdapter(Android.Support.V4.App.FragmentManager fm) : base(fm)
        {
            _fm = fm;
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            if (TabsOptions[position].Name.Equals("Encomendas"))
            {
                encomenda = new HomePageOrdersFragment((OrdersPage)TabsOptions[position], ViewModel);
                return encomenda;
            }
            else if (TabsOptions[position].Name.Equals("Localização"))
            {
                return new HomePageMapFragment(ViewModel);
            }
            else if (TabsOptions[position] is NotificationsPage notificationData)
            {
                return new HomeNotificationsFragment(ViewModel, notificationData);
            }
            return null;
        }

        internal void UpdateOrdersList()
        {
            if(encomenda == null)
            {
                var fragments = _fm.Fragments;
                foreach (Fragment fragment in fragments)
                {
                    if (fragment is HomePageOrdersFragment homePageOrdersFragment)
                    {
                        homePageOrdersFragment.UpdateOrdersList();
                        return;
                    }
                }
            }
            else
            {
                encomenda?.UpdateOrdersList();
            }
        }

        internal void UpdateAllLists()
        {
            var fragments = _fm.Fragments;
            foreach (Fragment fragment in fragments)
            {
                if (fragment is HomePageOrdersFragment homePageOrdersFragment)
                {
                    int position = TabsOptions.FindIndex(tab => tab.Name.Equals("Encomendas"));
                    homePageOrdersFragment.OrdersPage = (OrdersPage)TabsOptions[position];
                    homePageOrdersFragment.UpdateOrdersList(true);
                }
                if (fragment is HomeNotificationsFragment homeNotificationsFragment)
                {
                    int position = TabsOptions.FindIndex(tab => tab is NotificationsPage);
                    homeNotificationsFragment.NotificationData = TabsOptions[position] as NotificationsPage;
                    homeNotificationsFragment.UpdateNotificationsList(true);
                }
            }
        }
    }
}