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
using System.Diagnostics;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Global.Faturation;
using tabApp.UI.Adapters.Faturation.Spinners;
using tabApp.UI.Adapters.Faturation;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;
using Android.Media.TV;
using tabApp.UI.Adapters.Global;

namespace tabApp.UI.Fragments.Global.Faturation
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class FaturationFragment : BaseFragment<FaturationViewModel>
    {
        private View _view;
        private RecyclerView _faturasRv;
        private RecyclerView _productsListRv;
        private RecyclerView _remainingItemsRv;
        private View _emptyLayout;
        private Spinner _guiasSpinner;
        private ImageView _openGuiaIcon;
        private Button _addProductBt, _createFaturaBt;
        private TextView _clientName;
        private TextView _totalFatValue;
        private RadioGroup _fatRadioGroup;
        private RadioButton _fatRg;
        private ProductsListFatAdapter _productsListAdapter;
        private LastTrasnportationsDocsAdapter _lastTrasnportationsDocsAdapter;
        private LastTrasnportationsDocsSpinnerAdapter _guiasSpinnerAdapter;
        private ProductsListFatAdapter _productsRemainingListAdapter;
        private RadioButton _fatReceiptRg;
        private LinearLayout _actionTemplateContainer;
        private LinearLayout _templateContainer;
        private Button _templateOneBt;
        private Button _templateTwoBt;
        private Button _templateThreeBt;
        private Button _actionTemplateOneBt;
        private Button _actionTemplateTwoBt;
        private Button _actionTemplateThreeBt;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (_view != null) return _view;
            View view = inflater.Inflate(Resource.Layout.FaturationFragment, container, false);

            _view = view;
            _faturasRv = view.FindViewById<RecyclerView>(Resource.Id.faturasRv);
            _productsListRv = view.FindViewById<RecyclerView>(Resource.Id.productsListRv);
            _remainingItemsRv = view.FindViewById<RecyclerView>(Resource.Id.remainingItemsRv);
            _emptyLayout = view.FindViewById(Resource.Id.emptyLayout);
            _guiasSpinner = view.FindViewById<Spinner>(Resource.Id.guiasRv);
            _openGuiaIcon = view.FindViewById<ImageView>(Resource.Id.openGuiaIcon);
            _addProductBt = view.FindViewById<Button>(Resource.Id.addProductBt);
            _createFaturaBt = view.FindViewById<Button>(Resource.Id.createTrasnportationDoc);
            _clientName = view.FindViewById<TextView>(Resource.Id.clientName);
            _totalFatValue = view.FindViewById<TextView>(Resource.Id.totalFatValue);
            _fatRadioGroup = view.FindViewById<RadioGroup>(Resource.Id.fatRadioGroup);
            _fatRg = view.FindViewById<RadioButton>(Resource.Id.fatRg);
            _fatReceiptRg = view.FindViewById<RadioButton>(Resource.Id.fatReceiptRg);
            _actionTemplateContainer = view.FindViewById<LinearLayout>(Resource.Id.actionTemplateContainer);
            _templateContainer = view.FindViewById<LinearLayout>(Resource.Id.templateContainer);
            _templateOneBt = view.FindViewById<Button>(Resource.Id.templateOneBt);
            _templateTwoBt = view.FindViewById<Button>(Resource.Id.templateTwoBt);
            _templateThreeBt = view.FindViewById<Button>(Resource.Id.templateThreeBt);
            _actionTemplateOneBt = view.FindViewById<Button>(Resource.Id.actionTemplateOneBt);
            _actionTemplateTwoBt = view.FindViewById<Button>(Resource.Id.actionTemplateTwoBt);
            _actionTemplateThreeBt = view.FindViewById<Button>(Resource.Id.actionTemplateThreeBt);

            _faturasRv.SetLayoutManager(new LinearLayoutManager(Context));
            _productsListRv.SetLayoutManager(new LinearLayoutManager(Context));
            _remainingItemsRv.SetLayoutManager(new LinearLayoutManager(Context));

            return view;
        }

        public override void SetUI()
        {
            _clientName.Text = ViewModel.ClientName;
            _totalFatValue.Text = ViewModel.TotalFatValue;

            _fatRadioGroup.Visibility = ViewModel.FatForClient ? ViewStates.Visible : ViewStates.Gone;

            SetTemplateLayout();
        }

        private void SetTemplateLayout()
        {
            _actionTemplateContainer.Visibility = ViewModel.FatForClient ? ViewStates.Gone : ViewStates.Visible;
            _templateContainer.Visibility = ViewModel.FatForClient ? ViewStates.Gone : ViewStates.Visible;

            _templateOneBt.Enabled = ViewModel.UseTemplateOneCommand.CanExecute(null);
            _templateTwoBt.Enabled = ViewModel.UseTemplateTwoCommand.CanExecute(null);
            _templateThreeBt.Enabled = ViewModel.UseTemplateThreeCommand.CanExecute(null);

            _actionTemplateOneBt.Text = ViewModel.UseTemplateOneCommand.CanExecute(null) ? "Apagar" : "Criar";
            _actionTemplateTwoBt.Text = ViewModel.UseTemplateTwoCommand.CanExecute(null) ? "Apagar" : "Criar";
            _actionTemplateThreeBt.Text = ViewModel.UseTemplateThreeCommand.CanExecute(null) ? "Apagar" : "Criar";
        }

        public override void SetupBindings()
        {
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            _guiasSpinner.ItemSelected += GuiasSpinnerItemSelected;
            _openGuiaIcon.Click += OpenGuiaIconClick;
            ViewModel.CreateFaturaSimplesCommand.CanExecuteChanged += CreateFaturaSimplesCommandCanExecuteChanged;
            _createFaturaBt.Click += CreateFaturaBtClick;
            _addProductBt.Click += AddProductBtClick;
            _fatRadioGroup.CheckedChange += FatRadioGroupCheckedChange;

            _templateOneBt.Click += TemplateOneBtClick;
            _templateTwoBt.Click += TemplateTwoBtClick;
            _templateThreeBt.Click += TemplateThreeBtClick;
            _actionTemplateOneBt.Click += ActionTemplateOneBtClick;
            _actionTemplateTwoBt.Click += ActionTemplateTwoBtClick;
            _actionTemplateThreeBt.Click += ActionTemplateThreeBtClick;
        }

        public override void CleanBindings()
        {
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            _guiasSpinner.ItemSelected -= GuiasSpinnerItemSelected;
            _openGuiaIcon.Click -= OpenGuiaIconClick;
            ViewModel.CreateFaturaSimplesCommand.CanExecuteChanged -= CreateFaturaSimplesCommandCanExecuteChanged;
            _createFaturaBt.Click -= CreateFaturaBtClick;
            _addProductBt.Click -= AddProductBtClick;
            _fatRadioGroup.CheckedChange -= FatRadioGroupCheckedChange;

            _templateOneBt.Click -= TemplateOneBtClick;
            _templateTwoBt.Click -= TemplateTwoBtClick;
            _templateThreeBt.Click -= TemplateThreeBtClick;
            _actionTemplateOneBt.Click -= ActionTemplateOneBtClick;
            _actionTemplateTwoBt.Click -= ActionTemplateTwoBtClick;
            _actionTemplateThreeBt.Click -= ActionTemplateThreeBtClick;
        }

        private void ActionTemplateTwoBtClick(object sender, EventArgs e)
        {
            ViewModel.SetTemplateTwoCommand.Execute();
        }

        private void ActionTemplateThreeBtClick(object sender, EventArgs e)
        {
            ViewModel.SetTemplateThreeCommand.Execute();
        }

        private void ActionTemplateOneBtClick(object sender, EventArgs e)
        {
            ViewModel.SetTemplateOneCommand.Execute();
        }

        private void TemplateThreeBtClick(object sender, EventArgs e)
        {
            ViewModel.UseTemplateThreeCommand.Execute();
        }

        private void TemplateTwoBtClick(object sender, EventArgs e)
        {
            ViewModel.UseTemplateTwoCommand.Execute();
        }

        private void TemplateOneBtClick(object sender, EventArgs e)
        {
            ViewModel.UseTemplateOneCommand.Execute();
        }

        private void FatRadioGroupCheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            ViewModel.FaturaRecibo = _fatReceiptRg.Checked;
        }

        private void AddProductBtClick(object sender, EventArgs e)
        {
            ViewModel.AddProductCommand.Execute(null);
        }

        private void CreateFaturaBtClick(object sender, EventArgs e)
        {
            ViewModel.CreateFaturaSimplesCommand.Execute(null);
        }

        private void CreateFaturaSimplesCommandCanExecuteChanged(object sender, EventArgs e)
        {
            _createFaturaBt.Enabled = ViewModel.CreateFaturaSimplesCommand.CanExecute(null);
        }

        private void OpenGuiaIconClick(object sender, EventArgs e)
        {
            ViewModel.OpenDocCommand.Execute(ViewModel.GuiaSelected);
        }

        private void GuiasSpinnerItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ViewModel.GuiaSelected = ViewModel.LastTrasnportationsDocs[e.Position];
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.LastTrasnportationsDocs):
                    SetLastTrasnportationsDocsRv();
                    break;
                case nameof(ViewModel.FaturationDocs):
                    SeFaturationDocsRv();
                    break;
                case nameof(ViewModel.TotalFatValue):
                    SetUI();
                    break;
                case nameof(ViewModel.ProductsList):
                    _productsListAdapter = new ProductsListFatAdapter(ViewModel.ProductsList);
                    _productsListAdapter.RemoveProduct = RemoveProduct;
                    _productsListAdapter.UpdateValueCommand = ViewModel.UpdateValueCommand;
                    _productsListRv.SetAdapter(_productsListAdapter);
                    CreateFaturaSimplesCommandCanExecuteChanged(null, null);
                    break;
                case nameof(ViewModel.ProductsRemaining):
                    _productsRemainingListAdapter = new ProductsListFatAdapter(ViewModel.ProductsRemaining, true); 
                    _remainingItemsRv.SetAdapter(_productsRemainingListAdapter);
                    break;

                case nameof(ViewModel.TemplateOneKey):
                case nameof(ViewModel.TemplateTwoKey):
                case nameof(ViewModel.TemplateThreeKey):
                    SetTemplateLayout();
                    break;
            }
        }

        private void SetLastTrasnportationsDocsRv()
        {
            _guiasSpinnerAdapter = new LastTrasnportationsDocsSpinnerAdapter(ViewModel.LastTrasnportationsDocs);
            _guiasSpinner.Adapter = _guiasSpinnerAdapter;
        }

        private void RemoveProduct(FatItem fatItemToRemove)
        {
            int pos = ViewModel.ProductsList.IndexOf(fatItemToRemove);

            ViewModel.ProductsList.Remove(fatItemToRemove);
            _productsListAdapter.ProductsList = ViewModel.ProductsList;

            _productsListAdapter.NotifyItemRemoved(pos);
        }
        private void SeFaturationDocsRv()
        {
            _emptyLayout.Visibility = ViewModel.FaturationDocs == null || ViewModel.FaturationDocs.Count == 0 ? ViewStates.Visible : ViewStates.Invisible;

            _lastTrasnportationsDocsAdapter = new LastTrasnportationsDocsAdapter(ViewModel.FaturationDocs);
            _lastTrasnportationsDocsAdapter.DocClick = ViewModel.OpenDocCommand;
            _faturasRv.SetAdapter(_lastTrasnportationsDocsAdapter);
        }
    }
}