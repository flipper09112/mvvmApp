using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using iText.Kernel.Pdf;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Bases.Generic;
using tabApp.Helpers;

namespace tabApp.UI.Fragments.Bases.Generic
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class DocumentFragment : BaseFragment<DocumentViewModel>
    {
        private WebView _webview;
        private MainActivity _mainActivity;
        private bool _loaded;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.DocumentFragment, container, false);

            _webview = view.FindViewById<WebView>(Resource.Id.webview);

            return view;
        }

        public override void SetUI()
        {
            if (_loaded) return;
            _loaded = true;
            _webview.SetWebViewClient(new MyWebViewClient(ViewModel));

            _webview.Settings.JavaScriptEnabled = true;
            _webview.Settings.SetPluginState(WebSettings.PluginState.On);
            _webview.LoadUrl("https://docs.google.com/gview?embedded=true&url=" + ViewModel.DocUrl);
        }

        public override void SetupBindings()
        {
        }

        public override void CleanBindings()
        {
        }
    }

}