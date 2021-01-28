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
        private bool _isBusy;
        public bool IsBusy { 
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(nameof(IsBusy));
            }
        }
        protected BaseViewModel()
        {
        }

        public abstract void Appearing();
        public abstract void DisAppearing();

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            Appearing();
        }

        public override void ViewDisappeared()
        {
            base.ViewDisappeared();
            DisAppearing();
        }
    }

    public abstract class BaseViewModel<TParameter, TResult> : MvxViewModel<TParameter, TResult>
        where TParameter : class
        where TResult : class
    {
    }
}
