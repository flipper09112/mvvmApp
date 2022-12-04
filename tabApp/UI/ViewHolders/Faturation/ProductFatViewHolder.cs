using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;

namespace tabApp.UI.ViewHolders.Faturation
{
    public class ProductFatViewHolder : RecyclerView.ViewHolder
    {
        private TextView _productId;
        private TextView _productName;
        private TextView _productVat;
        private TextView _productAmmount;
        private ImageView _deleteIcon;
        private Action<FatItem> _removeProduct;
        private FatItem _fatItem;

        public ProductFatViewHolder(View itemView) : base(itemView)
        {
            _productId = itemView.FindViewById<TextView>(Resource.Id.productId);
            _productName = itemView.FindViewById<TextView>(Resource.Id.productName);
            _productVat = itemView.FindViewById<TextView>(Resource.Id.productVat);
            _productAmmount = itemView.FindViewById<EditText>(Resource.Id.productAmmount);
            _deleteIcon = itemView.FindViewById<ImageView>(Resource.Id.deleteIcon);

            _deleteIcon.Click -= DeleteIconClick;
            _deleteIcon.Click += DeleteIconClick;

            _productAmmount.TextChanged -= ProductAmmountTextChanged;
            _productAmmount.TextChanged += ProductAmmountTextChanged;
        }

        private void ProductAmmountTextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            _fatItem.Quantity = e.Text.ToString();
        }

        private void DeleteIconClick(object sender, EventArgs e)
        {
            _removeProduct.Invoke(_fatItem);
        }

        internal void Bind(FatItem fatItem, Action<FatItem> removeProduct)
        {
            _removeProduct = removeProduct;
            _fatItem = fatItem;

            _productAmmount.Text = fatItem.Quantity;
            _productId.Text = fatItem.Id;
            _productName.Text = fatItem.Details;
            _productVat.Text = fatItem.Vat + "%";
        }
    }
}