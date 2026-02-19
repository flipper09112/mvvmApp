# ISSUE-002: Action Plan - Architecture Assessment

**Phase:** Assessment (5 days)  
**Status:** READY TO START  
**Priority:** P0 - Critical

---

## 📅 Timeline Overview

| Day | Phase | Tasks | Deliverables |
|-----|-------|-------|--------------|
| 1 | Preparation | Setup, base classes | BaseViewModel, BaseContentPage |
| 2-3 | POC Implementation | Migrate sample ViewModel | Working POC |
| 4 | Documentation | Create guides | Migration patterns |
| 5 | Review & Training | Team validation | Approval to proceed |

---

## 📋 Day 1: Preparation & Foundation

### Morning (4h)

#### Task 1.1: Install CommunityToolkit.Mvvm (30m)
**Action:**
```bash
cd tabApp.CrossPlatform
dotnet add package CommunityToolkit.Mvvm --version 8.2.2
```

**Verification:**
- [ ] Package appears in .csproj
- [ ] No version conflicts
- [ ] `dotnet restore` succeeds

---

#### Task 1.2: Create BaseViewModel (1.5h)
**Location:** `tabApp.CrossPlatform/ViewModels/Bases/BaseViewModel.cs`

**Code:**
```csharp
using CommunityToolkit.Mvvm.ComponentModel;

namespace tabApp.CrossPlatform.ViewModels.Bases
{
    /// <summary>
    /// Base class for all ViewModels in the application.
    /// Provides common functionality like IsBusy, lifecycle events, etc.
    /// </summary>
    public abstract partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isBusy;
        
        [ObservableProperty]
        private string title = string.Empty;
        
        /// <summary>
        /// Called when the page/view appears
        /// </summary>
        public virtual void OnAppearing()
        {
            // Override in derived classes
        }
        
        /// <summary>
        /// Called when the page/view disappears
        /// </summary>
        public virtual void OnDisappearing()
        {
            // Override in derived classes
        }
        
        /// <summary>
        /// Initialize the ViewModel (called after construction)
        /// </summary>
        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
```

**Tests:**
- [ ] Compiles without errors
- [ ] `IsBusy` property auto-generated
- [ ] `Title` property auto-generated
- [ ] Virtual methods present

---

#### Task 1.3: Create BaseContentPage (1h)
**Location:** `tabApp.CrossPlatform/Views/Bases/BaseContentPage.cs`

**Code:**
```csharp
using tabApp.CrossPlatform.ViewModels.Bases;

namespace tabApp.CrossPlatform.Views.Bases
{
    /// <summary>
    /// Base class for all ContentPages in the application.
    /// Handles automatic ViewModel lifecycle management.
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
                viewModel.OnAppearing();
                await viewModel.InitializeAsync();
            }
        }
        
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            
            if (BindingContext is BaseViewModel viewModel)
            {
                viewModel.OnDisappearing();
            }
        }
    }
    
    /// <summary>
    /// Generic base class with strongly-typed ViewModel
    /// </summary>
    public abstract class BaseContentPage<TViewModel> : BaseContentPage
        where TViewModel : BaseViewModel
    {
        public TViewModel ViewModel => (TViewModel)BindingContext;
        
        protected BaseContentPage(TViewModel viewModel)
        {
            BindingContext = viewModel;
        }
    }
}
```

**Tests:**
- [ ] Compiles without errors
- [ ] Generic version compiles
- [ ] Can create derived page

---

#### Task 1.4: Setup Shell Navigation (1h)
**Location:** `tabApp.CrossPlatform/AppShell.xaml`

**Code:**
```xml
<?xml version="1.0" encoding="UTF-8" ?>
<Shell
    x:Class="tabApp.CrossPlatform.AppShell"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:views="clr-namespace:tabApp.CrossPlatform.Views"
    Title="Gestor App">
    
    <!-- FlyoutHeader (optional) -->
    <Shell.FlyoutHeader>
        <Grid HeightRequest="100" BackgroundColor="#2B5B84">
            <Label Text="Gestor de Frota" 
                   TextColor="White" 
                   FontSize="20"
                   HorizontalOptions="Center" 
                   VerticalOptions="Center"/>
        </Grid>
    </Shell.FlyoutHeader>
    
    <!-- Main Tabs (will be populated as we migrate pages) -->
    <TabBar>
        <ShellContent
            Title="Home"
            Icon="home.png"
            Route="home"
            ContentTemplate="{DataTemplate views:HomePage}" />
    </TabBar>
</Shell>
```

