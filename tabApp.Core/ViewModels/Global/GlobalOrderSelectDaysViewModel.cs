using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Orders;

namespace tabApp.Core.ViewModels.Global
{
    public class GlobalOrderSelectDaysViewModel : BaseViewModel
    {
        private IDialogService _dialogService;
        private IGlobalOrderFilterService _globalOrderFilterService;
        private IOrdersManagerService _ordersManagerService;

        private DateTime _lastDay;
        private DayType _dayTypeSelected;
        private DateTime _firstDay;

        public EventHandler GoBack;
        public MvxCommand<DayType> SelectDateCommand;
        public MvxCommand ConfirmCommand;
        
        public GlobalOrderSelectDaysViewModel(IDialogService dialogService,
                                              IGlobalOrderFilterService globalOrderFilterService,
                                              IOrdersManagerService ordersManagerService)
        {
            _dialogService = dialogService;
            _globalOrderFilterService = globalOrderFilterService;
            _ordersManagerService = ordersManagerService;

            _firstDay = DateTime.Today.AddDays(1);
            _lastDay = DateTime.Today.AddDays(1);

            SelectDateCommand = new MvxCommand<DayType>(SelectDate);
            ConfirmCommand = new MvxCommand(Confirm, CanConfirm);
        }

        private bool CanConfirm()
        {
            return FirstDay.Date <= LastDay.Date;
        }

        private void Confirm()
        {
            IsBusy = true;
            GoBack?.Invoke(null, null);
            if(FirstDay.Date == LastDay.Date)
                _globalOrderFilterService.IsActive = false;
            else
            {
                _globalOrderFilterService.IsActive = true;
                _globalOrderFilterService.ProductsList = _ordersManagerService.GetTotalOrder(FirstDay, LastDay);
            }
            IsBusy = false;
        }

        public DateTime FirstDay
        {
            get
            {
                return _firstDay;
            }
            set
            {
                _firstDay = value;
                ConfirmCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(FirstDay));
            }
        }


        public DateTime LastDay
        {
            get
            {
                return _lastDay;
            }
            set
            {
                _lastDay = value;
                ConfirmCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(LastDay));
            }
        }

        private void SelectDate(DayType dayType)
        {
            _dayTypeSelected = dayType;
            _dialogService.ShowDatePickerDialog(UpdateDateSelected);
        }

        private void UpdateDateSelected(DateTime obj)
        {
            switch(_dayTypeSelected)
            {
                case DayType.First:
                    FirstDay = obj;
                    break;
                case DayType.Last:
                    LastDay = obj;
                    break;
            }
        }

        public override void Appearing()
        {
        }

        public override void DisAppearing()
        {
        }
    }
    public enum DayType
    {
        First,
        Last
    }
}
