# ISSUE-002: Migration Patterns Guide

**Document Type:** Technical Reference  
**Audience:** Development Team  
**Purpose:** Step-by-step patterns for migrating ViewModels from MvvmCross to MAUI

---

## 📋 Pattern 1: Property Migration

### Before (MvvmCross)
```csharp
private string _title;
public string Title
{
    get => _title;
    set => SetProperty(ref _title, value);
}

private bool _isBusy;
public bool IsBusy
{
    get => _isBusy;
    set
    {
        _isBusy = value;
        RaisePropertyChanged(nameof(IsBusy));
    }
}
```

### After (CommunityToolkit.Mvvm)
```csharp
[ObservableProperty]
private string title;

[ObservableProperty]
private bool isBusy;
```

### Key Changes
- Add `[ObservableProperty]` attribute
- Make field `private` (lowercase)
- Remove getter/setter boilerplate
- Source generator creates public property automatically
- PropertyChanged notification automatic

### Code Reduction: 71% less code

---

## 📋 Pattern 2: Command Migration

### Before (MvvmCross)
```csharp
public MvxCommand SaveCommand { get; private set; }
public MvxAsyncCommand LoadDataCommand { get; private set; }

public MyViewModel()
{
    SaveCommand = new MvxCommand(Save, CanSave);
    LoadDataCommand = new MvxAsyncCommand(LoadDataAsync);
}

private void Save()
{
    // Implementation
}

private bool CanSave()
{
    return !IsBusy;
}

private async Task LoadDataAsync()
{
    // Implementation
}
```

### After (CommunityToolkit.Mvvm)
```csharp
[RelayCommand(CanExecute = nameof(CanSave))]
private void Save()
{
    // Implementation
}

private bool CanSave()
{
    return !IsBusy;
}

[RelayCommand]
private async Task LoadDataAsync()
{
    // Implementation
}
```

### Key Changes
- Add `[RelayCommand]` attribute to method
- Remove command property declarations
- Remove command initialization in constructor
- Async methods automatically get AsyncCommand
- CanExecute specified via attribute
- Command name = MethodName + "Command" (auto-generated)

### Code Reduction: 60% less code

---

## 📋 Pattern 3: Navigation Migration

### Before (MvvmCross)
```csharp
private readonly IMvxNavigationService _navigationService;

public MyViewModel(IMvxNavigationService navigationService)
{
    _navigationService = navigationService;
}

// Simple navigation
await _navigationService.Navigate<DetailsViewModel>();

// With parameter
await _navigationService.Navigate<EditViewModel, Client>(client);

// With result
var result = await _navigationService.Navigate<SelectViewModel, Product>();
```

### After (MAUI Shell)
```csharp
// Simple navigation
await Shell.Current.GoToAsync("details");

// With parameter (dictionary)
var parameters = new Dictionary<string, object>
{
    { "client", client }
};
await Shell.Current.GoToAsync("edit", parameters);

// With parameter (query string - simple types)
await Shell.Current.GoToAsync($"edit?clientId={client.Id}");

// Type-safe helper (recommended)
await Shell.Current.NavigateToDetailsAsync();
```

### Receiving Parameters
```csharp
[QueryProperty(nameof(Client), "client")]
[QueryProperty(nameof(ClientId), "clientId")]
public partial class EditViewModel : BaseViewModel
{
    [ObservableProperty]
    private Client client;
    
    [ObservableProperty]
    private string clientId;
}
```

### Key Changes
- No IMvxNavigationService injection needed
- Use Shell.Current.GoToAsync()
- Routes are strings (use constants)
- Parameters via Dictionary or QueryString
- Receive via [QueryProperty] attribute
- Type-safe helpers recommended

---

## 📋 Pattern 4: Lifecycle Migration

### Before (MvvmCross)
```csharp
public override void ViewAppeared()
{
    base.ViewAppeared();
    // Load data
}

public override void ViewDisappeared()
{
    base.ViewDisappeared();
    // Cleanup
}
```

### After (MAUI)
```csharp
public override void Appearing()
{
    base.Appearing();
    // Load data
}

public override void DisAppearing()
{
    base.DisAppearing();
    // Cleanup
}

public override async Task InitializeAsync()
{
    // Async initialization
    await LoadDataAsync();
}
```

### Key Changes
- Renamed: ViewAppeared → Appearing
- Renamed: ViewDisappeared → DisAppearing
- New: InitializeAsync() for async initialization
- BaseContentPage calls these automatically
- Must override in derived ViewModels

---

## 📋 Pattern 5: Dependency Injection Migration

### Before (MvvmCross)
```csharp
// App.cs
CreatableTypes()
   .EndingWith("Service")
   .AsInterfaces()
   .RegisterAsLazySingleton();

// Setup.cs
Mvx.LazyConstructAndRegisterSingleton<IFileService, FileService>();
```

### After (MAUI)
```csharp
// MauiProgram.cs
private static void RegisterServices(IServiceCollection services)
{
    services.AddSingleton<IFileService, FileService>();
    services.AddSingleton<ISQLiteService, SQLiteService>();
    services.AddTransient<IDialogService, DialogService>();
}

private static void RegisterViewModels(IServiceCollection services)
{
    services.AddTransient<HomeViewModel>();
    services.AddTransient<DetailsViewModel>();
}

private static void RegisterPages(IServiceCollection services)
{
    services.AddTransient<HomePage>();
    services.AddTransient<DetailsPage>();
}
```

