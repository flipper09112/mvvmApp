using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;

namespace tabApp.Core.ViewModels.Bases
{
    public partial class BaseOptionsListViewModel : BaseViewModel
    {
        public List<Option> Options { get; set; }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
}
