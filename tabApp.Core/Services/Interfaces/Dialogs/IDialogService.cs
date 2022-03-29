using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.Services.Interfaces.Dialogs
{
    public interface IDialogService
    {
        void ShowConfirmDialog(string question, string confirmText, Action<bool> confirmAction);
        void ShowConfirmDialog(string question, string confirmText, Action<object> confirmAction, string cancelText, object obj);

        void ShowInputDialog(string question, string confirmText, Action<double> confirmAction);
        void ShowDatePickerDialog(Action<DateTime> confirmAction, bool withMinDate, DateTime? minDate = null, DateTime? maxDate = null);
        void ShowSuccessChangeSnackBar(string info);
        void ShowChooseOptions(List<LongPressItem> data);
        void Show(string v1, string v2);
        void ShowErrorDialog(string title, string description, Action confirmAction = null);
    }
}
