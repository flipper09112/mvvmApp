using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Global.PriceTable;
using tabApp.UI.Fragments.Bases;

namespace tabApp.UI.Fragments.Global.PriceTable
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class PriceTableConfigurationFragment : BaseOptionsListFragment<PriceTableConfigurationViewModel>
    {        
    }
}