using Android.App;
using Android.OS; 
using Android.Support.V7.Widget;
using Android.Views;
using System;
using tabApp.Core.ViewModels;

namespace tabApp.UI.Adapters.EditClient
{
    internal class EditClientProfileFragment : Android.Support.V4.App.Fragment
    {
        private EditClientViewModel viewModel;
        private IntPtr context;

        public EditClientProfileFragment(EditClientViewModel viewModel)
        {
            this.viewModel = viewModel;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.RecyclerViewLayout, container, false);

            var adapter = new EditClientProfileItemsAdapter(viewModel.ProfileItems);
            var recycler = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            var layoutManager = new LinearLayoutManager(Context);
            recycler.SetLayoutManager(layoutManager);
            recycler.SetAdapter(adapter);

            return view;
        }
        /*public override void CleanBindings()
        {
        }

        public override void SetUI()
        {
        }

        public override void SetupBindings()
        {
        }*/
    }
}