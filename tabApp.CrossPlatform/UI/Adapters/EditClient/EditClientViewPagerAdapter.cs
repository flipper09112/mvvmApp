using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using tabApp.Core.Models;
using tabApp.Core.ViewModels;
using tabApp.UI.Fragments.EditClient;

namespace tabApp.UI.Adapters.EditClient
{
    public class EditClientViewPagerAdapter : FragmentStatePagerAdapter
    {
        public EditClientViewPagerAdapter(Android.Support.V4.App.FragmentManager fm) : base(fm)
        {
        }

        public List<string> Title { get; internal set; }
        public EditClientViewModel ViewModel { get; internal set; }

        public override int Count => 4;

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            if(position == 0)
            {
                return new EditClientProfileFragment(ViewModel, ViewModel.ProfileItems);

            } else if (position == 1)
            {
                return new EditClientMapFragment(ViewModel);
            }
            else if (position == 2)
            {
                return new EditClientProfileFragment(ViewModel, ViewModel.AdminItems);
            }
            else
            {
                return new EditDailyOrdersFragment(ViewModel);
            }
            return null;
        }

        public override void DestroyItem(View container, int position, Java.Lang.Object @object)
        {
            base.DestroyItem(container, position, @object);
        }
    }
}