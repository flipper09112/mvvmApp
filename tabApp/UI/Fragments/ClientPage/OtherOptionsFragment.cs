using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels;
using tabApp.UI.Adapters;

namespace tabApp.UI.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class OtherOptionsFragment : BaseFragment<OtherOptionsViewModel>
    {
        private GridView _otherOptionsGrid;
        private OtherOptionsGridAdapter _adapter;
        private MainActivity _activity;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _activity = ParentActivity as MainActivity;
            View view = inflater.Inflate(Resource.Layout.OtherOptionsFragment, container, false);

            _otherOptionsGrid = view.FindViewById<GridView>(Resource.Id.otherOptionsGrid);

            return view;
        }

        public override void CleanBindings()
        {
            ViewModel.ShowCalculatorEvent -= ShowCalculatorEvent;
        }

        public override void SetUI()
        {
            _activity.HideToolbar();
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            _adapter = new OtherOptionsGridAdapter(ViewModel.Options);
            _otherOptionsGrid.SetAdapter(_adapter);
        }

        public override void SetupBindings()
        {
            ViewModel.ShowCalculatorEvent += ShowCalculatorEvent;
        }

        private void ShowCalculatorEvent(object sender, EventArgs e)
        {
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();

            PackageManager pm = _activity.PackageManager;
            var packs = pm.GetInstalledPackages(PackageInfoFlags.MatchAll);
            foreach (PackageInfo pi in packs)
            {
                if (pi.PackageName.ToString().ToLower().Contains("calcul"))
                {
                    Dictionary<string, object> map = new Dictionary<string, object>();
                    map["appName"] = pi.ApplicationInfo.LoadLabel(pm);
                    map["packageName"] = pi.PackageName;
                    
                    items.Add(map);
                }
            }

            if (items.Count >= 1)
            {
                string packageName = (string) items[0]["packageName"];
                Intent i = pm.GetLaunchIntentForPackage(packageName);
                if (i != null)
                    _activity.StartActivity(i);
            }
            else
            {
                Toast.MakeText(Context, "APP calculadora não encontrada neste dispositivo", ToastLength.Short).Show();
            }
        }
    }
}