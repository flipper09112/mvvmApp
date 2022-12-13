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
using tabApp.Core.Services.Interfaces.WebServices.Sells.DTOs;
using tabApp.UI.ViewHolders.Faturation;

namespace tabApp.UI.Adapters.Faturation
{
    public class ProductsListFatAdapter : RecyclerView.Adapter
    {
        public ProductsListFatAdapter(List<FatItem> productsList)
        {
            ProductsList = productsList;
        }

        public override int ItemCount => ProductsList?.Count ?? 0;

        public List<FatItem> ProductsList { get; set; }
        public Action<FatItem> RemoveProduct { get; internal set; }
        public MvxCommand UpdateValueCommand { get; internal set; }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ProductFatViewHolder lastTrasnportationsDoc = holder as ProductFatViewHolder;
            lastTrasnportationsDoc.Bind(ProductsList[holder.AdapterPosition], RemoveProduct, UpdateValueCommand);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ProductFatItem, parent, false);
            return new ProductFatViewHolder(view);
        }
    }
}