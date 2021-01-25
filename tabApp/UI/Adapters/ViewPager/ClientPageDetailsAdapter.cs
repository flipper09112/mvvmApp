using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models;
using tabApp.UI.ViewHolders;

namespace tabApp.UI.Adapters.ViewPager
{
    public class ClientPageDetailsAdapter : RecyclerView.Adapter
    {
        private List<Regist> detailsList;

        public ClientPageDetailsAdapter(List<Regist> detailsList)
        {
            this.detailsList = detailsList;
        }

        public override int ItemCount => detailsList?.Count ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            DetailViewHolder detailVH = holder as DetailViewHolder;
            detailVH.Bind(detailsList[holder.AdapterPosition]);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.DetailItem, parent, false);
            return new DetailViewHolder(view);
        }
    }
}