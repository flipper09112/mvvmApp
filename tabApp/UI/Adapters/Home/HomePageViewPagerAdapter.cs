

using Android.Support.V4.App;
using Android.Views;
using System.Collections.Generic;
using tabApp.Core;
using tabApp.UI.Fragments.Home.ViewPager;

namespace tabApp.UI.Adapters.Home
{
    public class HomePageViewPagerAdapter : FragmentStatePagerAdapter
    {
        public List<SecondaryOptions> TabsOptions;

        public override int Count => TabsOptions?.Count ?? 0;
        public HomeViewModel ViewModel { get; internal set; }

        public HomePageViewPagerAdapter(Android.Support.V4.App.FragmentManager fm) : base(fm)
        {
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            if (TabsOptions[position].Name.Equals("Encomendas"))
            {
                return new HomePageOrdersFragment((OrdersPage)TabsOptions[position], ViewModel);
            }
            else if (TabsOptions[position].Name.Equals("Localização"))
            {
                return new HomePageMapFragment(ViewModel);
            }
            return null;
        }
        public override void DestroyItem(View container, int position, Java.Lang.Object @object)
        {
            base.DestroyItem(container, position, @object);
        }
    }
}