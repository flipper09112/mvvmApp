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
using tabApp.Core.ViewModelsClient.Catalog;
using tabApp.DroidClients.UI.ViewHolders;

namespace tabApp.DroidClients.UI.Adapters
{
    public class CatalogAdapter : RecyclerView.Adapter
    {
        private List<CatalogTypeItem> catalogTypeItemsList;

        public CatalogAdapter(List<CatalogTypeItem> catalogTypeItemsList)
        {
            this.catalogTypeItemsList = catalogTypeItemsList;
        }

        public override int ItemCount => catalogTypeItemsList?.Count ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if(holder is CatalogItemViewHolder catalogItemViewHolder)
            {
                catalogItemViewHolder.Bind(catalogTypeItemsList[holder.AdapterPosition]);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.button_layout, parent, false);
            return new CatalogItemViewHolder(view);
        }
    }
}