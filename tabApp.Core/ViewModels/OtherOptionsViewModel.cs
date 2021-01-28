using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.ViewModels
{
    public class OtherOptionsViewModel : BaseViewModel
    {
        public OtherOptionsViewModel()
        {

        }

        public List<Option> Options
        {
            get
            {
                List<Option> options = new List<Option>();

                options.Add(new Option(null, "Imprimir Conta", "ic_printer"));
                options.Add(new Option(null, "Alterar Quantidade", "ic_change"));
                //options.Add(new Option(null, "Imprimir", "ic_printer"));

                return options;
            }
        }
        public override void Appearing()
        {
        }
        public override void DisAppearing()
        {
        }
    }
}
