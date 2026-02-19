using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using tabApp.Core.ViewModels;

namespace tabApp.CrossPlatform.ViewModels.Main
{
    /// <summary>
    /// MIGRATION POC: Simple Settings ViewModel using CommunityToolkit.Mvvm
    /// 
    /// This demonstrates the new pattern:
    /// - Inherit from BaseViewModel instead of MvxViewModel
    /// - Use [ObservableProperty] for properties
    /// - Use [RelayCommand] for commands
    /// - Use async/await for async operations
    /// </summary>
    public partial class SimpleSettingsViewModel : BaseViewModel
    {
        // Observable properties using source generators
        [ObservableProperty]
        private string appVersion = "1.0.0";

        [ObservableProperty]
        private bool notificationsEnabled = true;

        [ObservableProperty]
        private string selectedLanguage = "Português";

        public SimpleSettingsViewModel()
        {
            Title = "Configurações";
        }

        /// <summary>
        /// Save settings command
        /// The [RelayCommand] attribute generates a public SaveSettingsCommand
        /// </summary>
        [RelayCommand]
        private async Task SaveSettingsAsync()
        {
            IsBusy = true;
            try
            {
                // Simulate saving
                await Task.Delay(1000);
                
                // Could call services here to save settings
                // await _settingsService.SaveAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Clear cache command
        /// </summary>
        [RelayCommand]
        private async Task ClearCacheAsync()
        {
            IsBusy = true;
            try
            {
                await Task.Delay(500);
                // await _cacheService.ClearAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Reset to defaults command
        /// </summary>
        [RelayCommand]
        private async Task ResetToDefaultsAsync()
        {
            IsBusy = true;
            try
            {
                AppVersion = "1.0.0";
                NotificationsEnabled = true;
                SelectedLanguage = "Português";
                await Task.Delay(300);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public override void Appearing()
        {
            // Called when page appears
            System.Diagnostics.Debug.WriteLine("SimpleSettingsViewModel: Appearing");
        }

        public override void DisAppearing()
        {
            // Called when page disappears
            System.Diagnostics.Debug.WriteLine("SimpleSettingsViewModel: DisAppearing");
        }
    }
}

