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
using tabApp.Core.ViewModelsClient.Catalog;
using tabApp.DroidClients.UI.Adapters;
using tabApp.UI;

namespace tabApp.DroidClients.UI.Fragments.Catalog
{
    [MvxFragmentPresentation(typeof(HomePageViewModel), Resource.Id.fragmentContainer, true)]
    public class ProductsListFragment : BaseFragment<ProductsListViewModel>
    {
        private View _view;
        private MainActivity _activity;
        private RecyclerView _recyclerView;
        private ProdutsListAdapter _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (_view != null) return _view;

            _view = inflater.Inflate(Resource.Layout.ProductsListFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _recyclerView = _view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            _recyclerView.HasFixedSize = true;
            _recyclerView.SetItemViewCacheSize(20);
            _recyclerView.DrawingCacheEnabled = true;
     
            return _view;
        }

        public override void SetUI()
        {
        }

        public override void SetupBindings()
        {
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
        }

        public override void CleanBindings()
        {
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _recyclerView.SetLayoutManager(new GridLayoutManager(Context, 2, LinearLayoutManager.Vertical, false));

            _adapter = new ProdutsListAdapter(ViewModel.ProductsList, ViewModel.ShowSelectedProductCommand);
            _recyclerView.SetAdapter(_adapter);
        }
    }
}