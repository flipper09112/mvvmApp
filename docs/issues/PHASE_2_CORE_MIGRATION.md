# Phase 2 - Core Layer Migration Issues

---

## ISSUE-021: Migrate Domain Models

### 🎯 Objective
Migrate all domain models from tabApp.Core/Models to ensure .NET 8.0+ compatibility.

### 📍 Current Implementation
- **Location:** tabApp.Core/Models (16 model classes)
- **Type:** Domain Layer
- **Complexity:** Low
- **Risk Level:** LOW

**Key Models:**
- Client
- Order
- Product
- Notification
- Address
- User
- Vehicle
- Invoice

### 🔄 Migration Strategy
- [ ] Review all model classes for MAUI compatibility
- [ ] Update to C# 10/11 features if beneficial
- [ ] Validate SQLite attributes compatibility
- [ ] Ensure JSON serialization compatibility
- [ ] Update DateTime handling for cross-platform
- [ ] Test model instantiation
- [ ] Validate relationships (SQLiteNetExtensions)
- [ ] Update documentation

### ⚠️ Risks
- Serialization breaking changes
- SQLite attribute incompatibility
- DateTime timezone handling differences

### 📦 Dependencies
- ISSUE-015 (SQLite Setup)

### ✅ Definition of Done
- [ ] All models compile in MAUI project
- [ ] SQLite attributes validated
- [ ] JSON serialization tested
- [ ] Relationships working
- [ ] DateTime handling verified
- [ ] Unit tests created for key models
- [ ] Documentation updated

**Labels:** `type:migration`, `platform:maui`, `risk:low`, `phase:core`, `domain`  
**Milestone:** Core & Infrastructure  
**Priority:** P1  
**Estimate:** 2 days

---

## ISSUE-022: Migrate DTOs and API Models

### 🎯 Objective
Migrate all DTO classes used for API communication and ensure serialization compatibility.

### 📍 Current Implementation
- **Location:** tabApp.Core/Converters/Http
- **Type:** Data Transfer
- **Complexity:** Low
- **Risk Level:** MEDIUM

### 🔄 Migration Strategy
- [ ] Inventory all DTO classes
- [ ] Validate JSON property attributes
- [ ] Test serialization/deserialization
- [ ] Update to System.Text.Json if needed
- [ ] Validate API contract compatibility
- [ ] Test with actual API calls
- [ ] Handle null reference types properly
- [ ] Document API contracts

### ⚠️ Risks
- JSON library changes (Newtonsoft vs System.Text.Json)
- Breaking API contract changes
- Null handling differences

### 📦 Dependencies
- ISSUE-021 (Domain Models)

### ✅ Definition of Done
- [ ] All DTOs migrated
- [ ] Serialization tested
- [ ] API calls successful
- [ ] Null handling validated
- [ ] Unit tests for serialization
- [ ] Documentation updated

**Labels:** `type:migration`, `platform:maui`, `risk:medium`, `phase:core`, `api`  
**Milestone:** Core & Infrastructure  
**Priority:** P1  
**Estimate:** 2 days

---

## ISSUE-023: Migrate Business Logic Services

### 🎯 Objective
Migrate all business logic service implementations from tabApp.Core/Services/Implementations.

### 📍 Current Implementation
- **Location:** tabApp.Core/Services/Implementations
- **Type:** Business Logic
- **Complexity:** High
- **Risk Level:** HIGH

**Service Categories:**
- Clients management
- Orders management
- Products management
- Notifications management
- Faturation/Invoicing
- Deliveries management

### 🔄 Migration Strategy
- [ ] Review all service implementations
- [ ] Update DI registration
- [ ] Remove MvvmCross dependencies
- [ ] Validate async/await patterns
- [ ] Test business logic integrity
- [ ] Update logging calls
- [ ] Validate error handling
- [ ] Create unit tests
- [ ] Document service contracts

### ⚠️ Risks
- Business logic regression
- Dependency injection issues
- Async pattern changes
- Error handling gaps

### 📦 Dependencies
- ISSUE-010 (DI Setup)
- ISSUE-021 (Domain Models)
- ISSUE-024 (Service Interfaces)

### ✅ Definition of Done
- [ ] All services migrated
- [ ] DI registration complete
- [ ] No MvvmCross dependencies
- [ ] Business logic tests passing
- [ ] Error handling validated
- [ ] Logging working
- [ ] Documentation updated

