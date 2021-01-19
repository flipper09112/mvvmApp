using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace tabApp.Core.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel
    {
        protected BaseViewModel()
        {
        }

        public abstract void Appearing();

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
