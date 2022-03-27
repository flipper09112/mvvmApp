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
using tabApp.Core.ViewModelsClient;
using tabApp.Core.ViewModelsClient.Catalog;
using tabApp.UI;

namespace tabApp.DroidClients.UI.Fragments.Catalog
{
    [MvxFragmentPresentation(typeof(HomePageViewModel), Resource.Id.fragmentContainer, true)]
    public class ProductsListFragment : BaseFragment<ProductsListViewModel>
    {
        private MainActivity _activity;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.ProductsListFragment, container, false);

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