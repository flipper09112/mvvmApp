using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabApp.Core.Models;
using tabApp.UI.ViewHolders;

namespace tabApp.UI.Adapters
{
    public class ProductsItemsListAdapter : RecyclerView.Adapter
    {
        private List<Product> allProducts;
        private string searchWord;
        private MvxCommand<Product> click;

        public ProductsItemsListAdapter(List<Product> allProducts, string searchWord, MvxCommand<Product> selectProductCommand)
        {
            this.allProducts = allProducts;
            this.searchWord = searchWord;
            this.click = selectProductCommand;
        }

        public override int ItemCount => allProducts.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            SimpleProductViewHolder simpleProductVH = holder as SimpleProductViewHolder;
            simpleProductVH.Bind(allProducts[holder.AdapterPosition], searchWord);
            simpleProductVH.Click = click;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.SimpleProductItem, parent, false);
            return new SimpleProductViewHolder(view);
        }
    }
}