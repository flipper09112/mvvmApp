using MvvmCross.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using tabApp.Core.Helpers;
using tabApp.Core.Models;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.Deliverys;
using tabApp.Core.Services.Interfaces.Dialogs;

namespace tabApp.Core.ViewModels.Main
{
    public class SettingsViewModel : BaseViewModel
    {
        private IDeliverysManagerService _deliverysManagerService;
        private IClientsManagerService _clientsManagerService;
        private IDialogService _dialogService;

        public List<SettingItem> SettingsList { get; set; }

        public EventHandler ChooseDeliveryEvent { get; set; }
        public EventHandler ReloadUIEvent { get; set; }

        public MvxCommand ChooseDeliveryCommand { get; private set; }
        public MvxCommand<int> ChooseDeliveryIndexCommand { get; private set; }
        public string[] DeliveriesList
        {
            get
            {
                var list = new List<string>();

                ((SingleChoiceSettingItem<Delivery>)SettingsList[1]).Options.ForEach(item => list.Add(item.DeliveryName + " (" + item.DeliveryId + ")"));

                return list.ToArray();
            }
        }

        public SettingsViewModel(IDeliverysManagerService deliverysManagerService,
                                 IClientsManagerService clientsManagerService,
                                 IDialogService dialogService)
        {
            _deliverysManagerService = deliverysManagerService;
            _clientsManagerService = clientsManagerService;
            _dialogService = dialogService;

            ChooseDeliveryCommand = new MvxCommand(ChooseDelivery);
            ChooseDeliveryIndexCommand = new MvxCommand<int>(ChooseDeliveryIndex);
        }

        private async void ChooseDeliveryIndex(int deliveryIndex)
        {
            var delivery = ((SingleChoiceSettingItem<Delivery>)SettingsList[1]).Options[deliveryIndex];

            await SecureStorageHelper.SaveKeyAsync(SecureStorageHelper.DeliveryId, delivery.DeliveryId == -1 ? SecureStorageHelper.DeliveryIdAdmin : delivery.DeliveryId.ToString());

            ((SingleChoiceSettingItem<Delivery>)SettingsList[1]).CurrentValue = delivery.DeliveryName + " (" + delivery.DeliveryId + ")";
            _clientsManagerService.DeliveryId = delivery.DeliveryId == -1 ? SecureStorageHelper.DeliveryIdAdmin : delivery.DeliveryId.ToString();
            ReloadUIEvent?.Invoke(null, null);
        }

        private void ChooseDelivery()
        {
            ChooseDeliveryEvent.Invoke(null, null);
        }

        public override void Appearing()
        {
            SettingsList = GetSettingItemsList();
        }

        private List<SettingItem> GetSettingItemsList()
        {
            var list = new List<SettingItem>();

            list.Add(new SettingItemTitle
            {
                Type = SettingItemType.Title,
                Title = "Listagem de Clientes"
            });

            var deliveryOptions = new List<Delivery>();
            deliveryOptions.Add(new Delivery() { DeliveryName = "Admin", DeliveryId = -1 });
            deliveryOptions.AddRange(_deliverysManagerService.Deliveries);
            list.Add(new SingleChoiceSettingItem<Delivery>() { 
                Type = SettingItemType.Delivery,
                CurrentValue = _deliverysManagerService.GetDeliveryId(_clientsManagerService.DeliveryId).DeliveryName + " (" + _deliverysManagerService.GetDeliveryId(_clientsManagerService.DeliveryId).DeliveryId + ")",
                Options = deliveryOptions,
                Command = ChooseDeliveryCommand
            });

            return list;
        }

        public override void DisAppearing()
        {
        }
    }

    public class SettingItem
    {
        public SettingItemType Type { get; set; }
        public ICommand Command { get; set; }
    }

    public enum SettingItemType
    {
        Delivery,
        Title
    }
    public class SettingItemTitle : SettingItem
    {
        public string Title { get; set; }
    }

    public class SingleChoiceSettingItem<T> : SettingItem
    {
        public string CurrentValue { get; set; }
        public List<T> Options { get; set; }
    }
}
