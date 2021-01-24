using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
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

namespace tabApp.UI
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class HomeFragment : BaseFragment<HomeViewModel>
    {
        private RecyclerView _clientsList;
        private RecyclerView _ordersList;
        private ClientsListAdapter _clientsAdapter;


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.HomeFragment, container, false);

            _clientsList = view.FindViewById<RecyclerView>(Resource.Id.clientsList);
            _ordersList = view.FindViewById<RecyclerView>(Resource.Id.ordersList);

            _clientsList.SetLayoutManager(new LinearLayoutManager(Context));

            return view;
        }

        public override void SetUI()
        {
            _clientsAdapter = new ClientsListAdapter(ViewModel.ClientsList, ViewModel.ShowClientPage); 
            _clientsList.SetAdapter(_clientsAdapter);
        }

        public override void SetupBindings()
        {
        }
        public override void CleanBindings()
        {
        }
    }
}