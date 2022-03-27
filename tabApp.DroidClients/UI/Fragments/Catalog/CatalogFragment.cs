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
    public class CatalogFragment : BaseFragment<CatalogViewModel>
    {
        private MainActivity _activity;
        private RecyclerView _recyclerView;
        private CatalogAdapter _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.CatalogFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

            _recyclerView.HasFixedSize = true;
            _recyclerView.SetItemViewCacheSize(20);
            _recyclerView.DrawingCacheEnabled = true;

            _recyclerView.SetLayoutManager(new GridLayoutManager(Context, 3, LinearLayoutManager.Horizontal, false));

            return view;
        }

        public override void SetUI()
        {
            SetRV();
        }

        private void SetRV()
        {
            if (_adapter != null)
            {
                ViewModelPropertyChanged(null, null);
                return;
            }

            _adapter = new CatalogAdapter(ViewModel.CatalogTypeItemsList);
            _recyclerView.SetAdapter(_adapter);
        }

        public override void SetupBindings()
        {
            _activity.BackPress += OnBackPress;
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
        }

        public override void CleanBindings()
        {
            _activity.BackPress -= OnBackPress;
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
        }

        private void OnBackPress(object sender, EventArgs e)
        {
            if(ViewModel.Position != 0)
            {
                ViewModel.GoBackMenuCommand.Execute(null);
            }
            else
            {
                _activity.SupportFragmentManager.PopBackStack();
            }
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(ViewModel.Position == 0)
            {
                _recyclerView.SetLayoutManager(new GridLayoutManager(Context, ViewModel.CatalogTypeItemsList.Count, LinearLayoutManager.Horizontal, false));
            }
            else if(ViewModel.Position == 1)
            {
                _recyclerView.SetLayoutManager(new GridLayoutManager(Context, ViewModel.CatalogTypeItemsList.Count, LinearLayoutManager.Horizontal, false));
            }

            _adapter = null;
            SetUI();
        }
    }
}