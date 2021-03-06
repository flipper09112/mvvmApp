﻿using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Global.Bt;

namespace tabApp.UI.Fragments.Global.Bt
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class BtIncomingFragment : BaseFragment<BtIncomingViewModel>
    {
        private MainActivity _activity;
        private LottieAnimationView _lottie;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.BtIncomingFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _lottie = view.FindViewById<LottieAnimationView>(Resource.Id.lottie);

            return view;
        }

        public override void CleanBindings()
        {
            CloseConnection();
        }

        public override void SetUI()
        {
        }

        public override void SetupBindings()
        {
            Connect();
        }

        public async void Connect()
        {
            ViewModel.StartServer(DisableLottie, ErrorLottie, FinishedLottie);
        }
        public void CloseConnection()
        {
            ViewModel.StopServer();
        }

        public void DisableLottie()
        {
            _lottie.ImageAssetsFolder = "Lotties/";
            LottieDrawable drawable = new LottieDrawable();
            LottieComposition.Factory.FromAssetFileName(Context, "Lotties/connected.json", (composition) => {
                _lottie.Composition = composition;
                _lottie.PlayAnimation();
            });
        }
        public void ErrorLottie()
        {
            _lottie.ImageAssetsFolder = "Lotties/";
            LottieDrawable drawable = new LottieDrawable();
            LottieComposition.Factory.FromAssetFileName(Context, "Lotties/error.json", (composition) => {
                _lottie.Composition = composition;
                _lottie.PlayAnimation();
            });
        }
        public void FinishedLottie()
        {
            _lottie.ImageAssetsFolder = "Lotties/";
            LottieDrawable drawable = new LottieDrawable();
            LottieComposition.Factory.FromAssetFileName(Context, "Lotties/success.json", (composition) => {
                _lottie.Composition = composition;
                _lottie.PlayAnimation();
            });
        }
    }
}