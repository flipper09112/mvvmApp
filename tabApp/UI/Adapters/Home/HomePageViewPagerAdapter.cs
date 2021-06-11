

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
        private HomePageOrdersFragment encomenda;

        public override int Count => TabsOptions?.Count ?? 0;
        public HomeViewModel ViewModel { get; internal set; }

        public HomePageViewPagerAdapter(Android.Support.V4.App.FragmentManager fm) : base(fm)
        {
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
        public override void DestroyItem(View container, int position, Java.Lang.Object @object)
        {
            base.DestroyItem(container, position, @object);
        }

        internal void UpdateOrdersList()
        {
            encomenda.UpdateOrdersList();
        }
    }
}