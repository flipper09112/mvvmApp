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
        public void ShowConfirmDialog(string question, string confirmText, Action<bool> confirmAction)
        {
            var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            var act = top.Activity;

            CheckBox checkBox = new CheckBox(act);

            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(act);
            alert.SetTitle(question);
            alert.SetMessage("Remover extra?");
            alert.SetView(checkBox);
            alert.SetPositiveButton(confirmText, (senderAlert, args) =>
            {
                confirmAction.Invoke(checkBox.Checked);
            });

            Dialog dialog = alert.Create();
            dialog.Show();

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