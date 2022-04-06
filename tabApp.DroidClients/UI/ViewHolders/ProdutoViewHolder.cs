using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using Java.IO;
using Java.Nio.FileNio;
using Java.Util.Regex;
using MvvmCross.Commands;
using Square.Picasso;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using tabApp.Core.Services.Interfaces.WebServices;
using Xamarin.Essentials;

namespace tabApp.DroidClients.UI.ViewHolders
{
    public class ProdutoViewHolder : RecyclerView.ViewHolder
    {
        private ConstraintLayout _widget;
        private ImageView _image;
        private TextView _produtoName;
        private ProductModel _product;
        private MvxCommand<ProductModel> _showSelectedProductCommand;

        public ProdutoViewHolder(View itemView) : base(itemView)
        {
            _widget = itemView.FindViewById<ConstraintLayout>(Resource.Id.product_widget);
            _image = itemView.FindViewById<ImageView>(Resource.Id.image_product);
            _produtoName = itemView.FindViewById<TextView>(Resource.Id.product_name);

            _widget.Click -= WidgetClick;
            _widget.Click += WidgetClick;
        }

        private void WidgetClick(object sender, EventArgs e)
        {
            _showSelectedProductCommand?.Execute(_product);
        }

        internal void Bind(ProductModel productModel, MvvmCross.Commands.MvxCommand<ProductModel> showSelectedProductCommand)
        {
            _product = productModel;
            _showSelectedProductCommand = showSelectedProductCommand;

            Context context = Android.App.Application.Context;
            _produtoName.Text = FormatText(_product.ProductName);

            if (_product.ProductImage != null)
            {
                if (_product.ProductImage != null)
                {
                    byte[] imageByteArray = Base64.Decode(_product.ProductImage, Base64Flags.Default); 
                    
                    Glide.With(context)
                          .Load(imageByteArray)
                          .Error(Resource.Drawable.image_not_found)
                          .Into(_image);
                }
                else
                    Picasso.With(context).Load(Resource.Drawable.image_not_found).Fit().Into(_image);
            }
            else
            {
                Picasso.With(context).Load(Resource.Drawable.image_not_found).Fit().Into(_image);
            }
        }

        public Bitmap Base64ToBitmap(String base64String)
        {
            byte[] imageAsBytes = Base64.Decode(base64String, Base64Flags.Default);
            return BitmapFactory.DecodeByteArray(imageAsBytes, 0, imageAsBytes.Length);
        }

        private string FormatText(string nome)
        {
            Java.Util.Regex.Pattern pattern = Java.Util.Regex.Pattern.Compile("^\\D*(\\d)");
            Matcher matcher = pattern.Matcher(nome);
            matcher.Find();

            try
            {
                int res = matcher.Start(1);
                return nome.Substring(0, res);
            }
            catch (Exception e)
            {
                return nome;
            }

        }
    }
}