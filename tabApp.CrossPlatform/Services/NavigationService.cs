namespace tabApp.CrossPlatform.Services
{
    /// <summary>
    /// Route constants for type-safe navigation.
    /// Using constants instead of magic strings reduces typos and makes refactoring easier.
    /// </summary>
    public static class Routes
    {
        // Main routes (in visual hierarchy)
        public const string Home = "//home";

        // Detail routes (registered programmatically)
        public const string Settings = "settings";
        public const string Details = "details";
        public const string Edit = "edit";

        // TODO: Add more routes as pages are migrated
    }

    /// <summary>
    /// Navigation helper extensions for type-safe navigation.
    /// These helpers wrap Shell.GoToAsync to provide a cleaner API.
    /// </summary>
    public static class NavigationExtensions
    {
        /// <summary>
        /// Navigate to Home page
        /// </summary>
        public static Task NavigateToHomeAsync(this Shell shell)
        {
            return shell.GoToAsync(Routes.Home);
        }

        /// <summary>
        /// Navigate to Settings page
        /// </summary>
        public static Task NavigateToSettingsAsync(this Shell shell)
        {
            return shell.GoToAsync(Routes.Settings);
        }

        /// <summary>
        /// Navigate to Details page with parameter
        /// </summary>
        public static Task NavigateToDetailsAsync<T>(this Shell shell, string paramName, T parameter)
        {
            var parameters = new Dictionary<string, object>
            {
                { paramName, parameter }
            };
            return shell.GoToAsync(Routes.Details, parameters);
        }

        /// <summary>
        /// Navigate back (pop current page)
        /// </summary>
        public static Task NavigateBackAsync(this Shell shell)
        {
            return shell.GoToAsync("..");
        }

        /// <summary>
        /// Navigate to absolute route (uses // prefix)
        /// </summary>
        public static Task NavigateToAbsoluteAsync(this Shell shell, string route)
        {
            return shell.GoToAsync($"//{route}");
        }

        // TODO: Add more helper methods as needed
    }
}