**Labels:** `type:migration`, `platform:maui`, `risk:high`, `phase:core`, `business-logic`  
**Milestone:** Core & Infrastructure  
**Priority:** P0  
**Estimate:** 5 days

---

## ISSUE-024: Migrate Service Interfaces

### 🎯 Objective
Migrate all service interface definitions from tabApp.Core/Services/Interfaces.

### 📍 Current Implementation
- **Location:** tabApp.Core/Services/Interfaces (multiple folders)
- **Type:** Contracts
- **Complexity:** Medium
- **Risk Level:** MEDIUM

**Interface Categories:**
- Bluetooth interfaces
- Client management interfaces
- Database interfaces
- Dialog interfaces
- Faturation interfaces
- Helper interfaces
- Notification interfaces
- Order interfaces
- Product interfaces
- Timer interfaces
- WebService interfaces

### 🔄 Migration Strategy
- [ ] Review all interface definitions
- [ ] Remove platform-specific types
- [ ] Update to MAUI-compatible types
- [ ] Validate method signatures
- [ ] Update XML documentation
- [ ] Test interface implementations
- [ ] Create interface documentation
- [ ] Validate backwards compatibility where needed

### ⚠️ Risks
- Breaking interface changes
- Platform type incompatibility
- Implementation mismatch

### 📦 Dependencies
- ISSUE-021 (Domain Models)

### ✅ Definition of Done
- [ ] All interfaces migrated
- [ ] No platform-specific types
- [ ] Documentation complete
- [ ] Implementations compatible
- [ ] No compilation errors
- [ ] Interface contracts validated

**Labels:** `type:migration`, `platform:maui`, `risk:medium`, `phase:core`, `contracts`  
**Milestone:** Core & Infrastructure  
**Priority:** P0  
**Estimate:** 2 days

---

## ISSUE-025: Migrate Helper Classes

### 🎯 Objective
Migrate utility and helper classes to MAUI-compatible implementations.

### 📍 Current Implementation
- **Location:** 
  - tabApp.Core/Helpers
  - tabApp/Helpers
- **Type:** Utilities
- **Complexity:** Medium
- **Risk Level:** MEDIUM

**Helper Classes:**
- ClientHelper
- DecimalDigitsInputFilter
- Downloader
- FragmentHelper
- ImageHelper
- LoadingPopPupHelper
- MyWebViewClient
- NotificationHelper
- PrinterHelper
- StringHelper

### 🔄 Migration Strategy
- [ ] Review each helper class
- [ ] Identify platform-specific helpers
- [ ] Refactor or remove Android-specific helpers (FragmentHelper, etc.)
- [ ] Migrate reusable helpers to Core
- [ ] Create MAUI equivalents for UI helpers
- [ ] Test helper functionality
- [ ] Update usages throughout codebase
- [ ] Document helper purposes

### ⚠️ Risks
- Platform-specific helper functionality loss
- Breaking changes in helper APIs
- Performance differences

### 📦 Dependencies
- ISSUE-021 (Domain Models)

### ✅ Definition of Done
- [ ] All helpers reviewed
- [ ] Platform-agnostic helpers migrated
- [ ] MAUI equivalents created
- [ ] Helper functionality tested
- [ ] Usages updated
- [ ] Documentation complete

**Labels:** `type:migration`, `platform:maui`, `risk:medium`, `phase:core`, `utilities`  
**Milestone:** Core & Infrastructure  
**Priority:** P2  
**Estimate:** 3 days

---

## ISSUE-026: Migrate Enums and Constants

### 🎯 Objective
Migrate all enumerations and constant definitions to MAUI project.

### 📍 Current Implementation
- **Location:** tabApp.Core/Enums
- **Type:** Domain
- **Complexity:** Low
- **Risk Level:** LOW

### 🔄 Migration Strategy
- [ ] Copy all enum files
- [ ] Validate enum values
- [ ] Ensure serialization compatibility
- [ ] Update any platform-specific enums
- [ ] Test enum usage
- [ ] Document enum purposes

### ⚠️ Risks
- Serialization value mismatches
- Breaking changes in enum values

### 📦 Dependencies
- ISSUE-021 (Domain Models)

### ✅ Definition of Done
- [ ] All enums migrated
- [ ] Serialization tested
- [ ] No compilation errors
- [ ] Enum values validated
- [ ] Documentation complete

**Labels:** `type:migration`, `platform:maui`, `risk:low`, `phase:core`, `domain`  
**Milestone:** Core & Infrastructure  
**Priority:** P2  
**Estimate:** 1 day

---