**AppShell.xaml.cs:**
```csharp
namespace tabApp.CrossPlatform
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }
        
        private void RegisterRoutes()
        {
            // Register routes for pages not in visual hierarchy
            // Format: Routing.RegisterRoute("routeName", typeof(PageType));
            
            // Examples (will be added as we migrate):
            // Routing.RegisterRoute("details", typeof(DetailsPage));
            // Routing.RegisterRoute("edit", typeof(EditPage));
        }
    }
}
```

**Tests:**
- [ ] Shell compiles
- [ ] Shell renders (empty is fine)
- [ ] Routes can be registered

---

### Afternoon (3.5h)

#### Task 1.5: Configure Dependency Injection (2h)
**Location:** `tabApp.CrossPlatform/MauiProgram.cs`

**Code:**
```csharp
using Microsoft.Extensions.Logging;
using tabApp.CrossPlatform.ViewModels;
using tabApp.CrossPlatform.Views;
using tabApp.Core.Services.Interfaces;
using tabApp.Core.Services.Implementations;

namespace tabApp.CrossPlatform
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Register Services
            RegisterServices(builder.Services);
            
            // Register ViewModels
            RegisterViewModels(builder.Services);
            
            // Register Pages
            RegisterPages(builder.Services);

            return builder.Build();
        }
        
        private static void RegisterServices(IServiceCollection services)
        {
            // TODO: Register all services from Core
            // For now, register a few examples:
            
            // Singletons (app-wide state)
            // services.AddSingleton<IFileService, FileService>();
            // services.AddSingleton<ISQLiteService, SQLiteService>();
            
            // Transients (created each time)
            // services.AddTransient<IDialogService, DialogService>();
        }
        
        private static void RegisterViewModels(IServiceCollection services)
        {
            // ViewModels are typically Transient
            // TODO: Register all ViewModels as we migrate them
            
            // Example:
            // services.AddTransient<HomeViewModel>();
        }
        
        private static void RegisterPages(IServiceCollection services)
        {
            // Pages are typically Transient
            // TODO: Register all Pages as we migrate them
            
            // Example:
            // services.AddTransient<HomePage>();
        }
    }
}
```

**Tests:**
- [ ] App builds
- [ ] MauiProgram runs
- [ ] DI container initializes

---

#### Task 1.6: Create Navigation Helpers (1h)
**Location:** `tabApp.CrossPlatform/Services/NavigationService.cs`

**Code:**
```csharp
namespace tabApp.CrossPlatform.Services
{
    /// <summary>
    /// Route constants for type-safe navigation
    /// </summary>
    public static class Routes
    {
        // Main routes (in visual hierarchy)
        public const string Home = "//home";
        
        // Detail routes (registered programmatically)
        public const string Details = "details";
        public const string Edit = "edit";
        
        // Add more as pages are migrated
    }
    
    /// <summary>
    /// Navigation helper extensions for type-safe navigation
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
        /// Navigate back
        /// </summary>
        public static Task NavigateBackAsync(this Shell shell)
        {
            return shell.GoToAsync("..");
        }
        
        // Add more helper methods as needed
    }
}
```

**Tests:**
- [ ] Compiles without errors
- [ ] Routes are accessible
- [ ] Extension methods visible

---

#### Task 1.7: Update App.xaml.cs (30m)
**Location:** `tabApp.CrossPlatform/App.xaml.cs`

**Code:**
```csharp
namespace tabApp.CrossPlatform
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}
```

**Tests:**
- [ ] App launches
- [ ] Shell is MainPage
- [ ] No crashes

---

### Day 1 Deliverables
- [ ] CommunityToolkit.Mvvm installed
- [ ] BaseViewModel created and tested
- [ ] BaseContentPage created and tested
- [ ] Shell navigation configured
- [ ] DI infrastructure ready
- [ ] Navigation helpers created
- [ ] App launches successfully

---

## 📋 Day 2-3: POC Implementation

### Day 2 Morning (4h)

#### Task 2.1: Select POC ViewModel (30m)
**Action:** Choose a simple ViewModel for migration

