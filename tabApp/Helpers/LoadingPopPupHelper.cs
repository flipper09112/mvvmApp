using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using MvvmCross.Droid.Support.V4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Thread = Java.Lang.Thread;

namespace tabApp.Helpers
{
    public static class LoadingPopPupHelper
    {
        private static ProgressBar _text;
        private static TextView _text2;
        private static Dialog _dialog;

        public static void ShowDialog(this MvxFragment fragment, string msg)
        {
            _dialog = new Dialog(fragment.Context);
            _dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            _dialog.SetCancelable(false);
            _dialog.SetContentView(Resource.Layout.loadingDialog);

            _text = (ProgressBar)_dialog.FindViewById(Resource.Id.progress_horizontal);
            _text2 = _dialog.FindViewById<TextView>(Resource.Id.value123);

            new Thread(Action).Run();

            _dialog.Show();

            Window window = _dialog.Window;
            window.SetLayout(WindowManagerLayoutParams.MatchParent, WindowManagerLayoutParams.WrapContent);
        }

        private static void Action()
        {
            int status = 0;
            while (status < 100)
            {
                status += 1;

                try
                {
                    Thread.Sleep(200);
                }
                catch (InterruptedException e)
                {
                    e.PrintStackTrace();
                }

                _text.SetProgress(status, true);
                _text2.Text = status.ToString();

                if (status == 100)
                {
                    _dialog.Dismiss();
                }
            }
        }
    }
}