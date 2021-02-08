using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.ClientPage.OtherOptions;
using tabApp.UI.Adapters;

namespace tabApp.UI.Fragments.EditClient
{
    public class EditDailyOrdersFragment : Android.Support.V4.App.Fragment
    {
        private EditClientViewModel viewModel;
        private ChangeDailyOrderViewModel viewModel1;

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
        private DailyOrderDescAdapter _sabAdapter;
        private DailyOrderDescAdapter _domAdapter;
        private DailyOrderDescAdapter _quiAdapter;
        private Button _addProductSeg;
        private Button _addProductTer;
        private Button _addProductQua;
        private Button _addProductQui;
        private Button _addProductSex;
        private Button _addProductSab;
        private Button _addProductDom;
        private Button _duplicateDaysButton;

        public EditDailyOrdersFragment(EditClientViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public EditDailyOrdersFragment(ChangeDailyOrderViewModel viewModel1)
        {
            this.viewModel1 = viewModel1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.DailysOrdersDescEditFragment, container, false);

            _segRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.segRecyclerView);
            _terRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.terRecyclerView);
            _quaRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.quaRecyclerView);
            _quiRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.quiRecyclerView);
            _sexRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.sexRecyclerView);
            _sabRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.sabRecyclerView);
            _domRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.domRecyclerView);

            _addProductSeg = view.FindViewById<Button>(Resource.Id.addProductSeg);
            _addProductTer = view.FindViewById<Button>(Resource.Id.addProductTer);
            _addProductQua = view.FindViewById<Button>(Resource.Id.addProductQua);
            _addProductQui = view.FindViewById<Button>(Resource.Id.addProductQui);
            _addProductSex = view.FindViewById<Button>(Resource.Id.addProductSex);
            _addProductSab = view.FindViewById<Button>(Resource.Id.addProductSab);
            _addProductDom = view.FindViewById<Button>(Resource.Id.addProductDom);

            _duplicateDaysButton = view.FindViewById<Button>(Resource.Id.duplicateDaysButton);

            _segRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            _terRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            _quaRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            _quiRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            _sexRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            _sabRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            _domRecyclerView.SetLayoutManager(new LinearLayoutManager(Context));

            return view;
        }

        public override void OnResume()
        {
            SetUi();
            SetupBindings();
            base.OnResume();
        }
        public override void OnPause()
        {
            CleaningBindings();
            base.OnPause();
        }

        private void CleaningBindings()
        {
            _addProductSeg.Click -= AddProductSegClick;
            _addProductTer.Click -= AddProductTerClick;
            _addProductQua.Click -= AddProductQuaClick;
            _addProductQui.Click -= AddProductQuiClick;
            _addProductSex.Click -= AddProductSexClick;
            _addProductSab.Click -= AddProductSabClick;
            _addProductDom.Click -= AddProductDomClick;

            _duplicateDaysButton.Click -= DuplicateDaysButtonClick;

            if(viewModel != null)
                viewModel.PropertyChanged -= ViewModelPropertyChanged;

            if (viewModel1 != null)
                viewModel1.PropertyChanged -= ViewModelPropertyChanged;
        }

        private void SetupBindings()
        {
            _addProductSeg.Click += AddProductSegClick;
            _addProductTer.Click += AddProductTerClick;
            _addProductQua.Click += AddProductQuaClick;
            _addProductQui.Click += AddProductQuiClick;
            _addProductSex.Click += AddProductSexClick;
            _addProductSab.Click += AddProductSabClick;
            _addProductDom.Click += AddProductDomClick;

            _duplicateDaysButton.Click += DuplicateDaysButtonClick;

            if (viewModel != null)
                viewModel.PropertyChanged += ViewModelPropertyChanged;
            if (viewModel1 != null)
                viewModel1.PropertyChanged += ViewModelPropertyChanged;
        }

        private void DuplicateDaysButtonClick(object sender, EventArgs e)
        {
            viewModel?.ShowSelectDaysPageCommand.Execute(null);
            viewModel1?.ShowSelectDaysPageCommand.Execute(null);
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(viewModel.SegDailyItemsList):
                    SetupSegList();
                    break;
                case nameof(viewModel.TerDailyItemsList):
                    SetupTerList();
                    break;
                case nameof(viewModel.QuaDailyItemsList):
                    SetupQuaList();
                    break;
                case nameof(viewModel.QuiDailyItemsList):
                    SetupQuiList();
                    break;
                case nameof(viewModel.SexDailyItemsList):
                    SetupSexList();
                    break;
                case nameof(viewModel.SabDailyItemsList):
                    SetupSabList();
                    break;
                case nameof(viewModel.DomDailyItemsList):
                    SetupDomList();
                    break;
            }
        }
        private void AddProductDomClick(object sender, EventArgs e)
        {
            viewModel?.AddProductCommand.Execute(DayOfWeek.Sunday);
            viewModel1?.AddProductCommand.Execute(DayOfWeek.Sunday);
        }

        private void AddProductSabClick(object sender, EventArgs e)
        {
            viewModel?.AddProductCommand.Execute(DayOfWeek.Saturday);
            viewModel1?.AddProductCommand.Execute(DayOfWeek.Saturday);
        }

        private void AddProductSexClick(object sender, EventArgs e)
        {
            viewModel?.AddProductCommand.Execute(DayOfWeek.Friday);
            viewModel1?.AddProductCommand.Execute(DayOfWeek.Friday);
        }

        private void AddProductQuiClick(object sender, EventArgs e)
        {
            viewModel?.AddProductCommand.Execute(DayOfWeek.Thursday);
            viewModel1?.AddProductCommand.Execute(DayOfWeek.Thursday);
        }

        private void AddProductQuaClick(object sender, EventArgs e)
        {
            viewModel?.AddProductCommand.Execute(DayOfWeek.Wednesday);
            viewModel1?.AddProductCommand.Execute(DayOfWeek.Wednesday);
        }

        private void AddProductTerClick(object sender, EventArgs e)
        {
            viewModel?.AddProductCommand.Execute(DayOfWeek.Tuesday);
            viewModel1?.AddProductCommand.Execute(DayOfWeek.Tuesday);
        }

        private void AddProductSegClick(object sender, EventArgs e)
        {
            viewModel?.AddProductCommand.Execute(DayOfWeek.Monday);
            viewModel1?.AddProductCommand.Execute(DayOfWeek.Monday);
        }

        private void SetUi()
        {
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
            if(viewModel != null)
                _segAdapter = new DailyOrderDescAdapter(viewModel.SegDailyItemsList);
            if (viewModel1 != null)
                _segAdapter = new DailyOrderDescAdapter(viewModel1.SegDailyItemsList);
            _segRecyclerView.SetAdapter(_segAdapter);
        }
        private void SetupTerList()
        {
            if (viewModel != null)
                _terAdapter = new DailyOrderDescAdapter(viewModel.TerDailyItemsList);
            if (viewModel1 != null)
                _terAdapter = new DailyOrderDescAdapter(viewModel1.TerDailyItemsList);
            _terRecyclerView.SetAdapter(_terAdapter);
        }
        private void SetupQuaList()
        {
            if (viewModel != null)
                _quaAdapter = new DailyOrderDescAdapter(viewModel.QuaDailyItemsList);
            if (viewModel1 != null)
                _quaAdapter = new DailyOrderDescAdapter(viewModel1.QuaDailyItemsList);
            _quaRecyclerView.SetAdapter(_quaAdapter);
        }
        private void SetupQuiList()
        {
            if (viewModel != null)
                _quiAdapter = new DailyOrderDescAdapter(viewModel.QuiDailyItemsList);
            if (viewModel1 != null)
                _quiAdapter = new DailyOrderDescAdapter(viewModel1.QuiDailyItemsList);
            _quiRecyclerView.SetAdapter(_quiAdapter);
        }
        private void SetupSexList()
        {
            if (viewModel != null)
                _sexAdapter = new DailyOrderDescAdapter(viewModel.SexDailyItemsList);
            if (viewModel1 != null)
                _sexAdapter = new DailyOrderDescAdapter(viewModel1.SexDailyItemsList);
            _sexRecyclerView.SetAdapter(_sexAdapter);
        }
        private void SetupSabList()
        {
            if (viewModel != null)
                _sabAdapter = new DailyOrderDescAdapter(viewModel.SabDailyItemsList);
            if (viewModel1 != null)
                _sabAdapter = new DailyOrderDescAdapter(viewModel1.SabDailyItemsList);
            _sabRecyclerView.SetAdapter(_sabAdapter);
        }
        private void SetupDomList()
        {
            if (viewModel != null)
                _domAdapter = new DailyOrderDescAdapter(viewModel.DomDailyItemsList);
            if (viewModel1 != null)
                _domAdapter = new DailyOrderDescAdapter(viewModel1.DomDailyItemsList);
            _domRecyclerView.SetAdapter(_domAdapter);
        }
    }
}