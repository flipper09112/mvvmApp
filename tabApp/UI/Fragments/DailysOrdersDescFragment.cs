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
        private LinearLayout _segLayout;
        private LinearLayout _terLayout;
        private LinearLayout _quaLayout;
        private LinearLayout _quiLayout;
        private LinearLayout _sexLayout;
        private LinearLayout _sabLayout;
        private LinearLayout _domLayout;
        private LinearLayout _mainLayout;
        private HorizontalScrollView _horizontalScrollView;
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

            _segLayout = view.FindViewById<LinearLayout>(Resource.Id.segLayout);
            _terLayout = view.FindViewById<LinearLayout>(Resource.Id.terLayout);
            _quaLayout = view.FindViewById<LinearLayout>(Resource.Id.quaLayout);
            _quiLayout = view.FindViewById<LinearLayout>(Resource.Id.quiLayout);
            _sexLayout = view.FindViewById<LinearLayout>(Resource.Id.sexLayout);
            _sabLayout = view.FindViewById<LinearLayout>(Resource.Id.sabLayout);
            _domLayout = view.FindViewById<LinearLayout>(Resource.Id.domLayout);

            _mainLayout = view.FindViewById<LinearLayout>(Resource.Id.mainLayout);
            _horizontalScrollView = view.FindViewById<HorizontalScrollView>(Resource.Id.horizontalScrollView);

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

            SetContainerLayout();
            _horizontalScrollView.FullScroll(FocusSearchDirection.Right);
        }

        private void SetContainerLayout()
        {
            _segLayout.SetBackgroundResource(DateTime.Today.DayOfWeek == DayOfWeek.Monday ? Resource.Drawable.ContainerRetangle : Resource.Color.transparent);
            _terLayout.SetBackgroundResource(DateTime.Today.DayOfWeek == DayOfWeek.Tuesday ? Resource.Drawable.ContainerRetangle : Resource.Color.transparent);
            _quaLayout.SetBackgroundResource(DateTime.Today.DayOfWeek == DayOfWeek.Wednesday ? Resource.Drawable.ContainerRetangle : Resource.Color.transparent);
            _quiLayout.SetBackgroundResource(DateTime.Today.DayOfWeek == DayOfWeek.Thursday ? Resource.Drawable.ContainerRetangle : Resource.Color.transparent);
            _sexLayout.SetBackgroundResource(DateTime.Today.DayOfWeek == DayOfWeek.Friday ? Resource.Drawable.ContainerRetangle : Resource.Color.transparent);
            _sabLayout.SetBackgroundResource(DateTime.Today.DayOfWeek == DayOfWeek.Saturday ? Resource.Drawable.ContainerRetangle : Resource.Color.transparent);
            _domLayout.SetBackgroundResource(DateTime.Today.DayOfWeek == DayOfWeek.Sunday ? Resource.Drawable.ContainerRetangle : Resource.Color.transparent);
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