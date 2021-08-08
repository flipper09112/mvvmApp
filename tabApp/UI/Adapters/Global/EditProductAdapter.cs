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
    public class EditProductAdapter : RecyclerView.Adapter
    {
        public override int ItemCount => productSelected.ReSaleValues?.Count ?? 0 + 1;

        private Product productSelected;
        private EditProductViewModel viewModel;

        public EditProductAdapter(Product productSelected, EditProductViewModel viewModel)
        {
            this.productSelected = productSelected;
            this.viewModel = viewModel;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if(holder is EditProductItemViewHolder editProductItemViewHolder)
            {
                editProductItemViewHolder.ChangeValueCommand = viewModel.ChangeValueCommand;
                if (position == 0)
                    editProductItemViewHolder.Bind(viewModel.GetValue(-1, productSelected.PVP), true);
                else
                    editProductItemViewHolder.Bind(viewModel.GetValue(productSelected.ReSaleValues[position - 1].ClientId, productSelected.ReSaleValues[position - 1].Value),
                                                   false, 
                                                   viewModel.GetClientName(productSelected.ReSaleValues[position - 1].ClientId),
                                                   productSelected.ReSaleValues[position - 1].ClientId);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.EditProfileItem, parent, false);
            return new EditProductItemViewHolder(view);
        }
    }
}