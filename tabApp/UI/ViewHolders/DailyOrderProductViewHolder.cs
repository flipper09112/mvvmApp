using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.ViewModels;
using tabApp.Helpers;

namespace tabApp.UI.ViewHolders
{
    public class DailyOrderProductViewHolder : RecyclerView.ViewHolder
    {
        private TextView _productName;
        private TextView _productAmmount;
        private EditText _productEditAmmount;
        private ClientProfileField clientProfileField;

        public DailyOrderProductViewHolder(View itemView) : base(itemView)
        {
            _productName = itemView.FindViewById<TextView>(Resource.Id.productName);
            _productAmmount = itemView.FindViewById<TextView>(Resource.Id.productAmmount);
            _productEditAmmount = itemView.FindViewById<EditText>(Resource.Id.productAmmountet);
        }

        internal void Bind((Product Product, string Ammount) p)
        {
            _productName.Text = p.Product.Name;
            _productAmmount.Text = p.Ammount;
        }

        internal void Bind(ClientProfileField clientProfileField)
        {
            this.clientProfileField = clientProfileField;

            _productName.Text = clientProfileField.Product.Name;
            _productEditAmmount.Text = clientProfileField.NewValue != null ? clientProfileField.NewValue : clientProfileField.Value;

            _productEditAmmount.SetFilters(new IInputFilter[] { new DecimalDigitsInputFilter(2) });
            _productEditAmmount.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal;

            _productEditAmmount.TextChanged -= ProductEditAmmountTextChanged;
            _productEditAmmount.TextChanged += ProductEditAmmountTextChanged;
        }

        private void ProductEditAmmountTextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            clientProfileField.NewValue = _productEditAmmount.Text;
            clientProfileField.RefreshSaveCommand.RaiseCanExecuteChanged();
        }
    }
}