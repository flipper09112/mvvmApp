﻿using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Presenters;
using System.Collections.Generic;
using System.Reflection;
using tabApp.Core;
using tabApp.Core.Services.Implementations.Bluetooth;
using tabApp.Core.Services.Interfaces;
using tabApp.Core.Services.Interfaces.Bluetooth;
using tabApp.Core.Services.Interfaces.DB;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Services;
using tabApp.Services.Implementations;
using tabApp.Services.Implementations.CrossPlat;

namespace tabApp
{
    public class Setup : MvxAppCompatSetup<App>
    {
        protected override IEnumerable<Assembly> AndroidViewAssemblies => new List<Assembly>(base.AndroidViewAssemblies)
        {
            typeof(Android.Support.V7.Widget.Toolbar).Assembly,
            typeof(RecyclerView).Assembly,
            typeof(CoordinatorLayout).Assembly,
            typeof(AppBarLayout).Assembly,
            typeof(FrameLayout).Assembly,
            typeof(RelativeLayout).Assembly,
            typeof(View).Assembly,
        };

        /// <summary>
        /// Fill the Binding Factory Registry with bindings from the support library.
        /// </summary>
        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            MvxAppCompatSetupHelper.FillTargetFactories(registry);
            base.FillTargetFactories(registry);
        }

        protected override void InitializePlatformServices()
        {
            base.InitializePlatformServices();

            Mvx.LazyConstructAndRegisterSingleton<IFileService, FileService>();
            Mvx.LazyConstructAndRegisterSingleton<IDialogService, DialogService>();
            Mvx.LazyConstructAndRegisterSingleton<IBluetoothService, BluetoothService>();
            Mvx.LazyConstructAndRegisterSingleton<ISQLiteService, SQLiteService>();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new MvxAppCompatViewPresenter(AndroidViewAssemblies);
        }
    }
}