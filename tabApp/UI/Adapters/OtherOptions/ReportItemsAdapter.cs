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
using tabApp.Core.ViewModels.Global.Other;
using tabApp.UI.ViewHolders;

namespace tabApp.UI.Adapters.OtherOptions
{
    public class ReportItemsAdapter : RecyclerView.Adapter
    {
        public override int ItemCount => selectedProducts?.Count + 1 ?? 1;

        private List<Product> selectedProducts;
        private ReportViewModel viewModel;

        public ReportItemsAdapter(List<Product> selectedProducts, ReportViewModel viewModel)
        {
            this.selectedProducts = selectedProducts;
            this.viewModel = viewModel;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if(holder is AddItemReportViewHolder addItemReportViewHolder)
            {
                addItemReportViewHolder.Click = AddProduct;
            }
            else if (holder is ItemReportViewHolder itemReportViewHolder)
            {
                itemReportViewHolder.Click = RemoveProduct;
                itemReportViewHolder.Bind(selectedProducts[holder.AdapterPosition]);
            }
        }

        private void RemoveProduct(Product product)
        {
            viewModel.RemoveProductCommand.Execute(product);
        }

        private void AddProduct()
        {
            viewModel.AddProductCommand.Execute(null);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if(viewType == (selectedProducts?.Count ?? 0))
            {
                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.AddItemReport, parent, false);
                return new AddItemReportViewHolder(view);
            }
            else
            {
                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ItemReport, parent, false);
                return new ItemReportViewHolder(view);
            }
        }

        public override int GetItemViewType(int position)
        {
            return position;
        }
    }
}