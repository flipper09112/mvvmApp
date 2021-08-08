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

            _productsRv.SetLayoutManager(new LinearLayoutManager(Context));

            return view;
        }


        public override void SetUI()
        {
            SetList();
            _outValue.Text = ViewModel.OutValue;
            _inValue.Text = ViewModel.InValue;
            _saldoValue.Text = ViewModel.SaldoValue;
            _editTextDate.Text = ViewModel.DateSelected.ToString("dd/MM/yyyy");
            _editTextDate.Focusable = false;
            _editTextDate.Clickable = true;
        }

        private void SetList()
        {
            _mainAdapter = new HomeFinancialsProductsAdapter(ViewModel.ProductsList);
            _productsRv.SetAdapter(_mainAdapter);
        }

        public override void SetupBindings()
        {
            _editTextDate.Click += EditTextDateClick;
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
        }

        public override void CleanBindings()
        {
            _editTextDate.Click -= EditTextDateClick;
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
        }
        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.DateSelected))
                SetList();
            SetUI();
        }

        private void EditTextDateClick(object sender, EventArgs e)
        {
            ViewModel.SelectDateCommand.Execute(null);
        }
    }
}