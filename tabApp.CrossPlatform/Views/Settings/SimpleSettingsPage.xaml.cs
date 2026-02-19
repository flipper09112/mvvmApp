using tabApp.CrossPlatform.ViewModels.Main;
using tabApp.CrossPlatform.Views.Bases;

namespace tabApp.CrossPlatform.Views.Settings
{
    /// <summary>
    /// MIGRATION POC: Simple Settings Page
    /// 
    /// Demonstrates:
    /// 1. Inheriting from BaseContentPage<TViewModel>
    /// 2. Automatic ViewModel lifecycle management
    /// 3. Type-safe ViewModel access via ViewModel property
    /// 4. Dependency injection in constructor
    /// </summary>
    public partial class SimpleSettingsPage : BaseContentPage<SimpleSettingsViewModel>
    {
        public SimpleSettingsPage(SimpleSettingsViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }
    }
}

