using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Bumptech.Glide;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Square.Picasso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModelsClient;
using tabApp.Core.ViewModelsClient.Catalog;
using tabApp.UI;

namespace tabApp.DroidClients.UI.Fragments.Catalog
{
    [MvxFragmentPresentation(typeof(HomePageViewModel), Resource.Id.fragmentContainer, true)]
    public class ProductDetailsFragment : BaseFragment<ProductDetailsViewModel>
    {
        private MainActivity _activity;
        private TextView _nameLabel;
        private TextView _refLabel;
        private TextView _precoLabel;
        private TextView _tipoLabel;
        private ImageView _image;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.ProductDetailsFragment, container, false);

            _activity = ParentActivity as MainActivity;

            _nameLabel = view.FindViewById<TextView>(Resource.Id.nome);
            _refLabel = view.FindViewById<TextView>(Resource.Id.reference);
            _precoLabel = view.FindViewById<TextView>(Resource.Id.preco);
            _tipoLabel = view.FindViewById<TextView>(Resource.Id.tipo);
            _image = view.FindViewById<ImageView>(Resource.Id.imageProduct);

            return view;
        }

        public override void SetUI()
        {
            if (ViewModel.Product == null || ViewModel.Product.Product == null) return;

            _nameLabel.Text = ViewModel.Product.Product.Name;
            _refLabel.Text = ViewModel.Product.Product.ImageReference;

            if (ViewModel.Product.Product.Unity)
            {
                _tipoLabel.Text = "Unitário";
                _precoLabel.Text = $"{ViewModel.Product.Product.PVP.ToString("0.00")} €";
            }
            else
            {
                _tipoLabel.Text = "Kilograma";
                _precoLabel.Text = $"{ViewModel.Product.Product.PVP.ToString("0.00")} €/kg";
            }

            SetImage();
        }

        private void SetImage()
        {
            if (ViewModel.Product.ProductImage != null)
            {
                if (ViewModel.Product.ProductImage != null)
                {
                    byte[] imageByteArray = Base64.Decode(ViewModel.Product.ProductImage, Base64Flags.Default);

                    Glide.With(Activity)
                          .Load(imageByteArray)
                          .Error(Resource.Drawable.image_not_found)
                          .Into(_image);
                }
                else
                    Picasso.With(Activity).Load(Resource.Drawable.image_not_found).Fit().Into(_image);
            }
            else
            {
                Picasso.With(Activity).Load(Resource.Drawable.image_not_found).Fit().Into(_image);
            }
        }

        public override void SetupBindings()
        {
            ViewModel.PropertyChanged += ViewModelPropertyChanged;
        }

        public override void CleanBindings()
        {
            ViewModel.PropertyChanged -= ViewModelPropertyChanged;
        }
        private void ViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetUI();
        }
    }
}