## ISSUE-027: Migrate SQLite Service Implementation

### 🎯 Objective
Migrate SQLiteService from platform-specific to MAUI cross-platform implementation.

### 📍 Current Implementation
- **Location:** tabApp/Services/Implementations/CrossPlat/SQLiteService.cs
- **Type:** Data Access
- **Complexity:** Medium
- **Risk Level:** HIGH

### 🔄 Migration Strategy
- [ ] Review current SQLiteService implementation
- [ ] Update platform-specific paths using MAUI APIs
- [ ] Test database file location on all platforms
- [ ] Validate connection management
- [ ] Test CRUD operations
- [ ] Implement database migration logic
- [ ] Test concurrent access scenarios
- [ ] Create database service tests

### ⚠️ Risks
- Database file location issues per platform
- Data migration from existing app
- Concurrent access issues
- Performance degradation

### 📦 Dependencies
- ISSUE-015 (SQLite Setup)
- ISSUE-024 (Service Interfaces)

### ✅ Definition of Done
- [ ] SQLiteService migrated
- [ ] Platform paths configured
- [ ] CRUD operations tested on all platforms
- [ ] Migration logic working
- [ ] Performance validated
- [ ] Unit tests passing
- [ ] Documentation complete

**Labels:** `type:migration`, `platform:maui`, `risk:high`, `phase:core`, `database`  
**Milestone:** Core & Infrastructure  
**Priority:** P0  
**Estimate:** 3 days

---

## ISSUE-028: Migrate File Service Implementation

### 🎯 Objective
Migrate FileService to use MAUI file system APIs.

### 📍 Current Implementation
- **Location:** tabApp/Services/Implementations/CrossPlat/FileService.cs
- **Type:** Platform Service
- **Complexity:** Medium
- **Risk Level:** MEDIUM

**Current Features:**
- File reading/writing
- Directory management
- File sharing (FileProvider)
- External storage access

### 🔄 Migration Strategy
- [ ] Review current FileService implementation
- [ ] Migrate to MAUI FileSystem API
- [ ] Update file paths for cross-platform
- [ ] Implement file sharing using MAUI APIs
- [ ] Handle permissions properly
- [ ] Test file operations on all platforms
- [ ] Validate external storage access
- [ ] Create unit tests

### ⚠️ Risks
- Platform-specific file path differences
- Permission handling complexity
- File sharing mechanism changes
- Storage access restrictions on iOS

### 📦 Dependencies
- ISSUE-024 (Service Interfaces)
- ISSUE-017 (Permissions)

### ✅ Definition of Done
- [ ] FileService migrated
- [ ] MAUI FileSystem APIs used
- [ ] File operations tested on all platforms
- [ ] File sharing working
- [ ] Permissions handled
- [ ] Unit tests passing
- [ ] Documentation complete

**Labels:** `type:migration`, `platform:maui`, `risk:medium`, `phase:core`, `file-system`  
**Milestone:** Core & Infrastructure  
**Priority:** P1  
**Estimate:** 2 days

---

## ISSUE-029: Migrate Dialog Service Implementation

### 🎯 Objective
Migrate DialogService to use MAUI display alerts and popups.

### 📍 Current Implementation
- **Location:** tabApp/Services/Implementations/CrossPlat/DialogService.cs
- **Type:** UI Service
- **Complexity:** Medium
- **Risk Level:** MEDIUM

**Current Features:**
- Alert dialogs
- Confirmation dialogs
- Input dialogs
- Custom dialogs
- Loading indicators

### 🔄 Migration Strategy
- [ ] Review current DialogService implementation
- [ ] Migrate to MAUI Page.DisplayAlert
- [ ] Implement prompt dialogs
- [ ] Create loading popup component
- [ ] Implement custom dialog support (CommunityToolkit.Maui.Popup)
- [ ] Test dialogs on all platforms
- [ ] Validate threading (UI thread access)
- [ ] Create dialog examples

### ⚠️ Risks
- Custom dialog complexity in MAUI
- UI thread access issues
- Platform-specific dialog behavior differences
- Loading indicator implementation complexity

### 📦 Dependencies
- ISSUE-024 (Service Interfaces)
- ISSUE-011 (Shell Navigation)

### ✅ Definition of Done
- [ ] DialogService migrated
- [ ] Basic alerts working
- [ ] Confirmation dialogs working
- [ ] Loading indicator implemented
- [ ] Custom dialogs supported
- [ ] Tested on all platforms
- [ ] Documentation complete

