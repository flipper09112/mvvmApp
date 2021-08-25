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
using tabApp.Core.ViewModels.Global.PriceTable;
using tabApp.UI.ViewHolders.PriceTable;

namespace tabApp.UI.Adapters.Global
{
    public class EditProductCostAdapter : RecyclerView.Adapter
    {
        private List<ValueChange> valuesForChangeList;

        public EditProductCostAdapter(List<ValueChange> valuesForChangeList)
        {
            this.valuesForChangeList = valuesForChangeList;
        }

        public override int ItemCount => valuesForChangeList?.Count ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is EditProductCostItemViewHolder editProductCostItemVH)
            {
                editProductCostItemVH.Bind(valuesForChangeList[holder.AdapterPosition]);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.EditProfileItem, parent, false);
            return new EditProductCostItemViewHolder(view);
        }
    }
}