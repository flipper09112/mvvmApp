using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.UI;

namespace tabApp.Helpers
{
    public static class FragmentHelper
    {
        public static void ShowNewFragment(Fragment origin, Fragment dest)
        {
            FragmentTransaction fragmentTransaction = origin.FragmentManager.BeginTransaction();

            // replace the FrameLayout with new Fragment
            fragmentTransaction.Replace(Resource.Id.fragmentContainer, dest);
            fragmentTransaction.AddToBackStack(dest.Class.Name);
            fragmentTransaction.Commit(); // save the changes
        }

        /*public static void ShowFirstFragment(FragmentManager fragmentManager, MvxFragment dest)
        {
            FragmentTransaction fragmentTransaction = fragmentManager.BeginTransaction();

            // replace the FrameLayout with new Fragment
            fragmentTransaction.Replace(Resource.Id.fragmentContainer, dest);
            fragmentTransaction.AddToBackStack(dest.Class.Name);
            fragmentTransaction.Commit(); // save the changes
        }*/
    }
}