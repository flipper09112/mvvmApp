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
    public class SelectDaysPageFragment : BaseFragment<SelectDaysPageViewModel>
    {
        private MainActivity _activity;
        private Button _selectButton;
        private RadioGroup _radioGroup;
        private RadioButton _radioButtonSeg;
        private RadioButton _radioButtonTer;
        private RadioButton _radioButtonQua;
        private RadioButton _radioButtonQui;
        private RadioButton _radioButtonSex;
        private RadioButton _radioButtonSab;
        private RadioButton _radioButtonDom;
        private CheckBox _checkBoxSeg;
        private CheckBox _checkBoxTer;
        private CheckBox _checkBoxQua;
        private CheckBox _checkBoxQui;
        private CheckBox _checkBoxSex;
        private CheckBox _checkBoxSab;
        private CheckBox _checkBoxDom;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.SelectDaysPageFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _selectButton = view.FindViewById<Button>(Resource.Id.selectButton);

            _radioGroup = view.FindViewById<RadioGroup>(Resource.Id.radioGroup);

            _radioButtonSeg = view.FindViewById<RadioButton>(Resource.Id.radioButtonSeg);
            _radioButtonTer = view.FindViewById<RadioButton>(Resource.Id.radioButtonTer);
            _radioButtonQua = view.FindViewById<RadioButton>(Resource.Id.radioButtonQua);
            _radioButtonQui = view.FindViewById<RadioButton>(Resource.Id.radioButtonQui);
            _radioButtonSex = view.FindViewById<RadioButton>(Resource.Id.radioButtonSex);
            _radioButtonSab = view.FindViewById<RadioButton>(Resource.Id.radioButtonSab);
            _radioButtonDom = view.FindViewById<RadioButton>(Resource.Id.radioButtonDom);

            _checkBoxSeg = view.FindViewById<CheckBox>(Resource.Id.checkBoxSeg);
            _checkBoxTer = view.FindViewById<CheckBox>(Resource.Id.checkBoxTer);
            _checkBoxQua = view.FindViewById<CheckBox>(Resource.Id.checkBoxQua);
            _checkBoxQui = view.FindViewById<CheckBox>(Resource.Id.checkBoxQui);
            _checkBoxSex = view.FindViewById<CheckBox>(Resource.Id.checkBoxSex);
            _checkBoxSab = view.FindViewById<CheckBox>(Resource.Id.checkBoxSab);
            _checkBoxDom = view.FindViewById<CheckBox>(Resource.Id.checkBoxDom);

            return view;
        }
        public override void CleanBindings()
        {
            _radioGroup.CheckedChange -= RadioGroupCheckedChange;
            ViewModel.SelectDaysCommand.CanExecuteChanged -= SelectDaysCommandCanExecuteChanged;

            _checkBoxSeg.CheckedChange -= CheckBoxSegCheckedChange;
            _checkBoxTer.CheckedChange -= CheckBoxTerCheckedChange;
            _checkBoxQua.CheckedChange -= CheckBoxQuaCheckedChange;
            _checkBoxQui.CheckedChange -= CheckBoxQuiCheckedChange;
            _checkBoxSex.CheckedChange -= CheckBoxSexheckedChange;
            _checkBoxSab.CheckedChange -= CheckBoxSabCheckedChange;
            _checkBoxDom.CheckedChange -= CheckBoxDomCheckedChange;

            _selectButton.Click -= SelectButtonClick;
        }

        public override void SetUI()
        {
            _activity.HideToolbar();
            SelectDaysCommandCanExecuteChanged(null, null);
        }

        public override void SetupBindings()
        {
            _radioGroup.CheckedChange += RadioGroupCheckedChange;
            ViewModel.SelectDaysCommand.CanExecuteChanged += SelectDaysCommandCanExecuteChanged;

            _checkBoxSeg.CheckedChange += CheckBoxSegCheckedChange;
            _checkBoxTer.CheckedChange += CheckBoxTerCheckedChange;
            _checkBoxQua.CheckedChange += CheckBoxQuaCheckedChange;
            _checkBoxQui.CheckedChange += CheckBoxQuiCheckedChange;
            _checkBoxSex.CheckedChange += CheckBoxSexheckedChange;
            _checkBoxSab.CheckedChange += CheckBoxSabCheckedChange;
            _checkBoxDom.CheckedChange += CheckBoxDomCheckedChange;

            _selectButton.Click += SelectButtonClick;
        }

        private void SelectButtonClick(object sender, EventArgs e)
        {
            ViewModel.SelectDaysCommand.Execute(null);
        }

        private void CheckBoxDomCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            ViewModel.AddDayToPastCommand.Execute(DayOfWeek.Sunday);
        }

        private void CheckBoxSabCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            ViewModel.AddDayToPastCommand.Execute(DayOfWeek.Saturday);
        }

        private void CheckBoxSexheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            ViewModel.AddDayToPastCommand.Execute(DayOfWeek.Friday);
        }

        private void CheckBoxQuiCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            ViewModel.AddDayToPastCommand.Execute(DayOfWeek.Thursday);
        }

        private void CheckBoxQuaCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            ViewModel.AddDayToPastCommand.Execute(DayOfWeek.Wednesday);
        }

        private void CheckBoxTerCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            ViewModel.AddDayToPastCommand.Execute(DayOfWeek.Tuesday);
        }

        private void CheckBoxSegCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            ViewModel.AddDayToPastCommand.Execute(DayOfWeek.Monday);
        }

        private void SelectDaysCommandCanExecuteChanged(object sender, EventArgs e)
        {
            if (ViewModel.SelectDaysCommand.CanExecute(null))
                _selectButton.Enabled = true;
            else
                _selectButton.Enabled = false;
        }

        private void RadioGroupCheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            switch(e.CheckedId)
            {
                case Resource.Id.radioButtonSeg:
                    ViewModel.DaySelectedToCopy = DayOfWeek.Monday;
                    break;
                case Resource.Id.radioButtonTer:
                    ViewModel.DaySelectedToCopy = DayOfWeek.Tuesday;
                    break;
                case Resource.Id.radioButtonQua:
                    ViewModel.DaySelectedToCopy = DayOfWeek.Wednesday;
                    break;
                case Resource.Id.radioButtonQui:
                    ViewModel.DaySelectedToCopy = DayOfWeek.Thursday;
                    break;
                case Resource.Id.radioButtonSex:
                    ViewModel.DaySelectedToCopy = DayOfWeek.Friday;
                    break;
                case Resource.Id.radioButtonSab:
                    ViewModel.DaySelectedToCopy = DayOfWeek.Saturday;
                    break;
                case Resource.Id.radioButtonDom:
                    ViewModel.DaySelectedToCopy = DayOfWeek.Sunday;
                    break;
            }
        }
    }
}