**Labels:** `type:migration`, `platform:maui`, `risk:medium`, `phase:core`, `ui-service`  
**Milestone:** Core & Infrastructure  
**Priority:** P1  
**Estimate:** 3 days

---

## ISSUE-030: Create Base ViewModel Classes

### 🎯 Objective
Create base ViewModel classes to replace MvvmCross base classes.

### 📍 Current Implementation
- **Location:** tabApp.Core/ViewModels/Bases
- **Type:** MVVM Infrastructure
- **Complexity:** High
- **Risk Level:** HIGH

**Current Base Classes:**
- MvxViewModel
- MvxNavigationViewModel
- Custom base classes

### 🔄 Migration Strategy
- [ ] Create ViewModelBase class (INotifyPropertyChanged)
- [ ] Implement NavigatableViewModel base class
- [ ] Add common properties (IsBusy, Title, etc.)
- [ ] Implement navigation support
- [ ] Add lifecycle methods (OnAppearing, OnDisappearing)
- [ ] Implement command helpers
- [ ] Create property change helpers
- [ ] Test base functionality
- [ ] Document ViewModel patterns

### ⚠️ Risks
- Navigation pattern differences from MvvmCross
- Lifecycle event timing differences
- Property change notification issues
- Command execution context issues

### 📦 Dependencies
- ISSUE-011 (Shell Navigation)
- ISSUE-002 (Architecture Assessment)

### ✅ Definition of Done
- [ ] ViewModelBase created
- [ ] Navigation support implemented
- [ ] Common properties defined
- [ ] Lifecycle methods working
- [ ] Command helpers implemented
- [ ] Sample ViewModel using base class
- [ ] Unit tests for base class
- [ ] Documentation complete

**Labels:** `type:migration`, `platform:maui`, `risk:high`, `phase:core`, `mvvm`, `architecture`  
**Milestone:** Core & Infrastructure  
**Priority:** P0  
**Estimate:** 4 days

---

## ISSUE-031: Migrate ViewModels - Login Module

### 🎯 Objective
Migrate LoginViewModel to MAUI base classes.

### 📍 Current Implementation
- **Location:** tabApp.Core/ViewModels/Login/LoginViewModel.cs
- **Type:** ViewModel
- **Complexity:** Medium
- **Risk Level:** HIGH

**Current Features:**
- User authentication
- Credential validation
- Navigation to Home
- Error handling

### 🔄 Migration Strategy
- [ ] Inherit from new ViewModelBase
- [ ] Remove MvvmCross dependencies
- [ ] Update navigation calls
- [ ] Update command implementations
- [ ] Test authentication flow
- [ ] Validate error handling
- [ ] Create ViewModel unit tests

### ⚠️ Risks
- Authentication flow regression
- Navigation breaking changes
- Credential storage issues

### 📦 Dependencies
- ISSUE-030 (Base ViewModel)
- ISSUE-023 (Business Logic Services)

### ✅ Definition of Done
- [ ] LoginViewModel migrated
- [ ] No MvvmCross dependencies
- [ ] Authentication working
- [ ] Navigation functional
- [ ] Error handling validated
- [ ] Unit tests passing
- [ ] Integration test with UI

**Labels:** `type:migration`, `platform:maui`, `risk:high`, `phase:core`, `mvvm`, `authentication`  
**Milestone:** Core & Infrastructure  
**Priority:** P0  
**Estimate:** 2 days

---

## ISSUE-032: Migrate ViewModels - Main Module

### 🎯 Objective
Migrate MainViewModel and SettingsViewModel to MAUI base classes.

### 📍 Current Implementation
- **Location:** 
  - tabApp.Core/ViewModels/MainViewModel.cs
  - tabApp.Core/ViewModels/Main/SettingsViewModel.cs
- **Type:** ViewModel
- **Complexity:** Medium
- **Risk Level:** MEDIUM

### 🔄 Migration Strategy
- [ ] Inherit from new ViewModelBase
- [ ] Remove MvvmCross dependencies
- [ ] Update navigation calls
- [ ] Migrate menu/drawer logic
- [ ] Update settings management
- [ ] Test navigation flows
- [ ] Create unit tests

### ⚠️ Risks
- Menu navigation complexity
- Settings persistence issues

### 📦 Dependencies
- ISSUE-030 (Base ViewModel)

### ✅ Definition of Done
- [ ] MainViewModel migrated
- [ ] SettingsViewModel migrated
- [ ] Navigation working
- [ ] Settings persistence validated
- [ ] Unit tests passing
- [ ] Documentation complete

