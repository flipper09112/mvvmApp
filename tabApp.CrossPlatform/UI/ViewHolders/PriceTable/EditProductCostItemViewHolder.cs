using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels.Global.PriceTable;
using tabApp.Helpers;

namespace tabApp.UI.ViewHolders.PriceTable
{
    public class EditProductCostItemViewHolder : RecyclerView.ViewHolder
    {
        private Context context;
        private ImageView _icon;
        private TextView _editLabel;
        private EditText _editValue;
        private ValueChange _item;

        public EditProductCostItemViewHolder(View itemView) : base(itemView)
        {
            context = itemView.Context;

            _icon = itemView.FindViewById<ImageView>(Resource.Id.icon);
            _editLabel = itemView.FindViewById<TextView>(Resource.Id.editLabel);
            _editValue = itemView.FindViewById<EditText>(Resource.Id.editValue);
        }

        internal void Bind(ValueChange valueChange)
        {
            _item = valueChange;

            _icon.SetImageResource(ImageHelper.GetImageResource(context, "ic_money"));

            _editValue.SetFilters(new IInputFilter[] { new DecimalDigitsInputFilter(3) });
            _editValue.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal;

            _editLabel.Text = valueChange.Name;
            _editValue.Text = valueChange.NewValue != 0 ? valueChange.NewValue.ToString("N3") : valueChange.Value.ToString("N3");

            _editValue.TextChanged -= EditValueTextChanged;
            _editValue.TextChanged += EditValueTextChanged;
        }

        private void EditValueTextChanged(object sender, TextChangedEventArgs e)
        {
            double newAmmount = -1;
            var parsed = double.TryParse(_editValue.Text.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture, out newAmmount);

            if (newAmmount != _item.NewValue) {
                _item.NewValue = newAmmount;
                _item.CanContinueRefreshCommand.RaiseCanExecuteChanged();
            }
        }
    }
}