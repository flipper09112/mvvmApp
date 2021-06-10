using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using tabApp.Core.Models;
using tabApp.UI.Adapters.EditClient;

namespace tabApp.UI.ViewHolders
{
    public class ItemRadioViewHolder : RecyclerView.ViewHolder
    {
        private RadioButton _radioButton;
        private ClientProfileListField _clientProfileListField;
        private EditListSelectableAdapter _editListSelectableAdapter;
        private string _type;

        public ItemRadioViewHolder(View itemView) : base(itemView)
        {
            _radioButton = itemView.FindViewById<RadioButton>(Resource.Id.radioButton);

            _radioButton.Click -= RadioButtonClick;
            _radioButton.Click += RadioButtonClick;
        }

        private void RadioButtonClick(object sender, EventArgs e)
        {
            _clientProfileListField.NewValue = _type;
            _editListSelectableAdapter.NotifyDataSetChanged();
            _clientProfileListField.RefreshSaveCommand.RaiseCanExecuteChanged();
        }

        internal void Bind(string name, ClientProfileListField clientProfileListField, EditListSelectableAdapter editListSelectableAdapter)
        {
            _clientProfileListField = clientProfileListField;
            _editListSelectableAdapter = editListSelectableAdapter;

            _type = name;

            _radioButton.Text = name;
            _radioButton.Checked = IsChecked(clientProfileListField, name) ? true : false;
        }

        private bool IsChecked(ClientProfileListField clientProfileListField, string name)
        {
            if (clientProfileListField.NewValue != null)
                return clientProfileListField.NewValue.Equals(name);
            else
                return clientProfileListField.Value.Equals(name);
        }
    }
}