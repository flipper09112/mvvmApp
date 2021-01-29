using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.ViewModels
{
    public class EditClientViewModel : BaseViewModel
    {

        public List<EditFieldsEnum> TabsOptions
        {
            get
            {
                List<EditFieldsEnum> items = new List<EditFieldsEnum>();
                items.Add(EditFieldsEnum.Profile);
                items.Add(EditFieldsEnum.Localização);
                items.Add(EditFieldsEnum.Admin);
                return items;
            }
        }
        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }

    public enum EditFieldsEnum
    {
        Profile,
        Localização,
        Admin
    }
}
