using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels;

namespace tabApp.UI.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class ClientOrderFragment : BaseFragment<ClientOrderViewModel>
    {
        private MainActivity _activity;
        private Button _saveOrderButton;
        private TextView _date;
        private TextView _despesa;
        private TextView _nome;
        private RadioButton _r_total;
        private RadioButton _r_parcial;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.ClientOrderFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _saveOrderButton = view.FindViewById<Button>(Resource.Id.saveOrderButton);
            _date = view.FindViewById<TextView>(Resource.Id.date);
            _despesa = view.FindViewById<TextView>(Resource.Id.despesa);
            _nome = view.FindViewById<TextView>(Resource.Id.nome);
            _r_total = view.FindViewById<RadioButton>(Resource.Id.r_total);
            _r_parcial = view.FindViewById<RadioButton>(Resource.Id.r_parcial);

            return view;
        }

        public override void CleanBindings()
        {
            _date.Click -= DateClick;
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
            ViewModel.SaveNewOrderCommand.CanExecuteChanged -= SaveNewOrderCommandCanExecuteChanged;
            _r_total.Click -= RtotalClick;
            _r_parcial.Click -= RparcialClick;
        }

        public override void SetUI()
        {
            _activity.HideToolbar();
            _date.Text = ViewModel.DateSelected.ToString("dd/MM/yyyy");
            _nome.Text = ViewModel.Client.Name;
            SaveNewOrderCommandCanExecuteChanged(null, null);
        }

        public override void SetupBindings()
        {
            _date.Click += DateClick;
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
            ViewModel.SaveNewOrderCommand.CanExecuteChanged += SaveNewOrderCommandCanExecuteChanged;
            _r_total.Click += RtotalClick;
            _r_parcial.Click += RparcialClick;
        }

        private void RparcialClick(object sender, EventArgs e)
        {
            ViewModel.IsTotal = false;
        }

        private void RtotalClick(object sender, EventArgs e)
        {
            ViewModel.IsTotal = true;
        }

        private void SaveNewOrderCommandCanExecuteChanged(object sender, EventArgs e)
        {
            if (ViewModel.SaveNewOrderCommand.CanExecute(null))
                _saveOrderButton.Enabled = true;
            else
                _saveOrderButton.Enabled = false;
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetUI();
        }

        private void DateClick(object sender, EventArgs e)
        {
            ViewModel.SelectDateCommand.Execute();
        }
    }
}