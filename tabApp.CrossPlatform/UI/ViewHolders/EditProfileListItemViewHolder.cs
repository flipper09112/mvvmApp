using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models;
using tabApp.Helpers;
using tabApp.UI.Adapters.EditClient;

namespace tabApp.UI.ViewHolders
{
    public class EditProfileListItemViewHolder : RecyclerView.ViewHolder
    {
        private Context context;
        private ImageView _icon;
        private TextView _editLabel;
        private RecyclerView _editValueRv;
        private ClientProfileListField clientProfileListField;
        private EditListSelectableAdapter _adapter;

        public EditProfileListItemViewHolder(View itemView) : base(itemView)
        {
            context = itemView.Context;

            _icon = itemView.FindViewById<ImageView>(Resource.Id.icon);
            _editLabel = itemView.FindViewById<TextView>(Resource.Id.editLabel);
            _editValueRv = itemView.FindViewById<RecyclerView>(Resource.Id.editValueRv);
        }
        internal void Bind(ClientProfileListField clientProfileField)
        {
            this.clientProfileListField = clientProfileField;

            _editLabel.Text = clientProfileField.Name;
            _icon.SetImageResource(ImageHelper.GetImageResource(context, clientProfileField.IconName));
            SetRvData();
        }

        private void SetRvData()
        {
            _editValueRv.SetLayoutManager(new LinearLayoutManager(context, LinearLayoutManager.Horizontal, false));
            _adapter = new EditListSelectableAdapter(clientProfileListField);
            _editValueRv.SetAdapter(_adapter);
        }
    }
}