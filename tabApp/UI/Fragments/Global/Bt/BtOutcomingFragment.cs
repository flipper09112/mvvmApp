using Android.App;
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
using tabApp.Services.Implementations.Native;

namespace tabApp.UI.Fragments.Global.Bt
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, true)]
    public class BtOutcomingFragment : BaseFragment<BtOutcomingViewModel>
    {
        private MainActivity _activity;
        private LottieAnimationView _lottie;
        private Button _startButton;
        private Button _sendData;
        private TextView _dateOutBt;
        private bool _ready;

        private int count = 1;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.BtOutcomingFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _lottie = view.FindViewById<LottieAnimationView>(Resource.Id.view);
            _startButton = view.FindViewById<Button>(Resource.Id.startButton);
            _sendData = view.FindViewById<Button>(Resource.Id.sendData);
            _dateOutBt = view.FindViewById<TextView>(Resource.Id.dateOutBt);

            return view;
        }
        public override void CleanBindings()
        {
            _startButton.Click -= StartButtonClick;
            _sendData.Click -= SendDataClick;
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
        }

        public override void SetUI()
        {
            _dateOutBt.Text = ViewModel.DateSelected.ToString("dd/MM/yyyy");
        }

        public override void SetupBindings()
        {
            _startButton.Click += StartButtonClick;
            _sendData.Click += SendDataClick;
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
        }

        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetUI();
        }

        private void SendDataClick(object sender, EventArgs e)
        {
            BluetoothManagerService.Write(ViewModel.ClientList);
            _sendData.Enabled = false;
        }

        private void StartButtonClick(object sender, EventArgs e)
        {
            ViewModel.Connect(ConnectedLottie, ErrorLottie, FinishedLottie, IncrementProgressiveBar);
        }

        private void IncrementProgressiveBar()
        {
            /*if(_ready)
            {
                _ready = false;
                _lottie.SetMaxProgress(ViewModel.ClientList.Count);
                _lottie.Progress = count;
            } else
            {
                count++;
                _lottie.Progress = count;
            }*/
        }

        public void ConnectedLottie()
        {
            _lottie.ImageAssetsFolder = "Lotties/";
            LottieDrawable drawable = new LottieDrawable();
            LottieComposition.Factory.FromAssetFileName(Context, "Lotties/load.json", (composition) => {
                _lottie.Composition = composition;
                _lottie.PlayAnimation();
            });

            _startButton.Enabled = false;
            _ready = true;
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