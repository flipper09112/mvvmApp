using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.ViewModels
{
    public class SelectDaysPageViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IAddProductToOrderService _addProductToOrderService;

        public MvxCommand SelectDaysCommand;
        public MvxCommand<DayOfWeek> AddDayToPastCommand;

        public SelectDaysPageViewModel(IMvxNavigationService navigationService, IAddProductToOrderService addProductToOrderService)
        {
            _navigationService = navigationService;
            _addProductToOrderService = addProductToOrderService;

            SelectDaysCommand = new MvxCommand(SelectDays, CanSelectDays);
            AddDayToPastCommand = new MvxCommand<DayOfWeek>(AddDayToPast);
        }

        private DayOfWeek? _daySelectedToCopy;
        public DayOfWeek? DaySelectedToCopy
        {
            get
            {
                return _daySelectedToCopy;
            }
            set
            {
                _daySelectedToCopy = value;
                SelectDaysCommand.RaiseCanExecuteChanged();
            }
        }

        private List<DayOfWeek> _daysToPast = new List<DayOfWeek>();
        private List<DayOfWeek> DaysToPast => _daysToPast;


        private bool CanSelectDays()
        {
            return DaySelectedToCopy != null && DaysToPast.Count > 0;
        }

        private async void SelectDays()
        {
            _addProductToOrderService.AddProductDay = (DayOfWeek)DaySelectedToCopy;
            _addProductToOrderService.ListDaysToPast = DaysToPast;
            await _navigationService.Close(this);
        }

        private void AddDayToPast(DayOfWeek day)
        {
            if (DaysToPast.Contains(day))
                DaysToPast.Remove(day);
            else
                DaysToPast.Add(day);

            SelectDaysCommand.RaiseCanExecuteChanged();
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
}
