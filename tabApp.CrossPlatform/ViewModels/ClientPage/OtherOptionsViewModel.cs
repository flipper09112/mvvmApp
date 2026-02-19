using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using tabApp.Core.Models;
using tabApp.Core.Services.Implementations.Clients;
using tabApp.Core.Services.Implementations.DB;
using tabApp.Core.Services.Implementations.Faturation;
using tabApp.Core.Services.Interfaces.Dialogs;
using tabApp.Core.Services.Interfaces.Faturation;
using tabApp.Core.ViewModels.ClientPage;
using tabApp.Core.ViewModels.ClientPage.OtherOptions;
using tabApp.Core.ViewModels.Global.Faturation;

namespace tabApp.Core.ViewModels
{
    public class OtherOptionsViewModel : BaseViewModel
    {
        private readonly IChooseClientService _chooseClientService;
        private readonly IMvxNavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly IDataBaseManagerService _dataBaseManagerService;
        private readonly IFaturationService _faturationService;
        public EventHandler ShowCalculatorEvent;

        public MvxCommand InsertNewRegistCommand;
        public MvxCommand PrintPageCommand;
        public MvxCommand ShowCalculatorCommand;
        public MvxCommand ChangeDailyOrdersCommand;
        public MvxCommand ShowCreateNoficationPageCommand;
        public MvxCommand ShowInvoiceCreatorPageCommand;

        public OtherOptionsViewModel(IChooseClientService chooseClientService, 
                                     IMvxNavigationService navigationService,
                                     IDialogService dialogService,
                                     IDataBaseManagerService dataBaseManagerService,
                                     IFaturationService faturationService)
        {
            _chooseClientService = chooseClientService;
            _navigationService = navigationService;
            _dialogService = dialogService;
            _dataBaseManagerService = dataBaseManagerService;
            _faturationService = faturationService;

            InsertNewRegistCommand = new MvxCommand(InsertNewRegist);
            PrintPageCommand = new MvxCommand(PrintPage);
            ShowCalculatorCommand = new MvxCommand(ShowCalculator);
            ChangeDailyOrdersCommand = new MvxCommand(ChangeDailyOrders);
            ShowCreateNoficationPageCommand = new MvxCommand(ShowCreateNoficationPage);
            ShowInvoiceCreatorPageCommand = new MvxCommand(ShowInvoiceCreatorPage);
        }

        private async void ShowInvoiceCreatorPage()
        {
            if(_chooseClientService.ClientSelected.NIF == null || _chooseClientService.ClientSelected.NIF == 0)
            {
                _dialogService.ShowInputDialog("Inserir NIF", "OK", SetNIFAction, false);
            }
            else
            {
                _faturationService.ClientSelected = _chooseClientService.ClientSelected;
                await _navigationService.Navigate<FaturationViewModel>();
            }
        }

        private void SetNIFAction(double nif)
        {
            if(!IsValidContrib(nif.ToString()))
            {
                _dialogService.ShowErrorDialog("Nif Invalido", "O contribuinte inserido não é valido");
            }
            else
            {
                _chooseClientService.ClientSelected.NIF = (int)nif;
                _dataBaseManagerService.SaveClient(_chooseClientService.ClientSelected, new Regist()
                {
                    DetailRegistDay = DateTime.Now,
                    Info = "Inserido o NIF (" + nif.ToString() + ")",
                    DetailType = DetailTypeEnum.UpdateNIF
                });
                ShowInvoiceCreatorPage();
            }
        }

        public bool IsValidContrib(string Contrib)
        {
            try
            {
                bool functionReturnValue = false;
                functionReturnValue = false;
                string[] s = new string[9];
                string Ss = null;
                string C = null;
                int i = 0;
                long checkDigit = 0;

                s[0] = Convert.ToString(Contrib[0]);
                s[1] = Convert.ToString(Contrib[1]);
                s[2] = Convert.ToString(Contrib[2]);
                s[3] = Convert.ToString(Contrib[3]);
                s[4] = Convert.ToString(Contrib[4]);
                s[5] = Convert.ToString(Contrib[5]);
                s[6] = Convert.ToString(Contrib[6]);
                s[7] = Convert.ToString(Contrib[7]);
                s[8] = Convert.ToString(Contrib[8]);

                if (Contrib.Length == 9)
                {
                    C = s[0];
                    if (s[0] == "1" || s[0] == "2" || s[0] == "5" || s[0] == "6" || s[0] == "9")
                    {
                        checkDigit = Convert.ToInt32(C) * 9;
                        for (i = 2; i <= 8; i++)
                        {
                            checkDigit = checkDigit + (Convert.ToInt32(s[i - 1]) * (10 - i));
                        }
                        checkDigit = 11 - (checkDigit % 11);
                        if ((checkDigit >= 10))
                            checkDigit = 0;
                        Ss = s[0] + s[1] + s[2] + s[3] + s[4] + s[5] + s[6] + s[7] + s[8];
                        if ((checkDigit == Convert.ToInt32(s[8])))
                            functionReturnValue = true;
                    }
                }
                return functionReturnValue;
            }
            catch { return false; }
        }

        private async void ShowCreateNoficationPage()
        {
            await _navigationService.Navigate<CreateNoficationViewModel>();
        }

        private async void ChangeDailyOrders()
        {
            await _navigationService.Navigate<ChangeDailyOrderViewModel>();
        }

        private void ShowCalculator()
        {
            ShowCalculatorEvent?.Invoke(null, null);
        }

        private async void PrintPage()
        {
            await _navigationService.Navigate<PrintAccountViewModel>();
        }

        private async void InsertNewRegist()
        {
            await _navigationService.Navigate<AddStoreRegistViewModel>();
        }

        public List<Option> Options
        {
            get
            {
                List<Option> options = new List<Option>();

                options.Add(new Option(PrintPageCommand, "Imprimir Conta", "ic_printer"));
                options.Add(new Option(ShowCalculatorCommand, "Abrir Calculadora", "ic_calculator"));
                options.Add(new Option(ChangeDailyOrdersCommand, "Alterar Quantidade", "ic_change"));
                if(_chooseClientService.ClientSelected.PaymentType == PaymentTypeEnum.Loja || _chooseClientService.ClientSelected.PaymentType == PaymentTypeEnum.JuntaDiasLoja)
                    options.Add(new Option(InsertNewRegistCommand, "Inserir despesa do dia", "ic_insert"));

                options.Add(new Option(ShowCreateNoficationPageCommand, "Adicionar notificação", "ic_notification"));
                options.Add(new Option(ShowInvoiceCreatorPageCommand, "Emitir Fatura", "ic_invoice"));

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
