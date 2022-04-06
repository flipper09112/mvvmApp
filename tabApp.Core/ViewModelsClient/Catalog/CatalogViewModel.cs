using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Products;
using tabApp.Core.ViewModels;

namespace tabApp.Core.ViewModelsClient.Catalog
{
    public class CatalogViewModel : BaseViewModel
    {
        public MvxCommand<string> NextScreenCommand { get; set; }
        public MvxCommand<string> CakesTypesCommand { get; set; }
        public MvxCommand GoBackMenuCommand { get; set; }

        private IMvxNavigationService _navigationService;
        private IChooseProductService _chooseProductService;

        public int Position { get; set; }

        public CatalogViewModel(IMvxNavigationService navigationService,
                                IChooseProductService chooseProductService)
        {
            _navigationService = navigationService;
            _chooseProductService = chooseProductService;

            NextScreenCommand = new MvxCommand<string>(NextScreen);
            CakesTypesCommand = new MvxCommand<string>(CakesTypes);
            GoBackMenuCommand = new MvxCommand(GoBackMenu);

            Position = 0;
            CatalogTypeItemsList = GetList(Position);
        }

        private void GoBackMenu()
        {
            Position = 0;
            CatalogTypeItemsList = GetList(Position);
        }

        public List<CatalogTypeItem> catalogTypeItemsList;
        public List<CatalogTypeItem> CatalogTypeItemsList
        {
            get
            {
                return catalogTypeItemsList;
            }
            set
            {
                catalogTypeItemsList = value;
                RaisePropertyChanged(nameof(CatalogTypeItemsList));
            }
        }

        private void CakesTypes(string s)
        {
            Position = 1;
            CatalogTypeItemsList = GetList(Position);
        }

        private async void NextScreen(string typeSelected)
        {
            _chooseProductService.ProductTypeSelected = (ProductTypeEnum)Enum.Parse(typeof(ProductTypeEnum), typeSelected);
            await _navigationService.Navigate<ProductsListViewModel>();
        }

        private List<CatalogTypeItem> GetList(int position)
        {
            switch(position)
            {
                case 0:
                    var lis = new List<CatalogTypeItem>();
                    lis.Add(new CatalogTypeItem("Padaria", "ic_bread", NextScreenCommand, ProductTypeEnum.Padaria));
                    lis.Add(new CatalogTypeItem("Pastelaria", "ic_cake", CakesTypesCommand, ProductTypeEnum.None));
                    lis.Add(new CatalogTypeItem("Outros", "ic_flour", NextScreenCommand, ProductTypeEnum.Outros));
                    return lis;
                case 1:
                    var list = new List<CatalogTypeItem>();
                    list.Add(new CatalogTypeItem("Pastelaria Individual Doce", "ic_cake", NextScreenCommand, ProductTypeEnum.PastelariaIndividual));
                    list.Add(new CatalogTypeItem("Pastelaria Individual Salgada", "ic_cake", NextScreenCommand, ProductTypeEnum.PastelariaIndividualSalgada));
                    list.Add(new CatalogTypeItem("Semi Frio Individual", "ic_cake", NextScreenCommand, ProductTypeEnum.SemiFrioIndividual));
                    list.Add(new CatalogTypeItem("Semi Frio Familiar", "ic_cake", NextScreenCommand, ProductTypeEnum.SemiFrioFamiliar));
                    list.Add(new CatalogTypeItem("Bolos Tradicionais", "ic_cake", NextScreenCommand, ProductTypeEnum.BolosTradicionais));
                    list.Add(new CatalogTypeItem("Sortido", "ic_cake", NextScreenCommand, ProductTypeEnum.Sortido));
                    list.Add(new CatalogTypeItem("Tartes", "ic_cake", NextScreenCommand, ProductTypeEnum.Tartes));
                    list.Add(new CatalogTypeItem("Tortas", "ic_cake", NextScreenCommand, ProductTypeEnum.Tortas));
                    list.Add(new CatalogTypeItem("Bolos Festivos", "ic_cake", NextScreenCommand, ProductTypeEnum.BolosFestivos));
                    return list;
                default:
                    return new List<CatalogTypeItem>();
            }
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }

    public class CatalogTypeItem
    {
        public string Name { get; set; }
        public string IconName { get; set; }
        public MvxCommand<string> Command { get; set; }
        public ProductTypeEnum ProductType { get; set; }

        public CatalogTypeItem(string name, string iconName, MvxCommand<string> nextScreenCommand, ProductTypeEnum productType)
        {
            Name = name;
            IconName = iconName;
            Command = nextScreenCommand;
            ProductType = productType;
        }
    }
}
