using MvvmCross.Commands;
using MvvmCross.Navigation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.DB;
using tabApp.Core.Services.Interfaces.Deliverys;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Products;
using tabApp.Core.ViewModels.EditClient;

namespace tabApp.Core.ViewModels
{
    public class EditClientViewModel : BaseViewModel
    {
        private readonly IChooseClientService _chooseClientService;
        private readonly IDialogService _dialogService;
        private readonly IDataBaseManagerService _dataBaseManagerService;
        protected readonly IMvxNavigationService _navService;
        protected readonly IProductsManagerService _productsManagerService;
        protected readonly IAddProductToOrderService _addProductToOrderService;
        private readonly IClientsManagerService _clientsManagerService;
        private readonly IDeliverysManagerService _deliverysManagerService;

        public MvxCommand SaveChangesCommand;
        public MvxCommand GoBackCommand;
        public MvxCommand ShowCreateNotificationPageCommand;
        public MvxCommand ShowSelectDaysPageCommand;
        public MvxCommand<DayOfWeek> AddProductCommand;

        public EditClientViewModel(IChooseClientService chooseClientService, 
                                   IDialogService dialogService, 
                                   IDataBaseManagerService dataBaseManagerService, 
                                   IMvxNavigationService navService,
                                   IProductsManagerService productsManagerService, 
                                   IAddProductToOrderService addProductToOrderService,
                                   IClientsManagerService clientsManagerService,
                                   IDeliverysManagerService deliverysManagerService)
        {
            _chooseClientService = chooseClientService;
            _dialogService = dialogService;
            _dataBaseManagerService = dataBaseManagerService;
            _navService = navService;
            _productsManagerService = productsManagerService;
            _addProductToOrderService = addProductToOrderService;
            _clientsManagerService = clientsManagerService;
            _deliverysManagerService = deliverysManagerService;

            SaveChangesCommand = new MvxCommand(SaveChanges, CanSaveChanges);
            ShowSelectDaysPageCommand = new MvxCommand(ShowSelectDaysPage);
            AddProductCommand = new MvxCommand<DayOfWeek>(AddProduct);
            ShowCreateNotificationPageCommand = new MvxCommand(CreateNotificationPage);
            GoBackCommand = new MvxCommand(GoBack);
        }

        private async void GoBack()
        {
            await _navService.Close(this);
        }

        private async void CreateNotificationPage()
        {
            await _navService.Close(this);
            await _navService.Navigate<CreateNotificationsViewModel>();
        }

        private async void ShowSelectDaysPage()
        {
            await _navService.Navigate<SelectDaysPageViewModel>();
        }

        private async void AddProduct(DayOfWeek day)
        {
            _addProductToOrderService.AddProductDay = day;
            await _navService.Navigate<ChooseProductViewModel>();
        }

