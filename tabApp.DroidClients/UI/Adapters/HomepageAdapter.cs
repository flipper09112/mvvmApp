using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.ViewModelsClient;
using tabApp.DroidClients.UI.ViewHolders;

namespace tabApp.DroidClients.UI.Adapters
{
    public class HomepageAdapter : RecyclerView.Adapter
    {
        private List<HomepageItem> homepageItems;

        public HomepageAdapter(List<HomepageItem> homepageItems)
        {
            this.homepageItems = homepageItems;
        }

        public override int ItemCount => homepageItems?.Count ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if(holder is HomepageItemViewHolder homepageItemVH)
            {
                homepageItemVH.Bind(homepageItems[holder.AdapterPosition]);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.HomepageItem, parent, false);
            return new HomepageItemViewHolder(view);
        }
    }
}