**Labels:** `type:migration`, `platform:maui`, `risk:medium`, `phase:core`, `mvvm`  
**Milestone:** Core & Infrastructure  
**Priority:** P1  
**Estimate:** 2 days

---

## ISSUE-033: Migrate ViewModels - Home Module

### 🎯 Objective
Migrate all Home module ViewModels to MAUI base classes.

### 📍 Current Implementation
- **Location:** tabApp.Core/ViewModels/Home/
- **Type:** ViewModel
- **Complexity:** High
- **Risk Level:** HIGH

**ViewModels:**
- HomeViewModel
- DeleteClientViewModel
- GlobalOrderSelectDaysViewModel
- InitDailyViewModel
- SplashViewModel
- StopDailyViewModel

### 🔄 Migration Strategy
- [ ] Migrate each ViewModel to new base class
- [ ] Remove MvvmCross dependencies
- [ ] Update navigation calls
- [ ] Test home dashboard functionality
- [ ] Validate daily operations flow
- [ ] Create unit tests for each
- [ ] Integration testing

### ⚠️ Risks
- Home dashboard complexity
- Daily operations flow regression
- Data refresh issues

### 📦 Dependencies
- ISSUE-030 (Base ViewModel)
- ISSUE-023 (Business Logic Services)

### ✅ Definition of Done
- [ ] All Home ViewModels migrated
- [ ] No MvvmCross dependencies
- [ ] Home functionality working
- [ ] Daily operations validated
- [ ] Unit tests passing
- [ ] Integration tests successful

**Labels:** `type:migration`, `platform:maui`, `risk:high`, `phase:core`, `mvvm`, `feature:home`  
**Milestone:** Core & Infrastructure  
**Priority:** P0  
**Estimate:** 4 days

---

## ISSUE-034: Migrate ViewModels - Client Module

### 🎯 Objective
Migrate all Client module ViewModels to MAUI base classes.

### 📍 Current Implementation
- **Location:** tabApp.Core/ViewModels/ClientPage/
- **Type:** ViewModel
- **Complexity:** High
- **Risk Level:** HIGH

**ViewModels:**
- ClientPageViewModel
- ChooseProductViewModel
- Various client detail ViewModels

### 🔄 Migration Strategy
- [ ] Migrate each ViewModel
- [ ] Remove MvvmCross dependencies
- [ ] Update navigation
- [ ] Test client operations
- [ ] Validate order creation
- [ ] Create unit tests

### ⚠️ Risks
- Client operations complexity
- Order creation flow regression

### 📦 Dependencies
- ISSUE-030 (Base ViewModel)
- ISSUE-023 (Business Logic Services)

### ✅ Definition of Done
- [ ] All Client ViewModels migrated
- [ ] Client operations working
- [ ] Order creation validated
- [ ] Unit tests passing

**Labels:** `type:migration`, `platform:maui`, `risk:high`, `phase:core`, `mvvm`, `feature:client`  
**Milestone:** Core & Infrastructure  
**Priority:** P1  
**Estimate:** 4 days

---

## ISSUE-035: Migrate Remaining ViewModels

### 🎯 Objective
Migrate all remaining ViewModels (EditClient, Global, Snooze modules).

### 📍 Current Implementation
- **Location:** 
  - tabApp.Core/ViewModels/EditClient/
  - tabApp.Core/ViewModels/Global/
  - tabApp.Core/ViewModels/Snooze/
- **Type:** ViewModel
- **Complexity:** High
- **Risk Level:** HIGH

### 🔄 Migration Strategy
- [ ] Migrate EditClient ViewModels
- [ ] Migrate Global feature ViewModels
- [ ] Migrate Snooze ViewModels
- [ ] Remove MvvmCross dependencies
- [ ] Update all navigation
- [ ] Create unit tests
- [ ] Integration testing

### ⚠️ Risks
- Large number of ViewModels
- Complex interdependencies
- Feature regression

### 📦 Dependencies
- ISSUE-030 (Base ViewModel)
- ISSUE-023 (Business Logic Services)

### ✅ Definition of Done
- [ ] All remaining ViewModels migrated
- [ ] No MvvmCross dependencies
- [ ] All features working
- [ ] Unit tests passing
- [ ] Integration tests successful

**Labels:** `type:migration`, `platform:maui`, `risk:high`, `phase:core`, `mvvm`  
**Milestone:** Core & Infrastructure  
**Priority:** P1  
**Estimate:** 6 days


