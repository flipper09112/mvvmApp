using Android.App;
using Android.Content;
using Android.Icu.Text;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using MvvmCross;
using MvvmCross.Platforms.Android;
using MvvmCross.Platforms.Android.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Helpers;
using tabApp.UI.Adapters.Home;

namespace tabApp.Services.Implementations
{
    public class DialogService : IDialogService
    {
        private Action<DateTime> _confirmAction;
        private EditText et;
        private RadioButton addRB;
        private RadioButton subRB;
        private AlertDialog _longPressPopUp;

        public void ShowConfirmDialog(string question, string confirmText, Action<bool> confirmAction)
        {
            var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            var act = top.Activity;

            LayoutInflater inflater = act.LayoutInflater;
            View dialogView = inflater.Inflate(Resource.Layout.PaymentDialog, null);
            CheckBox checkBox = (CheckBox)dialogView.FindViewById(Resource.Id.extraCheckBox);
            checkBox.Checked = true;

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
            dialog.DatePicker.MinDate = DateTimeOffset.Now.ToUnixTimeMilliseconds();
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


            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(act);
            alert.SetTitle(question);
            alert.SetMessage(""); 
            alert.SetPositiveButton(confirmText, (senderAlert, args) =>
            {
                double value;
                double.TryParse(et.Text.Replace(".", ","), out value);
                if(value != 0)
                    confirmAction.Invoke(addRB.Checked ? value : (value * -1));
            });
            alert.SetView(/*et*/Resource.Layout.AddExtraDialog);

            Dialog dialog = alert.Create();
            dialog.Show();

            et = dialog.FindViewById<EditText>(Resource.Id.textView5);
            et.SetFilters(new IInputFilter[] { new DecimalDigitsInputFilter(2) });
            et.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal;

            addRB = dialog.FindViewById<RadioButton>(Resource.Id.addRB);
            addRB.Checked = true;
            subRB = dialog.FindViewById<RadioButton>(Resource.Id.addRB);

        }

        public void ShowSuccessChangeSnackBar(string info)
        {
           

            try {

                var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
                var act = top.Activity;

                Android.Views.View activityRootView = act.FindViewById(Android.Resource.Id.Content);
                Snackbar.Make(activityRootView, info, Snackbar.LengthLong).Show();
            } catch (Exception e)
            {
                try
                {
                    var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
                    var act = top.Activity;

                    Toast.MakeText(act, info, ToastLength.Long);

                } catch(Exception ex)
                {

                }
            }
        }

        public void ShowChooseOptions(List<LongPressItem> data)
        {
            var top = Mvx.Resolve<IMvxAndroidCurrentTopActivity>();
            var act = top.Activity;

            _longPressPopUp = new AlertDialog.Builder(act)
                                        .SetView(GetChoiceView(act, data))
                                        .Create();

            _longPressPopUp.Show();
        }

        private View GetChoiceView(Activity act, List<LongPressItem> data)
        {
            View view = LayoutInflater.From(act).Inflate(Resource.Layout.dialog_choice, null);
            var _popUpGv = (GridView)view.FindViewById(Resource.Id.gv_choice);

            var _loadingImagePopUp = view.FindViewById<ImageView>(Resource.Id.custom_loading_imageView);
            Glide.With(act)
                    .Load(Resource.Drawable.loading)
                    .Into(_loadingImagePopUp);

            //Custom adapter
            LongPressPopPupAdapter adapter;
            //Judgment type, load data source settings Adapter
            adapter = new LongPressPopPupAdapter(data, CloseLongPressPopUp);
            _popUpGv.Adapter = adapter;
            //Set the default selection
            //adapter.(eventSelected);

            return view;
        }

        private void CloseLongPressPopUp()
        {
            _longPressPopUp?.Dismiss();
            _longPressPopUp = null;
        }
    }
}