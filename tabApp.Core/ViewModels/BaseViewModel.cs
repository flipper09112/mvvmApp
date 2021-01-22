using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace tabApp.Core.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel
    {
        protected BaseViewModel()
        {
        }

        public abstract void Appearing();

        public EventHandler PropertyChange;

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            Appearing();
        }
    }

    public abstract class BaseViewModel<TParameter, TResult> : MvxViewModel<TParameter, TResult>
        where TParameter : class
        where TResult : class
    {
    }
}
