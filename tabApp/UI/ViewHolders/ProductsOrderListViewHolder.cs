﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models;
using tabApp.Helpers;

namespace tabApp.UI.Adapters
{
    public class ProductsOrderListViewHolder : RecyclerView.ViewHolder
    {
        private TextView _productName;
        private LinearLayout _layout;
        private EditText _productAmmount;
        private Action<bool> _hideButtons;
        private ProductAmmount product;

        public MvxCommand SaveButtonCanChange { get; internal set; }

        public ProductsOrderListViewHolder(View itemView) : base(itemView)
        {
            _productName = itemView.FindViewById<TextView>(Resource.Id.productName);
            _layout = itemView.FindViewById<LinearLayout>(Resource.Id.layout);
            _productAmmount = itemView.FindViewById<EditText>(Resource.Id.productAmmount);

            _productAmmount.FocusChange -= ProductAmmountFocusChange;
            _productAmmount.FocusChange += ProductAmmountFocusChange;
        }

        private void ProductAmmountFocusChange(object sender, View.FocusChangeEventArgs e)
        {
            //_hideButtons?.Invoke(e.HasFocus);
        }

        internal void Bind(ProductAmmount product, bool withoutMargins, Action<bool> hideButtons)
        {
            _hideButtons = hideButtons;
            this.product = product;
            SetupEditText();
            _productName.Text = product.Product.Name;
            _productAmmount.Text = product.Product.Unity ? product.Ammount.ToString("N0") : product.Ammount.ToString("N2");

            if(withoutMargins)
            {
                SetMargins(_layout, 10, 10);
            }
        }

        private void SetupEditText()
        {
            _productAmmount.SetFilters(new IInputFilter[] { new DecimalDigitsInputFilter(2) });
            _productAmmount.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal;

            _productAmmount.TextChanged -= ProductAmmountTextChanged;
            _productAmmount.TextChanged += ProductAmmountTextChanged;
        }

        private void ProductAmmountTextChanged(object sender, TextChangedEventArgs e)
        {
            double ammount = 0;
            Double.TryParse(e.Text.ToString().Replace(".", ","), out ammount);
            product.Ammount = ammount;
            SaveButtonCanChange?.RaiseCanExecuteChanged();
        }

        public void SetMargins(View v, int l, int r)
        {
            if (v.LayoutParameters is ViewGroup.MarginLayoutParams) {
                ViewGroup.MarginLayoutParams p = (ViewGroup.MarginLayoutParams)v.LayoutParameters;
                p.SetMargins(l, p.TopMargin, r, p.BottomMargin);
                v.RequestLayout();
            }
        }
    }
}