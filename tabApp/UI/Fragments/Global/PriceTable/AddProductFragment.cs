using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Global.PriceTable;
using tabApp.Helpers;
using tabApp.UI.Adapters.Global;

namespace tabApp.UI.Fragments.Global.PriceTable
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class AddProductFragment : BaseFragment<AddProductViewModel>
    {
        private MainActivity _activity;
        private EditText _productName;
        private EditText _reference;
        private RadioButton _unityRadio;
        private RadioButton _kgRadio;
        private RecyclerView _rvProductsType;
        private EditText _price;
        private EditText _priceLow;
        private Button _addProductBt;
        private ProductsTypesListAdapter _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.AddProductFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _productName = view.FindViewById<EditText>(Resource.Id.productName);
            _reference = view.FindViewById<EditText>(Resource.Id.reference);
            _unityRadio = view.FindViewById<RadioButton>(Resource.Id.unityRadio);
            _kgRadio = view.FindViewById<RadioButton>(Resource.Id.kgRadio);
            _rvProductsType = view.FindViewById<RecyclerView>(Resource.Id.rvProductsType);
            _price = view.FindViewById<EditText>(Resource.Id.price);
            _priceLow = view.FindViewById<EditText>(Resource.Id.priceLow);
            _addProductBt = view.FindViewById<Button>(Resource.Id.addProductBt);

            _rvProductsType.SetLayoutManager(new LinearLayoutManager(Context));
            _adapter = new ProductsTypesListAdapter(ViewModel.ProductsTypesList, ViewModel);
            _adapter.Click = SelectProductType;
            _rvProductsType.SetAdapter(_adapter);

            return view;
        }

        public override void SetUI()
        {
            _price.SetFilters(new IInputFilter[] { new DecimalDigitsInputFilter(2) });
            _price.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal;

            _priceLow.SetFilters(new IInputFilter[] { new DecimalDigitsInputFilter(2) });
            _priceLow.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal;

            AddProductCommandCanExecuteChanged(null, null);
        }

        public override void CleanBindings()
        {
            _productName.TextChanged -= ProductNameTextChanged;
            _reference.TextChanged -= ReferenceTextChanged;
            _unityRadio.CheckedChange -= UnityRadioCheckedChange;
            _price.TextChanged -= PriceTextChanged;
            _priceLow.TextChanged -= PriceLowTextChanged;
            _kgRadio.CheckedChange -= kgRadioCheckedChange;
            _addProductBt.Click -= AddProductBtClick;
            ViewModel.AddProductCommand.CanExecuteChanged -= AddProductCommandCanExecuteChanged;
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            ViewModel.GoBack -= GoBack;
        }

        public override void SetupBindings()
        {
            _productName.TextChanged += ProductNameTextChanged;
            _reference.TextChanged += ReferenceTextChanged;
            _price.TextChanged += PriceTextChanged;
            _priceLow.TextChanged += PriceLowTextChanged;
            _unityRadio.CheckedChange += UnityRadioCheckedChange;
            _kgRadio.CheckedChange += kgRadioCheckedChange;
            _addProductBt.Click += AddProductBtClick;
            ViewModel.AddProductCommand.CanExecuteChanged += AddProductCommandCanExecuteChanged;
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            ViewModel.GoBack += GoBack;
        }

        private void GoBack(object sender, EventArgs e)
        {
            _activity.SupportFragmentManager.PopBackStack();
            _activity.SupportFragmentManager.PopBackStack();
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(ViewModel.ProductTypeSelected))
            {
                _adapter.NotifyDataSetChanged();
            }
        }

        private void SelectProductType(ProductTypeEnum obj)
        {
            ViewModel.ProductTypeSelected = obj;
        }

        private void AddProductCommandCanExecuteChanged(object sender, EventArgs e)
        {
            if(ViewModel.AddProductCommand.CanExecute(null))
            {
                _addProductBt.Enabled = true;
            }
            else
            {
                _addProductBt.Enabled = false;
            }
        }

        private void AddProductBtClick(object sender, EventArgs e)
        {
            ViewModel.AddProductCommand.Execute(null);
        }

        private void PriceLowTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ViewModel.PriceStores = double.Parse(_priceLow.Text.Replace(".", ","));

            } catch (Exception e2)
            {
                ViewModel.PriceStores = 0;
            }
        }

        private void PriceTextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            try
            {
                ViewModel.Price = double.Parse(_price.Text.Replace(".", ","));

            }
            catch (Exception e2)
            {
                ViewModel.Price = 0;
            }
        }

        private void kgRadioCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            ViewModel.Unity = e.IsChecked;
        }

        private void UnityRadioCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            
            ViewModel.Unity = e.IsChecked;
        }

        private void ReferenceTextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            ViewModel.Reference = _reference.Text;
        }

        private void ProductNameTextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            ViewModel.ProductName = _productName.Text;
        }
    }
}