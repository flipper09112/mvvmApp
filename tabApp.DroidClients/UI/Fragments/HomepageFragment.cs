using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModelsClient;
using tabApp.DroidClients.UI.Adapters;
using tabApp.UI;

namespace tabApp.DroidClients.UI.Fragments
{
    [MvxFragmentPresentation(typeof(HomePageViewModel), Resource.Id.fragmentContainer, true)]
    public class HomepageFragment : BaseFragment<HomepageViewModelClient>
    {
        private MainActivity _activity;
        private RecyclerView _homeRv;
        private RecyclerView.Adapter _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.HomepageFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _homeRv = view.FindViewById<RecyclerView>(Resource.Id.homeRv);
            _homeRv.SetLayoutManager(new LinearLayoutManager(Context));

            return view;
        }

        public override void SetUI()
        {
            _activity.HideToolbar();
            SetRv();
        }

        private void SetRv()
        {
            _adapter = new HomepageAdapter(ViewModel.HomepageItems);
            _homeRv.SetAdapter(_adapter);
        }

        public override void SetupBindings()
        {
        }

        public override void CleanBindings()
        {
        }
    }
}