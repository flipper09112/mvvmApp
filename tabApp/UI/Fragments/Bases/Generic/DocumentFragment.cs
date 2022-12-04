using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using AndroidX.CardView.Widget;
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
        private CardView _posPrinterBt;
        private CardView _openExternalApp;
        private CardView _printerBt;
        private MainActivity _mainActivity;
        private bool _loaded;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.DocumentFragment, container, false);

            _webview = view.FindViewById<WebView>(Resource.Id.webview);
            _posPrinterBt = view.FindViewById<CardView>(Resource.Id.posPrinterBt);
            _openExternalApp = view.FindViewById<CardView>(Resource.Id.openExternalApp);
            _printerBt = view.FindViewById<CardView>(Resource.Id.printerBt);

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
            _posPrinterBt.Click += PosPrinterBtClick;
            _openExternalApp.Click += OpenExternalAppClick;
            _printerBt.Click += PrinterBtClick;
        }

        public override void CleanBindings()
        {
            _posPrinterBt.Click -= PosPrinterBtClick;
            _openExternalApp.Click -= OpenExternalAppClick;
            _printerBt.Click -= PrinterBtClick;
        }

        private void PrinterBtClick(object sender, EventArgs e)
        {
        }

        private void OpenExternalAppClick(object sender, EventArgs e)
        {
            ViewModel.OpenExternalAppCommand.Execute(null);
        }

        private void PosPrinterBtClick(object sender, EventArgs e)
        {
            ViewModel.PosPrinterCommand.Execute(null);
        }
    }
}