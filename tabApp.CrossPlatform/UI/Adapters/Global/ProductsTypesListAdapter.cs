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
using tabApp.Core.ViewModels.Global.PriceTable;
using tabApp.UI.ViewHolders.PriceTable;

namespace tabApp.UI.Adapters.Global
{
    public class ProductsTypesListAdapter : RecyclerView.Adapter
    {
        public override int ItemCount => productsTypesList?.Count ?? 0;

        private AddProductViewModel _viewModel;
        private List<ProductTypeEnum> productsTypesList;

        public Action<ProductTypeEnum> Click;

        public ProductsTypesListAdapter(List<ProductTypeEnum> productsTypesList, AddProductViewModel viewModel)
        {
            _viewModel = viewModel;
            this.productsTypesList = productsTypesList;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var vh = holder as ProductTypeViewHolder;
            vh.Click = SelectItem;
            vh.Bind(productsTypesList[position], _viewModel.ProductTypeSelected);
        }

        private void SelectItem(ProductTypeEnum obj)
        {
            Click?.Invoke(obj);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.SelectClientRadio, parent, false);
            return new ProductTypeViewHolder(view);
        }
    }
}