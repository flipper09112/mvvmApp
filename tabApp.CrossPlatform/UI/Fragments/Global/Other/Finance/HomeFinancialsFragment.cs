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
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Global.Other;
using tabApp.UI.Adapters;
using tabApp.UI.Adapters.Global;

namespace tabApp.UI.Fragments.Global.Other
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class HomeFinancialsFragment : BaseFragment<HomeFinancialsViewModel>
    {
        private MainActivity _activity;
        private RecyclerView _productsRv;
        private TextView _outValue;
        private TextView _inValue;
        private TextView _saldoValue;
        private EditText _editTextDate;
        private Button _editButton;
        private Button _addProductButton;
        private Button _statsButton;
        private Button _weekSumButton;
        private HomeFinancialsProductsAdapter _mainAdapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _activity = ParentActivity as MainActivity;

            View view = inflater.Inflate(Resource.Layout.HomeFinancialsFragment, container, false);

            _productsRv = view.FindViewById<RecyclerView>(Resource.Id.productsRv);
            _outValue = view.FindViewById<TextView>(Resource.Id.outValue); 
            _inValue = view.FindViewById<TextView>(Resource.Id.inValue);
            _saldoValue = view.FindViewById<TextView>(Resource.Id.saldoValue);
            _editTextDate = view.FindViewById<EditText>(Resource.Id.editTextDate);
            _editButton = view.FindViewById<Button>(Resource.Id.editButton);
            _addProductButton = view.FindViewById<Button>(Resource.Id.addProductButton);
            _statsButton = view.FindViewById<Button>(Resource.Id.statsButton);
            _weekSumButton = view.FindViewById<Button>(Resource.Id.weekSumButton); 

            _productsRv.SetLayoutManager(new LinearLayoutManager(Context)); 


            return view;
        }


        public override void SetUI()
        {
            if(_mainAdapter == null)
                SetList();
            _outValue.Text = ViewModel.OutValue;
            _inValue.Text = ViewModel.InValue;
            _saldoValue.Text = ViewModel.SaldoValue;
            _editTextDate.Text = ViewModel.DateSelected.ToString("dd/MM/yyyy");
            _editTextDate.Focusable = false;
            _editTextDate.Clickable = true;
            _editButton.Text = !ViewModel.EditableList ? "Editar" : "Salvar";
            _addProductButton.Text = "Adicionar Produto";
            _statsButton.Text = "Estatisticas";
            _weekSumButton.Text = "Somar Dias";
        }

        private void SetList()
        {
            _mainAdapter = new HomeFinancialsProductsAdapter(ViewModel.ProductsList, ViewModel);
            _productsRv.SetAdapter(_mainAdapter);
        }

        public override void SetupBindings()
        {
            _editTextDate.Click += EditTextDateClick;
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            _editButton.Click += EditButtonClick;
            _outValue.Click += OutValueClick;
            _addProductButton.Click += AddProductButtonClick;
            _statsButton.Click += StatsButtonClick;
            _weekSumButton.Click += WeekSumButtonClick;     
        }

        public override void CleanBindings()
        {
            _editTextDate.Click -= EditTextDateClick;
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            _editButton.Click -= EditButtonClick;
            _outValue.Click -= OutValueClick;
            _addProductButton.Click -= AddProductButtonClick;
            _statsButton.Click -= StatsButtonClick;
            _weekSumButton.Click -= WeekSumButtonClick;

            _mainAdapter = null;
        }
        private void WeekSumButtonClick(object sender, EventArgs e)
        {
            ViewModel.WeekSumPageCommnand.Execute();
        }

        private void StatsButtonClick(object sender, EventArgs e)
        {
            ViewModel.StatsPageCommand.Execute();
        }

        private void AddProductButtonClick(object sender, EventArgs e)
        {
            ViewModel.AddProductCommand.Execute();
        }

        private void OutValueClick(object sender, EventArgs e)
        {
            ViewModel.ShowOutValueDescCommand.Execute();
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.DateSelected))
                SetList();
            SetUI();
        }

        private void EditButtonClick(object sender, EventArgs e)
        {
            if (ViewModel.EditableList)
                ViewModel.SaveChangesListCommand.Execute();

            ViewModel.EditableList = !ViewModel.EditableList;

            SetUI();
            _mainAdapter?.NotifyDataSetChanged();
        }

        private void EditTextDateClick(object sender, EventArgs e)
        {
            ViewModel.SelectDateCommand.Execute(null);
        }
    }
}