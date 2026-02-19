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
using tabApp.Core.Models;
using tabApp.UI.ViewHolders;
using tabApp.UI.ViewHolders.PriceTable;

namespace tabApp.UI.Adapters.EditClient
{
    public class EditListSelectableAdapter : RecyclerView.Adapter
    {
        public override int ItemCount => valueList?.Count ?? 0;

        private List<string> valueList;
        private MvxCommand refreshSaveCommand;
        private ClientProfileListField clientProfileListField;

        public EditListSelectableAdapter(ClientProfileListField clientProfileListField)
        {
            this.valueList = clientProfileListField.ValueList;
            this.refreshSaveCommand = clientProfileListField.RefreshSaveCommand;
            this.clientProfileListField = clientProfileListField;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is ItemRadioViewHolder itemRadioViewHolder)
            {
                itemRadioViewHolder.Bind(valueList[position], clientProfileListField, this);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ItemRadio, parent, false);
            return new ItemRadioViewHolder(view);
        }
    }
}