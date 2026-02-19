using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Load.Engine;
using Com.Bumptech.Glide.Request;
using Java.Lang;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels;
using tabApp.Core.ViewModels.Main;

namespace tabApp.UI.Fragments.Home
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.fragmentContainer, false)]
    public class SplahFragment : BaseFragment<SplashViewModel>
    {
        private MainActivity _activity;
        private ImageView _imageView;
        private Handler _handler;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.SplahFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _imageView = view.FindViewById<ImageView>(Resource.Id.videoView);

            

            return view;
        }

        private void ShowHomePage()
        {
            ViewModel.ShowHomePage.Execute();
        }

        public override void CleanBindings()
        {
        }

        public override void SetUI()
        {
            _activity.HideToolbar();

            _handler = new Handler();
            _handler.PostDelayed(ShowHomePage, 6000);

            RequestOptions options = new RequestOptions()
                    .Placeholder(Resource.Drawable.gif_base_image)
                    .InvokeDiskCacheStrategy(DiskCacheStrategy.All)
                    .InvokePriority(Priority.High);

            Glide.With(this)
                .Load(Resource.Drawable.gif_base)
                .Apply(options)
                .Into(_imageView);
        }

        public override void SetupBindings()
        {
        }
    }
}