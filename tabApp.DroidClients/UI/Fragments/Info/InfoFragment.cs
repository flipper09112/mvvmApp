using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModelsClient;
using tabApp.Core.ViewModelsClient.Info;
using tabApp.UI;
using Xamarin.Essentials;

namespace tabApp.DroidClients.UI.Fragments.Info
{
    [MvxFragmentPresentation(typeof(HomePageViewModel), Resource.Id.fragmentContainer, true)]
    public class InfoFragment : BaseFragment<InfoViewModel>
    {
        private MainActivity _activity;
        private AppCompatButton _button_manuela;
        private AppCompatButton _button_filipe;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.InfoFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _button_manuela = view.FindViewById<AppCompatButton>(Resource.Id.button_manuela);
            _button_filipe = view.FindViewById<AppCompatButton>(Resource.Id.button_filipe);

            return view;
        }

        public override void SetUI()
        {
        }

        public override void SetupBindings()
        {
            _button_filipe.Click += ButtonFilipeClick;
            _button_manuela.Click += ButtonManuelaClick;
        }

        public override void CleanBindings()
        {
            _button_filipe.Click -= ButtonFilipeClick;
            _button_manuela.Click -= ButtonManuelaClick;
        }
        private void ButtonManuelaClick(object sender, EventArgs e)
        {
            PlacePhoneCall("964690528");
        }

        private void ButtonFilipeClick(object sender, EventArgs e)
        {
            PlacePhoneCall("961122213");
        }

        public void PlacePhoneCall(string number)
        {
            try
            {
                PhoneDialer.Open(number);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
    }
}