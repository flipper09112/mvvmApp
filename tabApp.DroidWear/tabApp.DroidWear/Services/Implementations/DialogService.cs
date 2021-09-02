using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core;
using tabApp.Core.Services.Interfaces.Dialogs;

namespace tabApp.DroidWear.Services.Implementations
{
    public class DialogService : IDialogService
    {
        public void Show(string v1, string v2)
        {
            throw new NotImplementedException();
        }

        public void ShowChooseOptions(List<LongPressItem> data)
        {
            throw new NotImplementedException();
        }

        public void ShowConfirmDialog(string question, string confirmText, Action<bool> confirmAction)
        {
            throw new NotImplementedException();
        }

        public void ShowConfirmDialog(string question, string confirmText, Action<object> confirmAction, string cancelText, object obj)
        {
            throw new NotImplementedException();
        }

        public void ShowDatePickerDialog(Action<DateTime> confirmAction, bool withMinDate, DateTime? minDate = null, DateTime? maxDate = null)
        {
            throw new NotImplementedException();
        }

        public void ShowInputDialog(string question, string confirmText, Action<double> confirmAction)
        {
            throw new NotImplementedException();
        }

        public void ShowSuccessChangeSnackBar(string info)
        {
            throw new NotImplementedException();
        }
    }
}