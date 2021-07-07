using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models;
using tabApp.Helpers;

namespace tabApp.UI.ViewHolders.PriceTable
{
    public class EditProductItemViewHolder : RecyclerView.ViewHolder
    {
        private Context context;
        private ImageView _icon;
        private TextView _editLabel;
        private EditText _editValue;
        private int _clientId;

        public MvxCommand<(int ClientId, string newValue)> ChangeValueCommand { get; internal set; }

        public EditProductItemViewHolder(View itemView) : base(itemView)
        {
            context = itemView.Context;

            _icon = itemView.FindViewById<ImageView>(Resource.Id.icon);
            _editLabel = itemView.FindViewById<TextView>(Resource.Id.editLabel);
            _editValue = itemView.FindViewById<EditText>(Resource.Id.editValue);

        }

        private void EditValueTextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeValueCommand?.Execute((_clientId, _editValue.Text.Replace(",", ".")));
        }

        internal void Bind(double value, bool pvp, string clientName = "", int clientId = -1)
        {
            _clientId = clientId;

            _icon.SetImageResource(ImageHelper.GetImageResource(context, "ic_money"));

            _editValue.SetFilters(new IInputFilter[] { new DecimalDigitsInputFilter(2) });
            _editValue.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal;

            _editLabel.Text = pvp ? "Valor PVP" : "Preço de revenda de " + clientName;
            _editValue.Text = value.ToString("N2");

            _editValue.TextChanged -= EditValueTextChanged;
            _editValue.TextChanged += EditValueTextChanged;
        }
    }
}