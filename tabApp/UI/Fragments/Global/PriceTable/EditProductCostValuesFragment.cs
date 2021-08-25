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
using tabApp.Core.ViewModels.Global.PriceTable;
using tabApp.UI.Adapters.Global;

namespace tabApp.UI.Fragments.Global.PriceTable
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class EditProductCostValuesFragment : BaseFragment<EditProductCostValuesViewModel>
    {
        private MainActivity _activity;
        private TextView _pageTitle;
        private Button _saveButton;
        private RecyclerView _pricesEditRv;
        private EditProductCostAdapter _adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.EditProductCostValuesFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _pageTitle = view.FindViewById<TextView>(Resource.Id.pageTitle);
            _saveButton = view.FindViewById<Button>(Resource.Id.saveButton);
            _pricesEditRv = view.FindViewById<RecyclerView>(Resource.Id.pricesEditRv);

            return view;
        }

        public override void SetUI()
        {
            _activity.HideToolbar();

            _pageTitle.Text = "Editar preços de " + ViewModel.ProductSelected.Name;
            _saveButton.Enabled = ViewModel.SaveChangesCommand.CanExecute();

            SetRV();
        }

        private void SetRV()
        {
            if (_adapter != null) return;

            _adapter = new EditProductCostAdapter(ViewModel.ValuesForChangeList);
            _pricesEditRv.SetLayoutManager(new LinearLayoutManager(Context));
            _pricesEditRv.SetAdapter(_adapter);
        }

        public override void SetupBindings()
        {
            ViewModel.SaveChangesCommand.CanExecuteChanged += SaveChangesCommandCanExecuteChanged;
            _saveButton.Click += SaveButtonClick;
        }
        public override void CleanBindings()
        {
            ViewModel.SaveChangesCommand.CanExecuteChanged -= SaveChangesCommandCanExecuteChanged;
            _saveButton.Click -= SaveButtonClick;
        }
        private void SaveButtonClick(object sender, EventArgs e)
        {
            if(ViewModel.SaveChangesCommand.CanExecute())
                ViewModel.SaveChangesCommand?.Execute();
        }
        private void SaveChangesCommandCanExecuteChanged(object sender, EventArgs e)
        {
            SetUI();
        }
    }
}