**Candidates:**
1. SettingsViewModel (simple, no complex navigation)
2. SplashViewModel (simple initialization logic)
3. DatabaseManagerPageViewModel (moderate complexity)

**Selection:** SettingsViewModel  
**Rationale:** 
- Simple properties
- Few commands
- No complex navigation
- Good learning example

---

#### Task 2.2: Analyze SettingsViewModel (1h)
**Location:** `tabApp.Core/ViewModels/Main/SettingsViewModel.cs`

**Analysis:**
- [ ] List all properties
- [ ] List all commands
- [ ] Identify dependencies (services)
- [ ] Document navigation calls
- [ ] Note any special patterns

**Create Analysis Document:**
```markdown
# SettingsViewModel Analysis

## Properties
- [ ] Property1: Type
- [ ] Property2: Type

## Commands
- [ ] Command1: Action
- [ ] Command2: Action

## Dependencies
- [ ] IService1
- [ ] IService2

## Navigation
- [ ] Navigate to: ViewModelX
- [ ] Navigate to: ViewModelY

## Special Patterns
- [ ] Pattern1
- [ ] Pattern2
```

---

#### Task 2.3: Create New SettingsViewModel (2h)
**Location:** `tabApp.CrossPlatform/ViewModels/Settings/SettingsViewModel.cs`

**Template:**
```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using tabApp.CrossPlatform.ViewModels.Bases;
using tabApp.Core.Services.Interfaces;

namespace tabApp.CrossPlatform.ViewModels.Settings
{
    public partial class SettingsViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        // Add other dependencies
        
        // Properties (convert to ObservableProperty)
        [ObservableProperty]
        private string exampleProperty;
        
        public SettingsViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            Title = "Settings";
        }
        
        // Commands (convert to RelayCommand)
        [RelayCommand]
        private async Task SaveAsync()
        {
            IsBusy = true;
            try
            {
                // Implementation
                await _dialogService.ShowMessageAsync("Settings saved!");
            }
            finally
            {
                IsBusy = false;
            }
        }
        
        public override void OnAppearing()
        {
            base.OnAppearing();
            // Load data
        }
    }
}
```

**Tests:**
- [ ] Compiles without errors
- [ ] All properties auto-generated
- [ ] All commands auto-generated
- [ ] DI injection works

---

### Day 2 Afternoon (3.5h)

#### Task 2.4: Create SettingsPage XAML (2h)
**Location:** `tabApp.CrossPlatform/Views/Settings/SettingsPage.xaml`

**Code:**
```xml
<?xml version="1.0" encoding="utf-8" ?>
<base:BaseContentPage 
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:base="clr-namespace:tabApp.CrossPlatform.Views.Bases"
    xmlns:vm="clr-namespace:tabApp.CrossPlatform.ViewModels.Settings"
    x:Class="tabApp.CrossPlatform.Views.Settings.SettingsPage"
    x:DataType="vm:SettingsViewModel"
    Title="{Binding Title}">
    
    <ScrollView>
        <VerticalStackLayout Padding="20" Spacing="15">
            
            <!-- Example: Text Entry -->
            <Label Text="Example Property:" />
            <Entry Text="{Binding ExampleProperty}" 
                   Placeholder="Enter value"/>
            
            <!-- Example: Button with Command -->
            <Button Text="Save" 
                    Command="{Binding SaveCommand}"
                    IsEnabled="{Binding IsBusy, Converter={StaticResource InvertedBoolConverter}}"/>
            
            <!-- Loading Indicator -->
            <ActivityIndicator IsRunning="{Binding IsBusy}" 
                              IsVisible="{Binding IsBusy}"/>
            
        </VerticalStackLayout>
    </ScrollView>
</base:BaseContentPage>
```

**Code-behind:**
```csharp
using tabApp.CrossPlatform.ViewModels.Settings;
using tabApp.CrossPlatform.Views.Bases;

namespace tabApp.CrossPlatform.Views.Settings
{
    public partial class SettingsPage : BaseContentPage<SettingsViewModel>
    {
        public SettingsPage(SettingsViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
```

**Tests:**
- [ ] XAML compiles
- [ ] Page renders
- [ ] Bindings work
- [ ] Commands work

---

