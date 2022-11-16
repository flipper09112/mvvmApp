using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels.Bases.Generic;

namespace tabApp.Helpers
{
    public class MyWebViewClient : WebViewClient
    {
        public MyWebViewClient(DocumentViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public DocumentViewModel ViewModel { get; }

        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            //view.LoadUrl(url);
            return false;
        }

        // For API level 24 and later
        public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
        {
            //view.LoadUrl(request.Url.ToString());
            return false;
        }

        public override void OnPageStarted(WebView view, string url, Android.Graphics.Bitmap favicon)
        {
            ViewModel.IsBusy = true;
            base.OnPageStarted(view, url, favicon);
        }

        public override void OnPageFinished(WebView view, string url)
        {
            ViewModel.IsBusy = false;
            base.OnPageFinished(view, url);
        }

        public override void OnReceivedError(WebView view, ClientError errorCode, string description, string failingUrl)
        {
            base.OnReceivedError(view, errorCode, description, failingUrl);
        }
    }
}