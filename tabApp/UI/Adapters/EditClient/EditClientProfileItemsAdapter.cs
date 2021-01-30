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
using tabApp.Core.ViewModels;
using tabApp.UI.ViewHolders;

namespace tabApp.UI.Adapters.EditClient
{
    public class EditClientProfileItemsAdapter : RecyclerView.Adapter
    {
        private List<ClientProfileField> profileItems;

        public EditClientProfileItemsAdapter(List<ClientProfileField> profileItems)
        {
            this.profileItems = profileItems;
        }

        public override int ItemCount => profileItems?.Count ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            EditProfileItemViewHolder vh = holder as EditProfileItemViewHolder;
            vh.Bind(profileItems[holder.AdapterPosition]);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.EditProfileItem, parent, false);
            return new EditProfileItemViewHolder(view);
        }
    }
}