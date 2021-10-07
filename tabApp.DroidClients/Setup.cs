using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using tabApp.Core;
using tabApp.Core.Services.Interfaces;
using tabApp.Core.Services.Interfaces.DB;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.DroidClients.Services.Implementations;

namespace tabApp.DroidClients
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
            //Mvx.LazyConstructAndRegisterSingleton<IBluetoothService, BluetoothService>();
            Mvx.LazyConstructAndRegisterSingleton<ISQLiteService, SQLiteService>();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new MvxAppCompatViewPresenter(AndroidViewAssemblies);
        }
    }
}