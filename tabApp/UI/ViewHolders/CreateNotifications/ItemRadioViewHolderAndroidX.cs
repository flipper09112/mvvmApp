using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModels.EditClient;

namespace tabApp.UI.ViewHolders.CreateNotifications
{
    class ItemRadioViewHolderAndroidX : RecyclerView.ViewHolder
    {
        private RadioButton _radioButton;
        private string _type;
        private PreDefinedAlertItem _item;
        private MvxCommand<PreDefinedAlertItem> _selectItemCommand;

        public ItemRadioViewHolderAndroidX(View itemView) : base(itemView)
        {
            _radioButton = itemView.FindViewById<RadioButton>(Resource.Id.radioButton);

            _radioButton.Click -= RadioButtonClick;
            _radioButton.Click += RadioButtonClick;
        }

        private void RadioButtonClick(object sender, EventArgs e)
        {
            _selectItemCommand?.Execute(_item);
        }

        internal void Bind(PreDefinedAlertItem preDefinedAlertItem, 
                           PreDefinedAlertItem preDefinedAlertSelected, 
                           MvvmCross.Commands.MvxCommand<PreDefinedAlertItem> selectPreDefinedAlertCommand)
        {
            _item = preDefinedAlertItem;
            _selectItemCommand = selectPreDefinedAlertCommand;

            _radioButton.Text = preDefinedAlertItem.ItemName;
            _radioButton.Checked = preDefinedAlertItem == preDefinedAlertSelected;
        }
    }
}