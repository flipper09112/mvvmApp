using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.ViewModels;

namespace tabApp.Core.ViewModelsClient.Catalog
{
    public class CatalogViewModel : BaseViewModel
    {
        public MvxCommand<string> NextScreenCommand { get; set; }
        public MvxCommand<string> CakesTypesCommand { get; set; }

        private IMvxNavigationService _navigationService;

        public int Position { get; set; }

        public CatalogViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            NextScreenCommand = new MvxCommand<string>(NextScreen);
            CakesTypesCommand = new MvxCommand<string>(CakesTypes);

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

        private void NextScreen(string typeSelected)
        {
            //await _navigationService.Navigate<InfoViewModel>();
        }

        private List<CatalogTypeItem> GetList(int position)
        {
            switch(position)
            {
                case 0:
                    var lis = new List<CatalogTypeItem>();
                    lis.Add(new CatalogTypeItem("Padaria", "ic_bread", NextScreenCommand));
                    lis.Add(new CatalogTypeItem("Pastelaria", "ic_cake", CakesTypesCommand));
                    lis.Add(new CatalogTypeItem("Outros", "ic_flour", NextScreenCommand));
                    return lis;
                case 1:
                    var list = new List<CatalogTypeItem>();
                    list.Add(new CatalogTypeItem("Pastelaria Individual Doce", "ic_cake", NextScreenCommand));
                    list.Add(new CatalogTypeItem("Pastelaria Individual Salgada", "ic_cake", NextScreenCommand));
                    list.Add(new CatalogTypeItem("Semi Frio Individual", "ic_cake", NextScreenCommand));
                    list.Add(new CatalogTypeItem("Semi Frio Familiar", "ic_cake", NextScreenCommand));
                    list.Add(new CatalogTypeItem("Bolos Tradicionais", "ic_cake", NextScreenCommand));
                    list.Add(new CatalogTypeItem("Sortido", "ic_cake", NextScreenCommand));
                    list.Add(new CatalogTypeItem("Tartes", "ic_cake", NextScreenCommand));
                    list.Add(new CatalogTypeItem("Tortas", "ic_cake", NextScreenCommand));
                    list.Add(new CatalogTypeItem("Bolos Festivos", "ic_cake", NextScreenCommand));
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

        public CatalogTypeItem(string name, string iconName, MvxCommand<string> nextScreenCommand)
        {
            Name = name;
            IconName = iconName;
            Command = nextScreenCommand;
        }
    }
}