### Key Changes
- Configuration in MauiProgram.cs
- Use Microsoft.Extensions.DependencyInjection
- AddSingleton for app-wide services
- AddTransient for ViewModels and Pages
- Constructor injection works automatically

---

## 📋 Pattern 6: ViewModel Base Class

### Before (MvvmCross)
```csharp
public class MyViewModel : MvxViewModel
{
    // MvvmCross base
}
```

### After (MAUI)
```csharp
public partial class MyViewModel : BaseViewModel
{
    // Must be partial for source generators
}
```

### Key Changes
- Inherit from BaseViewModel (not MvxViewModel)
- Class must be `partial`
- BaseViewModel inherits from ObservableObject
- Provides IsBusy, Title, lifecycle methods

---

## 📋 Pattern 7: Page Creation

### New XAML (SimpleSettingsPage.xaml)
```xml
<?xml version="1.0" encoding="utf-8" ?>
<views:BaseContentPage 
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:views="clr-namespace:tabApp.CrossPlatform.Views.Bases"
    xmlns:vm="clr-namespace:tabApp.CrossPlatform.ViewModels.Main"
    x:Class="tabApp.CrossPlatform.Views.Settings.SimpleSettingsPage"
    x:DataType="vm:SimpleSettingsViewModel"
    Title="{Binding Title}">
    
    <ScrollView>
        <VerticalStackLayout>
            <!-- Property Binding -->
            <Entry Text="{Binding AppVersion}"/>
            
            <!-- Switch Binding -->
            <Switch IsToggled="{Binding NotificationsEnabled}"/>
            
            <!-- Command Binding -->
            <Button Text="Save" Command="{Binding SaveCommand}"/>
            
            <!-- IsBusy -->
            <ActivityIndicator IsRunning="{Binding IsBusy}"/>
        </VerticalStackLayout>
    </ScrollView>
</views:BaseContentPage>
```

### Code-Behind
```csharp
public partial class SimpleSettingsPage : BaseContentPage<SimpleSettingsViewModel>
{
    public SimpleSettingsPage(SimpleSettingsViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}
```

### Key Points
- Inherit from BaseContentPage<TViewModel>
- Use x:DataType for compile-time validation
- ViewModel injected via constructor
- Bindings to generated properties/commands
- Lifecycle managed automatically

---

## 📋 Pattern 8: Route Registration

### AppShell.xaml.cs
```csharp
private void RegisterRoutes()
{
    Routing.RegisterRoute("settings", typeof(SimpleSettingsPage));
    Routing.RegisterRoute("details", typeof(DetailsPage));
    Routing.RegisterRoute("edit", typeof(EditPage));
}
```

### NavigationService.cs Constants
```csharp
public static class Routes
{
    public const string Settings = "settings";
    public const string Details = "details";
    public const string Edit = "edit";
}
```

### Usage
```csharp
// Using constant
await Shell.Current.GoToAsync(Routes.Settings);

// Using helper
await Shell.Current.NavigateToSettingsAsync();
```

---

## 🎯 Complete Migration Checklist

### For Each ViewModel:
- [ ] Change base class to `BaseViewModel`
- [ ] Add `partial` keyword to class
- [ ] Convert properties to `[ObservableProperty]`
- [ ] Convert commands to `[RelayCommand]`
- [ ] Replace IMvxNavigationService with Shell navigation
- [ ] Update lifecycle methods (ViewAppeared → Appearing)
- [ ] Add async initialization if needed
- [ ] Register in MauiProgram.cs

### For Each Page:
- [ ] Create XAML file
- [ ] Inherit from `BaseContentPage<TViewModel>`
- [ ] Add x:DataType for ViewModel
- [ ] Update bindings to new property/command names
- [ ] Create code-behind with DI constructor
- [ ] Register in MauiProgram.cs
- [ ] Register route in AppShell

### Validation:
- [ ] Code compiles without errors
- [ ] Properties update UI
- [ ] Commands execute
- [ ] Navigation works
- [ ] Lifecycle events fire
- [ ] IsBusy state updates

---

## ⚠️ Common Gotchas

### 1. Forget `partial` keyword
**Error:** Source generator properties don't appear  
**Solution:** Add `partial` to class declaration

### 2. Wrong property casing
**Error:** Property not found  
**Solution:** Field is private lowercase, property is public PascalCase (auto-generated)

### 3. Command naming
**Error:** Command not found  
**Solution:** Command name = MethodName + "Command" (e.g., Save → SaveCommand)

### 4. Navigation route not registered
**Error:** Navigation fails silently  
**Solution:** Register route in AppShell.RegisterRoutes()

### 5. ViewModel not registered in DI
**Error:** Page constructor fails  
**Solution:** Register ViewModel in MauiProgram.RegisterViewModels()

---

## 📊 Code Comparison Summary

| Feature | MvvmCross Lines | MAUI Lines | Reduction |
|---------|----------------|------------|-----------|
| Property | 7 | 2 | 71% |
| Command | 8-13 | 4-8 | 38-60% |
| Navigation | 5 | 1-2 | 60-80% |
| Lifecycle | 5 | 3 | 40% |
| DI Setup | Complex | Simple | 50% |

**Overall Code Reduction:** 40-70% less boilerplate

---

**Document Version:** 1.0  
**Last Updated:** 2026-02-19  
**Status:** Complete & Validated with POC