#### Task 2.5: Register SettingsPage in DI (30m)
**Location:** `tabApp.CrossPlatform/MauiProgram.cs`

**Add:**
```csharp
private static void RegisterViewModels(IServiceCollection services)
{
    services.AddTransient<SettingsViewModel>();
}

private static void RegisterPages(IServiceCollection services)
{
    services.AddTransient<SettingsPage>();
}
```

**Add Route:**
```csharp
// AppShell.xaml.cs
Routing.RegisterRoute("settings", typeof(SettingsPage));
```

**Tests:**
- [ ] Page resolves from DI
- [ ] ViewModel injected correctly
- [ ] Navigation works

---

#### Task 2.6: Test Navigation (1h)
**Action:** Navigate to SettingsPage from another page

**Test Cases:**
- [ ] Navigate to settings
- [ ] ViewModel initializes
- [ ] OnAppearing called
- [ ] Properties bindable
- [ ] Commands executable
- [ ] Navigate back works
- [ ] OnDisappearing called

---

### Day 3 Morning (4h)

#### Task 2.7: Test All ViewModel Features (2h)
**Test Cases:**
1. **Properties:**
   - [ ] ObservableProperty generates public property
   - [ ] PropertyChanged event fires
   - [ ] UI updates on property change
   - [ ] Two-way binding works

2. **Commands:**
   - [ ] RelayCommand generates public command
   - [ ] Command executes
   - [ ] Async commands work
   - [ ] CanExecute updates
   - [ ] Command parameters work

3. **Lifecycle:**
   - [ ] OnAppearing called when page appears
   - [ ] OnDisappearing called when page disappears
   - [ ] InitializeAsync called once

4. **Navigation:**
   - [ ] Can navigate to page
   - [ ] Can navigate away
   - [ ] Back button works
   - [ ] Parameters pass correctly

---

#### Task 2.8: Document POC Patterns (2h)
**Create Document:** `docs/tech/issue-002/POC_PATTERNS.md`

**Content:**
```markdown
# POC Migration Patterns

## 1. Property Migration

### Before (MvvmCross)
[code example]

### After (CommunityToolkit)
[code example]

## 2. Command Migration

### Before (MvvmCross)
[code example]

### After (CommunityToolkit)
[code example]

## 3. Navigation Migration

### Before (MvvmCross)
[code example]

### After (MAUI Shell)
[code example]

## 4. Lifecycle Migration

[patterns]

## 5. Common Gotchas

[list issues and solutions]
```

---

### Day 3 Afternoon (3.5h)

#### Task 2.9: Performance Testing (1.5h)
**Test Cases:**
- [ ] App startup time
- [ ] Page navigation time
- [ ] Property change performance
- [ ] Command execution performance
- [ ] Memory usage
- [ ] CPU usage

**Document Results:**
```markdown
# Performance Comparison

| Metric | MvvmCross | MAUI Toolkit | Change |
|--------|-----------|--------------|--------|
| Startup | Xms | Yms | +/-Z% |
| Navigation | Xms | Yms | +/-Z% |
| ... | ... | ... | ... |
```

---

#### Task 2.10: Create POC Summary (2h)
**Create Document:** `docs/tech/issue-002/POC_SUMMARY.md`

**Content:**
```markdown
# POC Summary

## Objective
Validate MAUI MVVM architecture with CommunityToolkit.Mvvm

## ViewModel Migrated
SettingsViewModel

## Results
✅ Success / ⚠️ Issues Found / ❌ Blocked

## What Worked Well
- [item]
- [item]

## Challenges Found
- [challenge + solution]
- [challenge + solution]

## Performance
[summary from 2.9]

## Code Reduction
- Original lines: X
- Migrated lines: Y
- Reduction: Z%

## Recommendation
✅ Proceed with full migration
⚠️ Proceed with adjustments
❌ Reconsider approach

## Next Steps
[list]
```

---

### Day 2-3 Deliverables
- [ ] SettingsViewModel migrated
- [ ] SettingsPage created and working
- [ ] Navigation tested
- [ ] All features validated
- [ ] Performance tested
- [ ] POC patterns documented
- [ ] POC summary created

---

## 📋 Day 4: Documentation

### Morning (4h)

#### Task 3.1: Create ViewModel Migration Guide (2h)
**Create:** `docs/tech/issue-002/VIEWMODEL_MIGRATION_GUIDE.md`

