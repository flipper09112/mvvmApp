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
using tabApp.Core.ViewModels;
using tabApp.UI.Adapters;

namespace tabApp.UI.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class DailysOrdersDescFragment : BaseFragment<DailysOrdersDescViewModel>
    {
        private MainActivity _activity;
        private RecyclerView _segRecyclerView;
        private RecyclerView _terRecyclerView;
        private RecyclerView _quaRecyclerView;
        private RecyclerView _quiRecyclerView;
        private RecyclerView _sexRecyclerView;
        private RecyclerView _sabRecyclerView;
        private RecyclerView _domRecyclerView;
        private DailyOrderDescAdapter _segAdapter;
        private DailyOrderDescAdapter _terAdapter;
        private DailyOrderDescAdapter _quaAdapter;
        private DailyOrderDescAdapter _sexAdapter;
        private RecyclerView.Adapter _sabAdapter;
        private DailyOrderDescAdapter _domAdapter;
        private DailyOrderDescAdapter _quiAdapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.DailysOrdersDescFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _segRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.segRecyclerView);
            _terRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.terRecyclerView);
            _quaRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.quaRecyclerView);
            _quiRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.quiRecyclerView);
            _sexRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.sexRecyclerView);
            _sabRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.sabRecyclerView);
            _domRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.domRecyclerView);

            _segRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            _terRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            _quaRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            _quiRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            _sexRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            _sabRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            _domRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));

            return view;
        }

        public override void CleanBindings()
        {
        }

        #region UI
        public override void SetUI()
        {
            _activity.HideToolbar();

            SetupSegList();
            SetupTerList();
            SetupQuaList();
            SetupQuiList();
            SetupSexList();
            SetupSabList();
            SetupDomList();
        }

        private void SetupSegList()
        {
            _segAdapter = new DailyOrderDescAdapter(ViewModel.SegDailyItemsList);
            _segRecyclerView.SetAdapter(_segAdapter);
        }
        private void SetupTerList()
        {
            _terAdapter = new DailyOrderDescAdapter(ViewModel.TerDailyItemsList);
            _terRecyclerView.SetAdapter(_terAdapter);
        }
        private void SetupQuaList()
        {
            _quaAdapter = new DailyOrderDescAdapter(ViewModel.QuaDailyItemsList);
            _quaRecyclerView.SetAdapter(_quaAdapter);
        }
        private void SetupQuiList()
        {
            _quiAdapter = new DailyOrderDescAdapter(ViewModel.QuiDailyItemsList);
            _quiRecyclerView.SetAdapter(_quiAdapter);
        }
        private void SetupSexList()
        {
            _sexAdapter = new DailyOrderDescAdapter(ViewModel.SexDailyItemsList);
            _sexRecyclerView.SetAdapter(_sexAdapter);
        }
        private void SetupSabList()
        {
            _sabAdapter = new DailyOrderDescAdapter(ViewModel.SabDailyItemsList);
            _sabRecyclerView.SetAdapter(_sabAdapter);
        }
        private void SetupDomList()
        {
            _domAdapter = new DailyOrderDescAdapter(ViewModel.DomDailyItemsList);
            _domRecyclerView.SetAdapter(_domAdapter);
        }
        #endregion

        public override void SetupBindings()
        {
        }
    }
}