using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels;
using Xamarin.Essentials;

namespace tabApp.UI
{
    public abstract class BaseFragment : MvxFragment
    {
        private BaseViewModel Vm;

        public MvxAppCompatActivity ParentActivity
        {
            get
            {
                return (MvxAppCompatActivity)Activity;
            }
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                Vm = (BaseViewModel)ViewModel;
                if (Vm != null)
                {
                    Vm.PropertyChanged -= VmPropertyChanged;
                    Vm.PropertyChanged += VmPropertyChanged;
                }
            }
            finally { }
            
        }

        private void VmPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case "IsBusy":
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        MainActivity mainActivity = ParentActivity as MainActivity;

                        mainActivity.ViewModelPropertyChanged(Vm.IsBusy, e);
                    });
                   
                    break;
            }
        }

        public override void OnResume()
        {
            base.OnResume();
            SetUI();
            SetupBindings();
        }

        public override void OnPause()
        {
            base.OnPause();
            CleanBindings();
        }

        public abstract void CleanBindings();
        public abstract void SetupBindings();

        public abstract override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState);

        public abstract void SetUI();
    }

    public abstract class BaseFragment<TViewModel> : BaseFragment where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

    }
}