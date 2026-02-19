﻿// ==============================================================================
// TODO: ISSUE-002 - This file is obsolete and will be replaced
// This is a Xamarin.Android + MvvmCross Application class
// MAUI uses App.xaml.cs and MauiProgram.cs
// DO NOT USE THIS FILE IN MAUI PROJECT
// ==============================================================================

#if FALSE // Disabled - Android/MvvmCross specific

using Android.App;
using Android.Runtime;
using Autofac;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Views;
using System;
using tabApp.Core;
using tabApp.Core.Services.Interfaces;
using tabApp.Services;

namespace tabApp
{
    [Application]
    class MainApplication : MvxAppCompatApplication<Setup, App>
    {
        public MainApplication()
        {
        }

        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
    }
}

#endif // FALSE - End of disabled Android/MvvmCross code
