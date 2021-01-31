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
using tabApp.Helpers;
using tabApp.Services.Implementations;

namespace tabApp.UI.ViewHolders
{
    public class EditProfileItemViewHolder : RecyclerView.ViewHolder
    {
        private Context context;
        private ImageView _icon;
        private TextView _editLabel;
        private EditText _editValue;
        private ClientProfileField clientProfileField;

        public EditProfileItemViewHolder(View itemView) : base(itemView)
        {
            context = itemView.Context;

            _icon = itemView.FindViewById<ImageView>(Resource.Id.icon);
            _editLabel = itemView.FindViewById<TextView>(Resource.Id.editLabel);
            _editValue = itemView.FindViewById<EditText>(Resource.Id.editValue);
        }

        internal void Bind(ClientProfileField clientProfileField)
        {
            this.clientProfileField = clientProfileField;

            _editLabel.Text = clientProfileField.Name;
            _editValue.Text = clientProfileField.NewValue != null ? clientProfileField.NewValue : clientProfileField.Value;
            _icon.SetImageResource(ImageHelper.GetImageResource(context, clientProfileField.IconName));

            if (clientProfileField.IsDate)
            {
                _editValue.Click -= EditValueClick;
                _editValue.Click += EditValueClick;
            }
            else
            {
                if (clientProfileField.IsInt)
                {
                    _editValue.SetFilters(new IInputFilter[] { new DecimalDigitsInputFilter(0) });
                    _editValue.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal;
                }
                if (clientProfileField.IsDouble)
                {
                    _editValue.SetFilters(new IInputFilter[] { new DecimalDigitsInputFilter(2) });
                    _editValue.InputType = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal;

                }
                _editValue.TextChanged -= EditValueTextChanged;
                _editValue.TextChanged += EditValueTextChanged;
            }
        }

        private void EditValueClick(object sender, EventArgs e)
        {
            new DialogService().ShowDatePickerDialog(SetNewDate);
        }

        private void SetNewDate(DateTime obj)
        {
            _editValue.Text = obj.ToString("dd/MM/yyyy");
            EditValueTextChanged(null, null);
        }

        private void EditValueTextChanged(object sender, TextChangedEventArgs e)
        {
            clientProfileField.NewValue = _editValue.Text.ToString();
            clientProfileField.RefreshSaveCommand.RaiseCanExecuteChanged();
        }
    }
}