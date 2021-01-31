using Android.App;
using Android.Content;
using Android.Icu.Text;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using MvvmCross;
using MvvmCross.Platforms.Android;
using MvvmCross.Platforms.Android.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Helpers;

namespace tabApp.Services.Implementations
{
    public class DialogService : IDialogService
    {
        private Action<DateTime> _confirmAction;

        public void ShowConfirmDialog(string question, string confirmText, Action<bool> confirmAction)
        {
            var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            var act = top.Activity;

            LayoutInflater inflater = act.LayoutInflater;
            View dialogView = inflater.Inflate(Resource.Layout.PaymentDialog, null);
            CheckBox checkBox = (CheckBox)dialogView.FindViewById(Resource.Id.extraCheckBox);

            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(act);
            alert.SetTitle(question);
            alert.SetMessage("Remover extra?");
            alert.SetView(dialogView);
            alert.SetPositiveButton(confirmText, (senderAlert, args) =>
            {
                confirmAction?.Invoke(checkBox.Checked);
            }); 

            Dialog dialog = alert.Create();
            dialog.Show();
        }
        public void ShowConfirmDialog(string question, string confirmText, Action<object> confirmAction, string cancelText, object obj)
        {
            var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            var act = top.Activity;

            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(act);
            alert.SetTitle(question);
            alert.SetPositiveButton(confirmText, (senderAlert, args) =>
            {
                confirmAction.Invoke(obj);
            }); 
            alert.SetNeutralButton(cancelText, (senderAlert, args) =>
            {
            });

            Dialog dialog = alert.Create();
            dialog.Show();
        }

        public void ShowDatePickerDialog(Action<DateTime> confirmAction)
        {
            var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            var act = top.Activity;

            _confirmAction = confirmAction;

            DateTime today = DateTime.Today;
            DatePickerDialog dialog = new DatePickerDialog(act, OnDateSet, today.Year, today.Month - 1, today.Day);
            dialog.DatePicker.MinDate = today.Millisecond;
            dialog.Show();
        }

        private void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            _confirmAction.Invoke(e.Date);
        }

        public void ShowInputDialog(string question, string confirmText, Action<double> confirmAction)
        {
            var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            var act = top.Activity;

            EditText et = new EditText(act);
            et.SetFilters(new IInputFilter[] { new DecimalDigitsInputFilter(2) });
            et.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal;
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(act);
            alert.SetTitle(question);
            alert.SetMessage(""); 
            alert.SetPositiveButton(confirmText, (senderAlert, args) =>
            {
                double value;
                double.TryParse(et.Text.Replace(".", ","), out value);
                confirmAction.Invoke(value);
            });
            alert.SetView(et);

            Dialog dialog = alert.Create();
            dialog.Show();
        }

    }
}