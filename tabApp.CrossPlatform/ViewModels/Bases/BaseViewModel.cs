﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace tabApp.Core.ViewModels
{
    /// <summary>
    /// Base class for all ViewModels in the application.
    /// Provides common functionality like IsBusy, lifecycle events, etc.
    /// 
    /// MIGRATION NOTE: Migrated from MvvmCross MvxViewModel to 
    /// CommunityToolkit.Mvvm's ObservableObject for .NET MAUI compatibility.
    /// </summary>
    public abstract partial class BaseViewModel : ObservableObject
    {
        /// <summary>
        /// Indicates if the ViewModel is busy (loading data, processing, etc.)
        /// When true, typically disables user interaction
        /// </summary>
        [ObservableProperty]
        private bool isBusy;

        /// <summary>
        /// The title of the current page/view
        /// </summary>
        [ObservableProperty]
        private string title = string.Empty;

        protected BaseViewModel()
        {
        }

        /// <summary>
        /// Called when the page/view appears on screen
        /// Override in derived classes for custom logic
        /// </summary>
        public virtual void Appearing()
        {
            // Override in derived classes
        }

        /// <summary>
        /// Called when the page/view disappears from screen
        /// Override in derived classes for cleanup logic
        /// </summary>
        public virtual void DisAppearing()
        {
            // Override in derived classes
        }

        /// <summary>
        /// Initialize the ViewModel asynchronously
        /// Called after ViewModel is created and bound to view
        /// </summary>
        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Generic base class for ViewModels that receive and return typed parameters
    /// Used for modal/detail screens that pass data back to caller
    /// </summary>
    public abstract partial class BaseViewModel<TParameter, TResult> : BaseViewModel
        where TParameter : class
        where TResult : class
    {
        /// <summary>
        /// Called when the ViewModel receives a parameter
        /// </summary>
        public virtual void OnParameterReceived(TParameter parameter)
        {
            // Override in derived classes
        }

        /// <summary>
        /// Called to get the result before closing the page
        /// </summary>
        public virtual TResult GetResult()
        {
            return null;
        }
    }
}