        private bool CanSaveChanges()
        {
            double temp;
            if (_newLocation != null && !_newLocation.Equals(Client.Address.Coordenadas))
                return true;

            foreach (var item in _profileItems)
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue))
                    return true;
            }

            foreach (var item in _adminItems)
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue))
                    return true;
            }

            foreach (var item in _segDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, NumberStyles.Number, CultureInfo.InvariantCulture, out temp))
                    return true;
            }
            foreach (var item in _terDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, NumberStyles.Number, CultureInfo.InvariantCulture, out temp))
                    return true;
            }
            foreach (var item in _quaDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, NumberStyles.Number, CultureInfo.InvariantCulture, out temp))
                    return true;
            }
            foreach (var item in _quiDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, NumberStyles.Number, CultureInfo.InvariantCulture, out temp))
                    return true;
            }
            foreach (var item in _sexDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, NumberStyles.Number, CultureInfo.InvariantCulture, out temp))
                    return true;
            }
            foreach (var item in _sabDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, NumberStyles.Number, CultureInfo.InvariantCulture, out temp))
                    return true;
            }
            foreach (var item in _domDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, NumberStyles.Number, CultureInfo.InvariantCulture, out temp))
                    return true;
            }
            if (_segDailyItem)
                return true;
            if (_terDailyItem)
                return true;
            if (_quaDailyItem)
                return true;
            if (_quiDailyItem)
                return true;
            if (_sexDailyItem)
                return true;
            if (_sabDailyItem)
                return true;
            if (_domDailyItem)
                return true;
            return false;
        }

        private async void SaveChanges()
        {
            var hasDailyChanges = SaveChangesAction();

            if(hasDailyChanges)
            {
                _dialogService.ShowChooseOptions(GetOptions());
            }
            else
                await _navService.Close(this);
        }

        private List<LongPressItem> GetOptions()
        {
            List<LongPressItem> items = new List<LongPressItem>();

            items.Add(new LongPressItem()
            {
                Name = "Ignorar",
                Command = GoBackCommand
            });

            items.Add(new LongPressItem() { 
                Name = "Criar Notificações de Aviso",
                Command = ShowCreateNotificationPageCommand
            });

            return items;
        }

        private bool SaveChangesAction()
        {
            bool isValid = true;
            string toRegist = "";
            if (_newLocation != null && !_newLocation.Equals(Client.Address.Coordenadas))
            {
                toRegist += "Localização alterada de " + Client.Address.Coordenadas + " para " + _newLocation + "\n";
                Client.UpdateLocation(_newLocation);
            }

            foreach (var item in _profileItems)
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue))
                {
                    toRegist += "Alteração de '" + item.Name + "'" + " antes: " + item.Value + " depois: " + item.NewValue + "\n";
                    isValid = SaveRegist(item);
                }
            }
            if (!isValid) return false;
            foreach (var item in _adminItems)
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue))
                {
                    toRegist += "Alteração de '" + item.Name + "'" + " antes: " + item.Value + " depois: " + item.NewValue + "\n";
                    isValid = SaveRegist(item);
                }
            }
            if (!isValid) return false;
            bool hasDailyOrderCHange = false;
            double temp;
            foreach (var item in _segDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, NumberStyles.Number, CultureInfo.InvariantCulture, out temp))
                {
                    toRegist += "Alteração das quantidades de [Segunda]\n";
                    hasDailyOrderCHange = true;
                    isValid = SaveRegist(item, _segDailyItemsList);
                    break;
                }
                else if (_segDailyItem)
                {
                    toRegist += "Alteração das quantidades de [Segunda]\n";
                    hasDailyOrderCHange = true;
                    isValid = SaveRegist(item, _segDailyItemsList);
                    break;
                }
            }
            if (!isValid) return false;
            foreach (var item in _terDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, NumberStyles.Number, CultureInfo.InvariantCulture, out temp))
                {
                    toRegist += "Alteração das quantidades de [Terça]\n";
                    isValid = SaveRegist(item, _terDailyItemsList);
                    hasDailyOrderCHange = true;
                    break;
                }
                else if (_terDailyItem)
                {
                    toRegist += "Alteração das quantidades de [Terça]\n";
                    hasDailyOrderCHange = true;
                    isValid = SaveRegist(item, _terDailyItemsList);
                    break;
                }
            }
            if (!isValid) return false;
            foreach (var item in _quaDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, NumberStyles.Number, CultureInfo.InvariantCulture, out temp))
                {
                    toRegist += "Alteração das quantidades de [Quarta]\n";
                    isValid = SaveRegist(item, _quaDailyItemsList);
                    hasDailyOrderCHange = true;
                    break;
                }
                else if (_quaDailyItem)
                {
                    toRegist += "Alteração das quantidades de [Quarta]\n";
                    hasDailyOrderCHange = true;
                    isValid = SaveRegist(item, _quaDailyItemsList);
                    break;
                }
            }
            if (!isValid) return false;
            foreach (var item in _quiDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, NumberStyles.Number, CultureInfo.InvariantCulture, out temp))
                {
                    toRegist += "Alteração das quantidades de [Quinta]\n";
                    isValid = SaveRegist(item, _quiDailyItemsList);
                    hasDailyOrderCHange = true;
                    break;
                }
                else if (_quiDailyItem)
                {
                    toRegist += "Alteração das quantidades de [Quinta]\n";
                    hasDailyOrderCHange = true;
                    isValid = SaveRegist(item, _quiDailyItemsList);
                    break;
                }
            }
            if (!isValid) return false;
            foreach (var item in _sexDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, NumberStyles.Number, CultureInfo.InvariantCulture, out temp))
                {
                    toRegist += "Alteração das quantidades de [Sexta]\n";
                    isValid = SaveRegist(item, _sexDailyItemsList);
                    hasDailyOrderCHange = true;
                    break;
                }
                else if (_sexDailyItem)
                {
                    toRegist += "Alteração das quantidades de [Sexta]\n";
                    hasDailyOrderCHange = true;
                    isValid = SaveRegist(item, _sexDailyItemsList);
                    break;
                }
            }
            if (!isValid) return false;
            foreach (var item in _sabDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, NumberStyles.Number, CultureInfo.InvariantCulture, out temp))
                {
                    toRegist += "Alteração das quantidades de [Sabado]\n";
                    isValid = SaveRegist(item, _sabDailyItemsList);
                    hasDailyOrderCHange = true;
                    break;
                }
                else if (_sabDailyItem)
                {
                    toRegist += "Alteração das quantidades de [Sabado]\n";
                    hasDailyOrderCHange = true;
                    isValid = SaveRegist(item, _sabDailyItemsList);
                    break;
                }
            }
            if (!isValid) return false;
            foreach (var item in _domDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, NumberStyles.Number, CultureInfo.InvariantCulture, out temp))
                {
                    toRegist += "Alteração das quantidades de [Domingo]\n";
                    isValid = SaveRegist(item, _domDailyItemsList);
                    hasDailyOrderCHange = true;
                    break;
                }
                else if (_domDailyItem)
                {
                    toRegist += "Alteração das quantidades de [Domingo]\n";
                    hasDailyOrderCHange = true;
                    isValid = SaveRegist(item, _domDailyItemsList);
                    break;
                }
            }
            if (!isValid) return false;

            _dataBaseManagerService.SaveClient(Client, toRegist);

            return hasDailyOrderCHange;
        }

        private bool SaveRegist(ClientProfileField item, List<ClientProfileField> dailyOrderItems = null)
        {
            try
            {
                switch (item.Type)
                {
                    case nameof(Client.Name):
                        Client.UpdateName(item.NewValue);
                        break;
                    case nameof(Client.SubName):
                        Client.UpdateSubName(item.NewValue);
                        break;
                    case nameof(Client.Address.AddressDesc):
                        Client.UpdateAddressDesc(item.NewValue);
                        break;
                    case nameof(Client.Address.NumberDoor):
                        Client.UpdateNumberDoor(int.Parse(item.NewValue));
                        break;
                    case nameof(Client.PaymentDate):
                        Client.UpdatePaymentDate(DateTime.Parse(item.NewValue));
                        break;
                    case nameof(Client.ExtraValueToPay):
                        Client.UpdateExtraValueToPay(double.Parse(item.NewValue));
                        break;
                    case nameof(Client.Delivery):
                        Client.UpdateDelivery(_deliverysManagerService.GetDeliveryByNameAndId(item.NewValue));
                        break;
                    case nameof(SegDailyItemsList):
                        Client.UpdateDailyOrder(GetnewDailyOrder(dailyOrderItems, DayOfWeek.Monday), DayOfWeek.Monday);
                        break;
                    case nameof(TerDailyItemsList):
                        Client.UpdateDailyOrder(GetnewDailyOrder(dailyOrderItems, DayOfWeek.Tuesday), DayOfWeek.Tuesday);
                        break;
                    case nameof(QuaDailyItemsList):
                        Client.UpdateDailyOrder(GetnewDailyOrder(dailyOrderItems, DayOfWeek.Wednesday), DayOfWeek.Wednesday);
                        break;
                    case nameof(QuiDailyItemsList):
                        Client.UpdateDailyOrder(GetnewDailyOrder(dailyOrderItems, DayOfWeek.Thursday), DayOfWeek.Thursday);
                        break;
                    case nameof(SexDailyItemsList):
                        Client.UpdateDailyOrder(GetnewDailyOrder(dailyOrderItems, DayOfWeek.Friday), DayOfWeek.Friday);
                        break;
                    case nameof(SabDailyItemsList):
                        Client.UpdateDailyOrder(GetnewDailyOrder(dailyOrderItems, DayOfWeek.Saturday), DayOfWeek.Saturday);
                        break;
                    case nameof(DomDailyItemsList):
                        Client.UpdateDailyOrder(GetnewDailyOrder(dailyOrderItems, DayOfWeek.Sunday), DayOfWeek.Sunday);
                        break;
                    case nameof(Client.PhoneNumber):
                        Client.UpdatePhoneNumber(item.NewValue);
                        break;
                    case nameof(Client.PaymentType):
                        Client.UpdatePaymentType(item.NewValue);
                        break;
                    default:
                        _dialogService.ShowConfirmDialog("Alguma coisa correu mal", "Este parametro nao foi salvo (" + nameof(item.Type) + ")", null);
                        break;
                }
                return true;
            } catch (Exception e)
            {
                _dialogService.ShowConfirmDialog("Alguma coisa correu mal", "Algum parametro invalido", null);
                return false;
            }
        }

        private DailyOrder GetnewDailyOrder(List<ClientProfileField> dailyOrderItems, DayOfWeek day)
        {
            List<DailyOrderDetails> allItems = new List<DailyOrderDetails>();
            double temp;
            foreach (var item in dailyOrderItems)
            {
                if (item.NewValue != null && (item.NewValue.Equals("")))
                    continue;

                double.TryParse(item.NewValue == null ? item.Value : item.NewValue, NumberStyles.Number, CultureInfo.InvariantCulture, out temp);

                allItems.Add(new DailyOrderDetails() { 
                    ProductId = item.Product.Id ,
                    Ammount = temp
                });
            }
            return new DailyOrder { DayOfWeek = day, AllItems = allItems };
        }

        private string _newLocation;
        public string NewLocation
        {
            get
            {
                if (_newLocation == null)
                    return Client.Address.Coordenadas;
                return _newLocation;
            }
            set
            {
                _newLocation = value;
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }
        public Client Client => _chooseClientService.ClientSelected;
        public List<EditFieldsEnum> TabsOptions
        {
            get
            {
                List<EditFieldsEnum> items = new List<EditFieldsEnum>();
                items.Add(EditFieldsEnum.Profile);
                items.Add(EditFieldsEnum.Localização);
                items.Add(EditFieldsEnum.Admin);
                items.Add(EditFieldsEnum.Gastos);
                return items;
            }
        }
        private List<ClientProfileField> _profileItems;
        public List<ClientProfileField> ProfileItems
        {
            get
            {
                return _profileItems;
            }
            set
            {
                _profileItems = value;
                RaisePropertyChanged(nameof(ProfileItems));
            }
        }
        private List<ClientProfileField> _adminItems;
        public List<ClientProfileField> AdminItems
        {
            get
            {
                return _adminItems;
            }
            set
            {
                _adminItems = value;
                RaisePropertyChanged(nameof(AdminItems));
            }
        }
        public override void Appearing()
        {
            ProfileItems = GetProfileItems();
            AdminItems = GetAdminItems();
            GetNewProducts();
            GetDayToCopy();
            _addProductToOrderService.Clear();
        }

        private void GetDayToCopy()
        {
            if(_addProductToOrderService.ListDaysToPast.Count > 0)
            {
                foreach(var day in _addProductToOrderService.ListDaysToPast)
                {
                    List<ClientProfileField> copy = GetDayListItems(_addProductToOrderService.AddProductDay);
                    if (day == _addProductToOrderService.AddProductDay)
                        continue;

                    switch (day)
                    {
                        case DayOfWeek.Monday:
                            //_segDailyItemsList = copy.ForEach(item => ); new List<ClientProfileField>(copy);
                            _segDailyItemsList.Clear();
                            copy.ForEach(item => _segDailyItemsList.Add(new ClientProfileField()
                            {
                                Product = item.Product,
                                Type = nameof(SegDailyItemsList),
                                Value = item.NewValue == null ? item.Value : item.NewValue,
                                RefreshSaveCommand = SaveChangesCommand
                            }));
                            //_segDailyItemsList.ForEach(item => item.Type = nameof(SegDailyItemsList));
                            _segDailyItem = true;
                            break;
                        case DayOfWeek.Tuesday:
                            _terDailyItemsList.Clear();
                            copy.ForEach(item => _terDailyItemsList.Add(new ClientProfileField()
                            {
                                Product = item.Product,
                                Type = nameof(TerDailyItemsList),
                                Value = item.NewValue == null ? item.Value : item.NewValue,
                                RefreshSaveCommand = SaveChangesCommand
                            }));
                            _terDailyItem = true;
                            break;
                        case DayOfWeek.Wednesday:
                            _quaDailyItemsList.Clear();
                            copy.ForEach(item => _quaDailyItemsList.Add(new ClientProfileField()
                            {
                                Product = item.Product,
                                Type = nameof(QuaDailyItemsList),
                                Value = item.NewValue == null ? item.Value : item.NewValue,
                                RefreshSaveCommand = SaveChangesCommand
                            }));
                            _quaDailyItem = true;
                            break;
                        case DayOfWeek.Thursday:
                            _quiDailyItemsList.Clear();
                            copy.ForEach(item => _quiDailyItemsList.Add(new ClientProfileField()
                            {
                                Product = item.Product,
                                Type = nameof(QuiDailyItemsList),
                                Value = item.NewValue == null ? item.Value : item.NewValue,
                                RefreshSaveCommand = SaveChangesCommand
                            }));
                            _quiDailyItem = true;
                            break;
                        case DayOfWeek.Friday:
                            _sexDailyItemsList.Clear();
                            copy.ForEach(item => _sexDailyItemsList.Add(new ClientProfileField()
                            {
                                Product = item.Product,
                                Type = nameof(SexDailyItemsList),
                                Value = item.NewValue == null ? item.Value : item.NewValue,
                                RefreshSaveCommand = SaveChangesCommand
                            }));
                            _sexDailyItem = true;
                            break;
                        case DayOfWeek.Saturday:
                            _sabDailyItemsList.Clear();
                            copy.ForEach(item => _sabDailyItemsList.Add(new ClientProfileField()
                            {
                                Product = item.Product,
                                Type = nameof(SabDailyItemsList),
                                Value = item.NewValue == null ? item.Value : item.NewValue,
                                RefreshSaveCommand = SaveChangesCommand
                            }));
                            _sabDailyItem = true;
                            break;
                        case DayOfWeek.Sunday:
                            _domDailyItemsList.Clear();
                            copy.ForEach(item => _domDailyItemsList.Add(new ClientProfileField()
                            {
                                Product = item.Product,
                                Type = nameof(DomDailyItemsList),
                                Value = item.NewValue == null ? item.Value : item.NewValue,
                                RefreshSaveCommand = SaveChangesCommand
                            }));
                            _domDailyItem = true;
                            break;
                    }
                }
            }
        }

        private List<ClientProfileField> GetDayListItems(DayOfWeek day)
        {
            switch(day)
            {
                case DayOfWeek.Monday:
                    return SegDailyItemsList;
                case DayOfWeek.Tuesday:
                    return TerDailyItemsList;
                case DayOfWeek.Wednesday:
                    return QuaDailyItemsList;
                case DayOfWeek.Thursday:
                    return QuiDailyItemsList;
                case DayOfWeek.Friday:
                    return SexDailyItemsList;
                case DayOfWeek.Saturday:
                    return SabDailyItemsList;
                case DayOfWeek.Sunday:
                    return DomDailyItemsList;
                default:
                    return null;
            }
        }

        private void GetNewProducts()
        {
            if (_addProductToOrderService.ProductsSelected?.Count > 0)
            {
                switch (_addProductToOrderService.AddProductDay)
                {
                    case DayOfWeek.Monday:
                        _addProductToOrderService.ProductsSelected.ForEach(item => _segDailyItemsList.Add(new ClientProfileField
                        {
                            Product = item,
                            Type = nameof(SegDailyItemsList),
                            Value = "0",
                            RefreshSaveCommand = SaveChangesCommand
                        }));
                        RaisePropertyChanged(nameof(SegDailyItemsList));
                        
                        break;
                    case DayOfWeek.Tuesday:
                        _addProductToOrderService.ProductsSelected.ForEach(item => _terDailyItemsList.Add(new ClientProfileField
                        {
                            Product = item,
                            Type = nameof(TerDailyItemsList),
                            Value = "0",
                            RefreshSaveCommand = SaveChangesCommand
                        }));
                        RaisePropertyChanged(nameof(TerDailyItemsList));
                        break;
                    case DayOfWeek.Wednesday:
                        _addProductToOrderService.ProductsSelected.ForEach(item => _quaDailyItemsList.Add(new ClientProfileField
                        {
                            Product = item,
                            Type = nameof(QuaDailyItemsList),
                            Value = "0",
                            RefreshSaveCommand = SaveChangesCommand
                        }));
                        RaisePropertyChanged(nameof(QuaDailyItemsList));
                        break;
                    case DayOfWeek.Thursday:
                        _addProductToOrderService.ProductsSelected.ForEach(item => _quiDailyItemsList.Add(new ClientProfileField
                        {
                            Product = item,
                            Type = nameof(QuiDailyItemsList),
                            Value = "0",
                            RefreshSaveCommand = SaveChangesCommand
                        }));
                        RaisePropertyChanged(nameof(QuiDailyItemsList));
                        break;
                    case DayOfWeek.Friday:
                        _addProductToOrderService.ProductsSelected.ForEach(item => _sexDailyItemsList.Add(new ClientProfileField
                        {
                            Product = item,
                            Type = nameof(SexDailyItemsList),
                            Value = "0",
                            RefreshSaveCommand = SaveChangesCommand
                        }));
                        RaisePropertyChanged(nameof(SexDailyItemsList));
                        break;
                    case DayOfWeek.Saturday:
                        _addProductToOrderService.ProductsSelected.ForEach(item => _sabDailyItemsList.Add(new ClientProfileField
                        {
                            Product = item,
                            Type = nameof(SabDailyItemsList),
                            Value = "0",
                            RefreshSaveCommand = SaveChangesCommand
                        }));
                        RaisePropertyChanged(nameof(SabDailyItemsList));
                        break;
                    case DayOfWeek.Sunday:
                        _addProductToOrderService.ProductsSelected.ForEach(item => _domDailyItemsList.Add(new ClientProfileField
                        {
                            Product = item,
                            Type = nameof(DomDailyItemsList),
                            Value = "0",
                            RefreshSaveCommand = SaveChangesCommand
                        }));
                        RaisePropertyChanged(nameof(DomDailyItemsList));
                        break;
                }
            }
        }

        private List<ClientProfileField> GetProfileItems()
        {
            List<ClientProfileField> items = new List<ClientProfileField>();
            items.Add(new ClientProfileField()
            {
                IsInt = false,
                IconName = "profileicon",
                Name = "Nome",
                Value = Client.Name,
                Type = nameof(Client.Name),
                RefreshSaveCommand = SaveChangesCommand
            });
            items.Add(new ClientProfileField()
            {
                IsInt = false,
                IconName = "ic_nickname",
                Name = "Alcunha",
                Value = Client.SubName,
                Type = nameof(Client.SubName),
                RefreshSaveCommand = SaveChangesCommand
            });
            items.Add(new ClientProfileField()
            {
                IsInt = false,
                IconName = "ic_pin_location",
                Name = "Morada",
                Value = Client.Address.AddressDesc,
                Type = nameof(Client.Address.AddressDesc),
                RefreshSaveCommand = SaveChangesCommand
            });
            items.Add(new ClientProfileField()
            {
                IsInt = true,
                IconName = "ic_door",
                Name = "Número da porta",
                Value = Client.Address.NumberDoor.ToString(),
                Type = nameof(Client.Address.NumberDoor),
                RefreshSaveCommand = SaveChangesCommand
            });
            items.Add(new ClientProfileField()
            {
                IconName = "ic_phone",
                Name = "Telemóvel",
                Value = Client.PhoneNumber,
                Type = nameof(Client.PhoneNumber),
                RefreshSaveCommand = SaveChangesCommand
            });
            return items;
        }

        private List<ClientProfileField> GetAdminItems()
        {
            List<ClientProfileField> items = new List<ClientProfileField>();
            items.Add(new ClientProfileField()
            {
                IsDate = true,
                IconName = "ic_calendar",
                Name = "Data do último pagamento",
                Value = Client.PaymentDate.ToString("dd/MM/yyyy"),
                Type = nameof(Client.PaymentDate),
                RefreshSaveCommand = SaveChangesCommand
            });

            items.Add(new ClientProfileField()
            {
                IsDouble = true,
                IconName = "ic_money",
                Name = "Valor de extras a pagar",
                Value = Client.ExtraValueToPay.ToString("N2"),
                Type = nameof(Client.ExtraValueToPay),
                RefreshSaveCommand = SaveChangesCommand
            });

            items.Add(new ClientProfileListField()
            {
                IsDouble = true,
                IconName = "ic_money",
                Name = "Tipo Pagamento",
                Value = Client.PaymentType.ToString(),
                ValueList = _clientsManagerService.GetAllPaymentsTypes(),
                Type = nameof(Client.PaymentType),
                RefreshSaveCommand = SaveChangesCommand
            });

            items.Add(new ClientProfileListField()
            {
                IsDouble = true,
                IconName = "ic_money",
                Name = "Distribuidor",
                Value = Client.Delivery.DeliveryName + " (" + Client.Delivery.DeliveryId + ")",
                ValueList = _deliverysManagerService.GetListNamesAndId(),
                Type = nameof(Client.Delivery),
                RefreshSaveCommand = SaveChangesCommand
            });

            return items;
        }

        public override void DisAppearing()
        {
        }
        #region Get Lists
        private List<ClientProfileField> _segDailyItemsList;
        private bool _segDailyItem;
        public List<ClientProfileField> SegDailyItemsList
        {
            get
            {
                if (_segDailyItemsList != null)
                    return _segDailyItemsList;

                _segDailyItemsList = new List<ClientProfileField>();

                Product product;

                foreach (var item in Client.SegDailyOrder.AllItems)
                {
                    product = _productsManagerService.GetProductById(item.ProductId);

                    if (product.Unity)
                        _segDailyItemsList.Add(
                            new ClientProfileField()
                            {
                                Product = product,
                                Type = nameof(SegDailyItemsList),
                                Value = item.Ammount.ToString("N0"),
                                RefreshSaveCommand = SaveChangesCommand
                            });
                    else
                        _segDailyItemsList.Add(
                            new ClientProfileField()
                            {
                                Product = product,
                                Type = nameof(SegDailyItemsList),
                                Value = item.Ammount.ToString("N3"),
                                RefreshSaveCommand = SaveChangesCommand
                            });
                }

                return _segDailyItemsList;
            }
        }
        private List<ClientProfileField> _terDailyItemsList;
        private bool _terDailyItem;

        public List<ClientProfileField> TerDailyItemsList
        {
            get
            {
                if (_terDailyItemsList != null)
                    return _terDailyItemsList;

                _terDailyItemsList = new List<ClientProfileField>();

                Product product;

                foreach (var item in Client.TerDailyOrder.AllItems)
                {
                    product = _productsManagerService.GetProductById(item.ProductId);

                    if (product.Unity)
                        _terDailyItemsList.Add(new ClientProfileField()
                        {
                            Product = product,
                            Type = nameof(TerDailyItemsList),
                            Value = item.Ammount.ToString("N0"),
                            RefreshSaveCommand = SaveChangesCommand
                        });
                    else
                        _terDailyItemsList.Add(new ClientProfileField()
                        {
                            Product = product,
                            Type = nameof(TerDailyItemsList),
                            Value = item.Ammount.ToString("N3"),
                            RefreshSaveCommand = SaveChangesCommand
                        });
                }

                return _terDailyItemsList;
            }
        }
        private List<ClientProfileField> _quaDailyItemsList;
        private bool _quaDailyItem;

        public List<ClientProfileField> QuaDailyItemsList
        {
            get
            {
                if (_quaDailyItemsList != null)
                    return _quaDailyItemsList;

                _quaDailyItemsList = new List<ClientProfileField>();

                Product product;

                foreach (var item in Client.QuaDailyOrder.AllItems)
                {
                    product = _productsManagerService.GetProductById(item.ProductId);

                    if (product.Unity)
                        _quaDailyItemsList.Add(new ClientProfileField()
                        {
                            Product = product,
                            Type = nameof(QuaDailyItemsList),
                            Value = item.Ammount.ToString("N0"),
                            RefreshSaveCommand = SaveChangesCommand
                        });
                    else
                        _quaDailyItemsList.Add(new ClientProfileField()
                        {
                            Product = product,
                            Type = nameof(QuaDailyItemsList),
                            Value = item.Ammount.ToString("N3"),
                            RefreshSaveCommand = SaveChangesCommand
                        });
                }

                return _quaDailyItemsList;
            }
        }
        private List<ClientProfileField> _quiDailyItemsList;
        private bool _quiDailyItem;

        public List<ClientProfileField> QuiDailyItemsList
        {
            get
            {
                if (_quiDailyItemsList != null)
                    return _quiDailyItemsList;

                _quiDailyItemsList = new List<ClientProfileField>();

                Product product;

                foreach (var item in Client.QuiDailyOrder.AllItems)
                {
                    product = _productsManagerService.GetProductById(item.ProductId);

                    if (product.Unity)
                        _quiDailyItemsList.Add(new ClientProfileField()
                        {
                            Product = product,
                            Type = nameof(QuiDailyItemsList),
                            Value = item.Ammount.ToString("N0"),
                            RefreshSaveCommand = SaveChangesCommand
                        });
                    else
                        _quiDailyItemsList.Add(new ClientProfileField()
                        {
                            Product = product,
                            Type = nameof(QuiDailyItemsList),
                            Value = item.Ammount.ToString("N3"),
                            RefreshSaveCommand = SaveChangesCommand
                        });
                }

                return _quiDailyItemsList;
            }
        }
        private List<ClientProfileField> _sexDailyItemsList;
        private bool _sexDailyItem;

        public List<ClientProfileField> SexDailyItemsList
        {
            get
            {
                if (_sexDailyItemsList != null)
                    return _sexDailyItemsList;
                _sexDailyItemsList = new List<ClientProfileField>();

                Product product;

                foreach (var item in Client.SexDailyOrder.AllItems)
                {
                    product = _productsManagerService.GetProductById(item.ProductId);

                    if (product.Unity)
                        _sexDailyItemsList.Add(new ClientProfileField()
                        {
                            Product = product,
                            Type = nameof(SexDailyItemsList),
                            Value = item.Ammount.ToString("N0"),
                            RefreshSaveCommand = SaveChangesCommand
                        });
                    else
                        _sexDailyItemsList.Add(new ClientProfileField()
                        {
                            Product = product,
                            Type = nameof(SexDailyItemsList),
                            Value = item.Ammount.ToString("N3"),
                            RefreshSaveCommand = SaveChangesCommand
                        });
                }

                return _sexDailyItemsList;
            }
        }
        private List<ClientProfileField> _sabDailyItemsList;
        private bool _sabDailyItem;

        public List<ClientProfileField> SabDailyItemsList
        {
            get
            {
                if (_sabDailyItemsList != null)
                    return _sabDailyItemsList;
                _sabDailyItemsList = new List<ClientProfileField>();

                Product product;

                foreach (var item in Client.SabDailyOrder.AllItems)
                {
                    product = _productsManagerService.GetProductById(item.ProductId);

                    if (product.Unity)
                        _sabDailyItemsList.Add(new ClientProfileField()
                        {
                            Product = product,
                            Type = nameof(SabDailyItemsList),
                            Value = item.Ammount.ToString("N0"),
                            RefreshSaveCommand = SaveChangesCommand
                        });
                    else
                        _sabDailyItemsList.Add(new ClientProfileField()
                        {
                            Product = product,
                            Type = nameof(SabDailyItemsList),
                            Value = item.Ammount.ToString("N3"),
                            RefreshSaveCommand = SaveChangesCommand
                        });
                }

                return _sabDailyItemsList;
            }
        }
        private List<ClientProfileField> _domDailyItemsList;
        private bool _domDailyItem;

        public List<ClientProfileField> DomDailyItemsList
        {
            get
            {
                if (_domDailyItemsList != null)
                    return _domDailyItemsList;
                _domDailyItemsList = new List<ClientProfileField>();

                Product product;

                foreach (var item in Client.DomDailyOrder.AllItems)
                {
                    product = _productsManagerService.GetProductById(item.ProductId);

                    if (product.Unity)
                        _domDailyItemsList.Add(new ClientProfileField()
                        {
                            Product = product,
                            Type = nameof(DomDailyItemsList),
                            Value = item.Ammount.ToString("N0"),
                            RefreshSaveCommand = SaveChangesCommand
                        });
                    else
                        _domDailyItemsList.Add(new ClientProfileField()
                        {
                            Product = product,
                            Type = nameof(DomDailyItemsList),
                            Value = item.Ammount.ToString("N3"),
                            RefreshSaveCommand = SaveChangesCommand
                        });
                }

                return _domDailyItemsList;
            }
        }
        #endregion
    }

    public enum EditFieldsEnum
    {
        Profile,
        Localização,
        Admin,
        Gastos
    }
}

