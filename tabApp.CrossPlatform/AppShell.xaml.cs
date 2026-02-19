﻿using tabApp.CrossPlatform.Views.Settings;

namespace tabApp.CrossPlatform;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        RegisterRoutes();
    }

    /// <summary>
    /// Register routes for pages not in the visual hierarchy.
    /// Routes allow navigation via Shell.Current.GoToAsync("routeName")
    /// </summary>
    private void RegisterRoutes()
    {
        // POC: Register SimpleSettingsPage
        Routing.RegisterRoute("settings", typeof(SimpleSettingsPage));

        // TODO: Register routes as pages are migrated
        // Example:
        // Routing.RegisterRoute("details", typeof(DetailsPage));
    }
}