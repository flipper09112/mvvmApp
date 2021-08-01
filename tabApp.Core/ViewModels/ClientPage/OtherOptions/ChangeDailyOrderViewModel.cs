using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Interfaces.Clients;
using tabApp.Core.Services.Interfaces.DB;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Products;

namespace tabApp.Core.ViewModels.ClientPage.OtherOptions
{
    public class ChangeDailyOrderViewModel : BaseViewModel
    {
        protected readonly IMvxNavigationService _navService;
        protected readonly IAddProductToOrderService _addProductToOrderService;
        protected readonly IProductsManagerService _productsManagerService;
        protected readonly IChooseClientService _chooseClientService;
        protected readonly IDialogService _dialogService;
        protected readonly IDataBaseManagerService _dataBaseManagerService;
        protected readonly IAmmountToPayService _ammountToPayService;

        public MvxCommand ShowSelectDaysPageCommand;
        public MvxCommand<DayOfWeek> AddProductCommand;
        public MvxCommand SaveChangesCommand;
        public MvxCommand DatePickerDialogCommand;

        public EventHandler GoBack2Times;

        public ChangeDailyOrderViewModel(IMvxNavigationService navService, 
                                         IAddProductToOrderService addProductToOrderService, 
                                         IProductsManagerService productsManagerService, 
                                         IChooseClientService chooseClientService,
                                         IDialogService dialogService, 
                                         IAmmountToPayService ammountToPayService,
                                         IDataBaseManagerService dataBaseManagerService)
        {
            _navService = navService;
            _addProductToOrderService = addProductToOrderService;
            _productsManagerService = productsManagerService;
            _chooseClientService = chooseClientService;
            _dialogService = dialogService;
            _dataBaseManagerService = dataBaseManagerService;
            _ammountToPayService = ammountToPayService;

            ShowSelectDaysPageCommand = new MvxCommand(ShowSelectDaysPage);
            DatePickerDialogCommand = new MvxCommand(DatePickerDialog);
            AddProductCommand = new MvxCommand<DayOfWeek>(AddProduct);
            SaveChangesCommand = new MvxCommand(SaveChanges, CanSaveChanges);
        }

        private void DatePickerDialog()
        {
            _dialogService.ShowDatePickerDialog(SetDate, false);
        }

        private void SetDate(DateTime obj)
        {
            DateTime = obj;
        }

        private bool CanSaveChanges()
        {
            if (Client.PaymentDate.Date > DateTime.Date)
                return false;

            foreach (var item in _segDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, out double value))
                    return true;
            }
            foreach (var item in _terDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, out double value))
                    return true;
            }
            foreach (var item in _quaDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, out double value))
                    return true;
            }
            foreach (var item in _quiDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, out double value))
                    return true;
            }
            foreach (var item in _sexDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, out double value))
                    return true;
            }
            foreach (var item in _sabDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, out double value))
                    return true;
            }
            foreach (var item in _domDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, out double value))
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

        private void SaveChanges()
        {
            UpdateClient();
            string toRegist = "Alteração de quantidades a partir do dia " + DateTime.ToString("dd/MM/yyyy") + "\n\n";
            bool isValid = true;
            bool hasDailyOrderCHange = false;
            foreach (var item in _segDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, out double value))
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
            if (!isValid) return;
            foreach (var item in _terDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, out double value))
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
            if (!isValid) return;
            foreach (var item in _quaDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, out double value))
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
            if (!isValid) return;
            foreach (var item in _quiDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, out double value))
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
            if (!isValid) return;
            foreach (var item in _sexDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, out double value))
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
            if (!isValid) return;
            foreach (var item in _sabDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, out double value))
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
            if (!isValid) return;
            foreach (var item in _domDailyItemsList ?? new List<ClientProfileField>())
            {
                if (item.NewValue != null && !item.Value.Equals(item.NewValue) && !item.NewValue.Equals("") && double.TryParse(item.NewValue, out double value))
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
            if (!isValid) return;

            _dataBaseManagerService.SaveClient(Client, toRegist);
            GoBack2Times?.Invoke(null, null);
        }

        private void UpdateClient()
        {
            double extra = _ammountToPayService.CalculateUntilDate(Client, DateTime.AddDays(-1));
            Client.UpdatePaymentDate(DateTime.AddDays(-1));
            Client.UpdateExtraValueToPay(extra);
        }

        private bool SaveRegist(ClientProfileField item, List<ClientProfileField> dailyOrderItems = null)
        {
            try
            {
                switch (item.Type)
                {
                    
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
                    default:
                        _dialogService.ShowConfirmDialog("Alguma coisa correu mal", "Este parametro nao foi salvo (" + nameof(item.Type) + ")", null);
                        break;
                }
                return true;
            }
            catch (Exception e)
            {
                _dialogService.ShowConfirmDialog("Alguma coisa correu mal", "Algum parametro invalido", null);
                return false;
            }
        }
        private DailyOrder GetnewDailyOrder(List<ClientProfileField> dailyOrderItems, DayOfWeek day)
        {
            List<DailyOrderDetails> allItems = new List<DailyOrderDetails>();
            foreach (var item in dailyOrderItems)
            {
                if (item.NewValue != null && (item.NewValue.Equals("")))
                    continue;

                allItems.Add(new DailyOrderDetails { ProductId = item.Product.Id, Ammount = double.Parse(item.NewValue == null ? item.Value : item.NewValue) });
            }
            return new DailyOrder() { DayOfWeek = day, AllItems = allItems};
        }

        public Client Client => _chooseClientService.ClientSelected;
        private async void AddProduct(DayOfWeek day)
        {
            _addProductToOrderService.AddProductDay = day;
            await _navService.Navigate<ChooseProductViewModel>();
        }

        private async void ShowSelectDaysPage()
        {
            await _navService.Navigate<SelectDaysPageViewModel>();
        }

        private DateTime? _dateTime;
        public DateTime DateTime
        {
            get
            {
                if(_dateTime == null)
                {
                    _dateTime = DateTime.Today;
                }
                return (DateTime)_dateTime;
            }
            set
            {
                _dateTime = value;
                RaisePropertyChanged(nameof(DateTime));
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
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
        public override void Appearing()
        {
            GetNewProducts();
            GetDayToCopy();
            _addProductToOrderService.Clear();
        }
        private void GetDayToCopy()
        {
            if (_addProductToOrderService.ListDaysToPast.Count > 0)
            {
                foreach (var day in _addProductToOrderService.ListDaysToPast)
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
            switch (day)
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

        public override void DisAppearing()
        {
        }
    }
}
