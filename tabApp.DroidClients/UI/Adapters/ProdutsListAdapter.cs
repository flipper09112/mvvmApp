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
using tabApp.Core.Services.Interfaces.WebServices;
using tabApp.DroidClients.UI.ViewHolders;

namespace tabApp.DroidClients.UI.Adapters
{
    public class ProdutsListAdapter : RecyclerView.Adapter
    {
        private GetProductsOutput productsList;
        private MvxCommand<ProductModel> showSelectedProductCommand;

        public ProdutsListAdapter(GetProductsOutput productsList, MvvmCross.Commands.MvxCommand<ProductModel> showSelectedProductCommand)
        {
            this.productsList = productsList;
            this.showSelectedProductCommand = showSelectedProductCommand;
        }

        public override int ItemCount => productsList?.products?.Count ?? 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ProdutoViewHolder buttonViewHolder = (ProdutoViewHolder)holder;
            buttonViewHolder.Bind(productsList?.products[position], showSelectedProductCommand);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View mView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.produto_list_layout, parent, false);
            ProdutoViewHolder vh = new ProdutoViewHolder(mView);
            return vh;
        }
    }
}