**Outline:**
1. Prerequisites
2. Step-by-step migration process
3. Property conversion patterns
4. Command conversion patterns
5. Navigation conversion patterns
6. Lifecycle conversion patterns
7. Dependency injection
8. Common patterns library
9. Troubleshooting guide

---

#### Task 3.2: Create Navigation Guide (2h)
**Create:** `docs/tech/issue-002/NAVIGATION_GUIDE.md`

**Outline:**
1. Shell navigation basics
2. Route registration
3. Simple navigation
4. Navigation with parameters
5. Navigation with results
6. Type-safe navigation helpers
7. Deep linking
8. Back button handling
9. Navigation troubleshooting

---

### Afternoon (3.5h)

#### Task 3.3: Create Command Reference (1.5h)
**Create:** `docs/tech/issue-002/COMMAND_REFERENCE.md`

**Outline:**
1. RelayCommand basics
2. Async commands
3. Commands with parameters
4. CanExecute logic
5. Command naming conventions
6. Command bindings
7. Common patterns

---

#### Task 3.4: Create Code Templates (2h)
**Create Templates for:**
1. BaseViewModel usage
2. Simple ViewModel
3. ViewModel with navigation
4. ViewModel with parameters
5. ContentPage XAML
6. ContentPage code-behind

**Location:** `docs/tech/issue-002/templates/`

---

### Day 4 Deliverables
- [ ] ViewModel migration guide
- [ ] Navigation guide
- [ ] Command reference
- [ ] Code templates
- [ ] All documentation complete

---

## 📋 Day 5: Review & Training

### Morning (4h)

#### Task 4.1: Team Presentation (2h)
**Agenda:**
1. Current architecture overview (15min)
2. MAUI architecture overview (30min)
3. POC demo (30min)
4. Migration patterns walkthrough (30min)
5. Q&A (15min)

**Deliverable:** Presentation slides

---

#### Task 4.2: Hands-on Workshop (2h)
**Activity:** Team migrates a simple ViewModel together

**Steps:**
1. Select a simple ViewModel
2. Walkthrough analysis
3. Migrate together step-by-step
4. Test the result
5. Discuss learnings

---

### Afternoon (3.5h)

#### Task 4.3: Documentation Review (1.5h)
**Action:** Team reviews all documentation

**Feedback:**
- [ ] Documentation is clear
- [ ] Examples are sufficient
- [ ] Templates are usable
- [ ] Questions answered

---

#### Task 4.4: Architecture Approval (1h)
**Action:** Get formal approval to proceed

**Approval Checklist:**
- [ ] POC demonstrates feasibility
- [ ] Documentation is complete
- [ ] Team is trained
- [ ] Performance is acceptable
- [ ] Migration plan is clear

**Decision:** ✅ Approve / ⚠️ Approve with changes / ❌ Reject

---

#### Task 4.5: Create Migration Roadmap (1h)
**If approved:** Create detailed plan for Phase 1 implementation

**Document:** `docs/tech/issue-002/MIGRATION_ROADMAP.md`

**Content:**
- Priority order for ViewModels
- Timeline estimates
- Resource allocation
- Risk mitigation

---

### Day 5 Deliverables
- [ ] Team presentation completed
- [ ] Hands-on workshop completed
- [ ] Documentation reviewed
- [ ] Architecture approved
- [ ] Migration roadmap created

---

## ✅ Assessment Complete Criteria

- [ ] All 5 days completed
- [ ] POC successful
- [ ] Documentation complete
- [ ] Team trained
- [ ] Architecture approved
- [ ] Ready to proceed with implementation

---

## 📊 Success Metrics

| Metric | Target | Actual |
|--------|--------|--------|
| POC ViewModel migrated | 1 | ___ |
| Documentation pages | 5+ | ___ |
| Team training hours | 4 | ___ |
| Code templates created | 6+ | ___ |
| Team approval | ✅ | ___ |

---

## 🚀 Next Phase

**After Assessment Approval:**
- Begin ISSUE-002 Implementation Phase
- Start migrating ViewModels systematically
- Estimated: 10-15 days for all 47 ViewModels

---

**Document Version:** 1.0  
**Status:** READY TO EXECUTE  
**Start Date:** TBD (after ISSUE-001 review)


