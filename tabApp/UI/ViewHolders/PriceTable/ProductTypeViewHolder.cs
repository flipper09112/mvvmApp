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

namespace tabApp.UI.ViewHolders.PriceTable
{
    public class ProductTypeViewHolder : RecyclerView.ViewHolder
    {
        private RadioButton _radioButton;
        private ProductTypeEnum _productTypeEnum;

        public Action<ProductTypeEnum> Click;

        public ProductTypeViewHolder(View itemView) : base(itemView)
        {
            _radioButton = itemView.FindViewById<RadioButton>(Resource.Id.radioButton);
        }

        internal void Bind(ProductTypeEnum productTypeEnum, ProductTypeEnum productTypeSelected)
        {
            _productTypeEnum = productTypeEnum;
            _radioButton.Text = productTypeEnum.ToString();

            _radioButton.Checked = productTypeSelected == productTypeEnum ? true : false;

            _radioButton.Click -= RadioButtonClick;
            _radioButton.Click += RadioButtonClick;
        }

        private void RadioButtonClick(object sender, EventArgs e)
        {
            Click?.Invoke(_productTypeEnum);
        }
    }
}