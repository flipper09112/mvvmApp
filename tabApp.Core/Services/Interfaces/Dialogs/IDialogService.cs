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
        void ShowDatePickerDialog(Action<DateTime> confirmAction);
    }
}
