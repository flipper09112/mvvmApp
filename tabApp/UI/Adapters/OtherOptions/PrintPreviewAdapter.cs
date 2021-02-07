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
using tabApp.Core.Models;
using tabApp.UI.ViewHolders;

namespace tabApp.UI.Adapters.OtherOptions
{
    public class PrintPreviewAdapter : RecyclerView.Adapter
    {
        private List<PrintPreview> printPreviewList;

        public PrintPreviewAdapter(List<PrintPreview> printPreviewList)
        {
            this.printPreviewList = printPreviewList;
        }

        public override int ItemCount => printPreviewList?.Count ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            PrintPreviewViewHolder vh = holder as PrintPreviewViewHolder;
            vh.Bind(printPreviewList[holder.AdapterPosition]);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.PrintPreview, parent, false);
            return new PrintPreviewViewHolder(view);
        }
    }
}