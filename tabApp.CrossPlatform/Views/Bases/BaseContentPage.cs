using tabApp.Core.ViewModels;

namespace tabApp.CrossPlatform.Views.Bases
{
    /// <summary>
    /// Base class for all ContentPages in the application.
    /// Handles automatic ViewModel lifecycle management.
    /// 
    /// Ensures that when a page appears/disappears, the corresponding
    /// ViewModel lifecycle methods are called automatically.
    /// </summary>
    public abstract class BaseContentPage : ContentPage
    {
        protected BaseContentPage()
        {
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is BaseViewModel viewModel)
            {
                viewModel.Appearing();
                await viewModel.InitializeAsync();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (BindingContext is BaseViewModel viewModel)
            {
                viewModel.DisAppearing();
            }
        }
    }

    /// <summary>
    /// Generic base class with strongly-typed ViewModel
    /// Provides type-safe access to the ViewModel via the ViewModel property
    /// </summary>
    public abstract class BaseContentPage<TViewModel> : BaseContentPage
        where TViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets the ViewModel from the BindingContext
        /// </summary>
        public TViewModel ViewModel => (TViewModel)BindingContext;

        /// <summary>
        /// Initialize the page with the ViewModel
        /// </summary>
        protected BaseContentPage(TViewModel viewModel)
        {
            BindingContext = viewModel;
        }
    }
}

