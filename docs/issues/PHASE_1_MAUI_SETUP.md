# Phase 1 - MAUI Base Setup Issues

---

## ISSUE-009: Create MAUI Project Structure and Configuration

### 🎯 Objective
Initialize .NET MAUI project structure in tabApp.CrossPlatform with proper configuration for Android, iOS, and Windows platforms.

### 📍 Current Implementation
- **Location:** tabApp.CrossPlatform (partially created)
- **Type:** Project Setup
- **Complexity:** Medium
- **Risk Level:** MEDIUM

**Current State:**
- Basic MAUI project structure exists
- TargetFrameworks: net10.0-android, net10.0-ios, net10.0-maccatalyst, net10.0-windows
- Minimal configuration

### 🔄 Migration Strategy
- [ ] Review and update project file (tabApp.CrossPlatform.csproj)
- [ ] Configure Android manifest equivalent (Platforms/Android)
- [ ] Set ApplicationId to match existing (com.filipetorres.tabapp)
- [ ] Configure version code and name strategy
- [ ] Setup multi-targeting properly
- [ ] Configure assets and resources structure
- [ ] Setup platform-specific folders
- [ ] Configure build configurations (Debug/Release)
- [ ] Setup app icons and splash screens
- [ ] Configure permissions per platform

### ⚠️ Risks
- Platform-specific configuration mismatches
- Build configuration errors
- Resource file organization issues
- Permission configuration differences

### 📦 Dependencies
- ISSUE-001 (Dependency Audit)
- ISSUE-002 (Architecture Assessment)

### ✅ Definition of Done
- [ ] Project builds successfully for all target platforms
- [ ] ApplicationId matches existing app
- [ ] Version management configured
- [ ] App icon and splash screen configured
- [ ] Basic permissions configured
- [ ] Platform folders properly organized
- [ ] Build configurations tested
- [ ] Documentation updated

**Labels:** `type:migration`, `platform:maui`, `risk:medium`, `phase:setup`  
**Milestone:** MAUI Foundation  
**Priority:** P0  
**Estimate:** 3 days

---

## ISSUE-010: Setup Dependency Injection with .NET MAUI DI

### 🎯 Objective
Migrate from Autofac + MvvmCross DI to .NET MAUI built-in dependency injection with MauiProgram.cs.

### 📍 Current Implementation
- **Location:** 
  - tabApp/Setup.cs (MvvmCross setup)
  - tabApp.Core/App.cs (Autofac registration)
- **Type:** Infrastructure
- **Complexity:** High
- **Risk Level:** HIGH

**Current DI:**
- Autofac container
- MvvmCross DI integration
- Service registration by convention (EndsWith "Service", "Request", "Client")
- Platform-specific service registration in Setup.cs

### 🔄 Migration Strategy
- [ ] Create MauiProgram.cs with builder pattern
- [ ] Migrate all service registrations from Autofac
- [ ] Convert convention-based registration to explicit
- [ ] Register all 96+ services from Core
- [ ] Register platform-specific services
- [ ] Setup logging configuration
- [ ] Configure HttpClient factory
- [ ] Register ViewModels with proper lifetime
- [ ] Test service resolution
- [ ] Create service locator helper if needed

### ⚠️ Risks
- Service lifetime mismatches (Singleton vs Transient)
- Circular dependency issues
- Registration order dependencies
- Platform-specific service resolution issues
- Breaking changes in service construction

### 📦 Dependencies
- ISSUE-009 (Project Structure)
- ISSUE-002 (Architecture Assessment)

### ✅ Definition of Done
- [ ] MauiProgram.cs created and configured
- [ ] All Core services registered
- [ ] All platform services registered
- [ ] ViewModels registered correctly
- [ ] Service resolution tested
- [ ] No runtime DI exceptions
- [ ] Logging configured and working
- [ ] Documentation for service registration created

**Labels:** `type:migration`, `platform:maui`, `risk:high`, `phase:setup`, `type:infra`  
**Milestone:** MAUI Foundation  
**Priority:** P0  
**Estimate:** 4 days

---

## ISSUE-011: Setup Shell Navigation Structure

### 🎯 Objective
Create MAUI Shell navigation structure to replace MvvmCross navigation and MainActivity drawer navigation.

### 📍 Current Implementation
- **Location:** 
  - tabApp/UI/Activitys/MainActivity.cs (NavigationView drawer)
  - MvvmCross navigation service
- **Type:** Navigation Infrastructure
- **Complexity:** High
- **Risk Level:** HIGH

**Current Navigation:**
- NavigationView with drawer menu
- MvxNavigationService for fragment navigation
- Fragment-based navigation
- ViewPager for tabbed navigation

### 🔄 Migration Strategy
- [ ] Design Shell structure (Flyout vs TabBar vs Top Tabs)
- [ ] Create AppShell.xaml with navigation hierarchy
- [ ] Define routes for all pages
- [ ] Implement Flyout menu (equivalent to drawer)
- [ ] Setup route registration
- [ ] Create navigation service abstraction
- [ ] Implement deep linking support
- [ ] Setup navigation parameters passing
- [ ] Test navigation flow between major screens
- [ ] Document navigation patterns for team

### ⚠️ Risks
- Navigation paradigm shift from fragments to pages
- Complex navigation flows may not map directly
- Parameter passing differences
- Back navigation behavior changes
- ViewPager equivalent needs TabbedPage implementation

### 📦 Dependencies
- ISSUE-009 (Project Structure)
- ISSUE-010 (DI Setup)

### ✅ Definition of Done
- [ ] AppShell.xaml created with main navigation
- [ ] All routes registered
- [ ] Flyout menu functional
- [ ] Navigation between 3+ pages working
- [ ] Parameter passing validated
- [ ] Back navigation working correctly
- [ ] Deep linking tested
- [ ] Navigation documentation created

**Labels:** `type:migration`, `platform:maui`, `risk:high`, `phase:setup`, `navigation`  
**Milestone:** MAUI Foundation  
**Priority:** P0  
**Estimate:** 5 days

---

## ISSUE-012: Configure Logging and Debugging Infrastructure

### 🎯 Objective
Setup comprehensive logging infrastructure using Microsoft.Extensions.Logging and debugging tools for MAUI.

### 📍 Current Implementation
- **Location:** 
  - Android.Util.Log usage throughout code
  - Microsoft.AppCenter.Analytics
  - Microsoft.AppCenter.Crashes
- **Type:** Infrastructure
- **Complexity:** Medium
- **Risk Level:** MEDIUM

### 🔄 Migration Strategy
- [ ] Configure Microsoft.Extensions.Logging in MauiProgram
- [ ] Setup debug logging provider
- [ ] Configure AppCenter for MAUI
- [ ] Create logging abstraction/service
- [ ] Replace Android.Util.Log calls
- [ ] Configure log levels per environment
- [ ] Setup crash reporting
- [ ] Configure analytics events
- [ ] Test logging on all platforms
- [ ] Document logging standards

### ⚠️ Risks
- AppCenter MAUI compatibility issues
- Performance impact of excessive logging
- Log level misconfiguration
- Platform-specific logging differences

### 📦 Dependencies
- ISSUE-009 (Project Structure)
- ISSUE-010 (DI Setup)

### ✅ Definition of Done
- [ ] Logging configured in MauiProgram
- [ ] AppCenter integrated and tested
- [ ] Logging service created
- [ ] Sample logs working on all platforms
- [ ] Crash reporting validated
- [ ] Analytics tracking tested
- [ ] Log level configuration documented
- [ ] Team coding standards for logging documented

**Labels:** `type:migration`, `platform:maui`, `risk:medium`, `phase:setup`, `type:infra`  
**Milestone:** MAUI Foundation  
**Priority:** P1  
**Estimate:** 2 days

---

## ISSUE-013: Setup HttpClient Configuration and API Services

### 🎯 Objective
Configure HttpClient factory and base API service configuration for web service communication.

### 📍 Current Implementation
- **Location:** tabApp.Core/Services/Interfaces/WebServices
- **Type:** Infrastructure
- **Complexity:** Medium
- **Risk Level:** MEDIUM

**Current Setup:**
- HttpClient usage in service classes
- Base URL configuration
- Timeout and retry logic
- JSON serialization

### 🔄 Migration Strategy
- [ ] Setup HttpClient factory in MauiProgram
- [ ] Configure named/typed clients for different APIs
- [ ] Implement base API service class
- [ ] Configure default headers
- [ ] Setup timeout configuration
- [ ] Implement retry policies (Polly)
- [ ] Configure JSON serialization options
- [ ] Setup SSL pinning if required
- [ ] Test connectivity with backend
- [ ] Document API service patterns

### ⚠️ Risks
- API compatibility issues
- Authentication token handling changes
- SSL certificate validation issues
- Serialization breaking changes

### 📦 Dependencies
- ISSUE-010 (DI Setup)

### ✅ Definition of Done
- [ ] HttpClient factory configured
- [ ] Base API service implemented
- [ ] Named clients for all APIs created
- [ ] Retry policy implemented
- [ ] JSON serialization tested
- [ ] SSL configuration validated
- [ ] Sample API call working
- [ ] Documentation created

**Labels:** `type:migration`, `platform:maui`, `risk:medium`, `phase:setup`, `type:infra`  
**Milestone:** MAUI Foundation  
**Priority:** P1  
**Estimate:** 2 days

---

## ISSUE-014: Configure Environment and Settings Management

### 🎯 Objective
Setup environment configuration system (Dev, Staging, Production) and app settings management.

### 📍 Current Implementation
- **Location:** Various hardcoded values, AndroidManifest metadata
- **Type:** Configuration
- **Complexity:** Medium
- **Risk Level:** MEDIUM

### 🔄 Migration Strategy
- [ ] Design configuration system (appsettings.json approach)
- [ ] Create environment-specific configuration
- [ ] Implement settings service
- [ ] Migrate API URLs and endpoints
- [ ] Setup feature flags if needed
- [ ] Configure build-time settings injection
- [ ] Implement secure configuration for secrets
- [ ] Setup user preferences migration
- [ ] Test configuration loading
- [ ] Document configuration management

### ⚠️ Risks
- Configuration not available at startup
- Secret exposure in configuration files
- Build configuration complexity
- Platform-specific settings issues

### 📦 Dependencies
- ISSUE-009 (Project Structure)
- ISSUE-010 (DI Setup)

### ✅ Definition of Done
- [ ] Configuration system implemented
- [ ] Environment-specific configs created
- [ ] Settings service working
- [ ] No hardcoded URLs in code
- [ ] Build configurations tested
- [ ] Secrets properly secured
- [ ] User preferences accessible
- [ ] Documentation complete

**Labels:** `type:migration`, `platform:maui`, `risk:medium`, `phase:setup`, `type:infra`  
**Milestone:** MAUI Foundation  
**Priority:** P1  
**Estimate:** 2 days

---

## ISSUE-015: Setup SQLite Database Configuration

### 🎯 Objective
Configure SQLite database initialization and migration for MAUI application.

### 📍 Current Implementation
- **Location:** tabApp.Core (sqlite-net-pcl)
- **Type:** Data Layer
- **Complexity:** Medium
- **Risk Level:** HIGH

**Current Database:**
- sqlite-net-pcl for ORM
- SQLiteNetExtensions for relationships
- Local database file
- Existing schema with multiple tables

### 🔄 Migration Strategy
- [ ] Validate sqlite-net-pcl MAUI compatibility
- [ ] Configure database file location per platform
- [ ] Setup database initialization
- [ ] Plan schema migration strategy
- [ ] Implement database version management
- [ ] Test database access on all platforms
- [ ] Setup database seeding for dev/test
- [ ] Implement database backup/restore
- [ ] Validate data integrity
- [ ] Document database setup

### ⚠️ Risks
- Database file location differences across platforms
- Data migration from existing app
- Schema compatibility issues
- Performance differences
- File access permission issues

### 📦 Dependencies
- ISSUE-009 (Project Structure)
- ISSUE-010 (DI Setup)

### ✅ Definition of Done
- [ ] SQLite configured for all platforms
- [ ] Database initialization working
- [ ] Schema created successfully
- [ ] CRUD operations tested
- [ ] Migration strategy documented
- [ ] Backup/restore working
- [ ] Platform-specific paths configured
- [ ] Data integrity validated

**Labels:** `type:migration`, `platform:maui`, `risk:high`, `phase:setup`, `type:infra`, `database`  
**Milestone:** MAUI Foundation  
**Priority:** P0  
**Estimate:** 3 days

---

## ISSUE-016: Setup Fonts and Resources

### 🎯 Objective
Migrate custom fonts and configure MAUI resource system.

### 📍 Current Implementation
- **Location:** 
  - tabApp/Resources/font/arista_pro_bold.ttf
  - tabApp/Resources/font/arista_pro_light.ttf
- **Type:** Resources
- **Complexity:** Low
- **Risk Level:** LOW

### 🔄 Migration Strategy
- [ ] Copy font files to MAUI Resources/Fonts
- [ ] Register fonts in MauiProgram
- [ ] Update font references in styles
- [ ] Test font rendering on all platforms
- [ ] Setup resource dictionary structure
- [ ] Migrate colors from colors.xml
- [ ] Setup styles and themes
- [ ] Configure platform-specific resources

### ⚠️ Risks
- Font rendering differences across platforms
- Resource naming conflicts
- Style application issues

### 📦 Dependencies
- ISSUE-009 (Project Structure)

### ✅ Definition of Done
- [ ] Fonts copied and registered
- [ ] Font rendering validated on all platforms
- [ ] Resource dictionary structure created
- [ ] Colors migrated
- [ ] Basic styles defined
- [ ] Sample usage working
- [ ] Documentation updated

**Labels:** `type:migration`, `platform:maui`, `risk:low`, `phase:setup`, `type:ui`  
**Milestone:** MAUI Foundation  
**Priority:** P2  
**Estimate:** 1 day

---

## ISSUE-017: Setup Permissions Management System

### 🎯 Objective
Implement cross-platform permissions management system for MAUI.

### 📍 Current Implementation
- **Location:** AndroidManifest.xml (13 permissions)
- **Type:** Platform Feature
- **Complexity:** Medium
- **Risk Level:** HIGH

**Required Permissions:**
- Location (Fine, Coarse, Background)
- Bluetooth
- Network State
- Internet
- Storage (Write External)
- Foreground Service

### 🔄 Migration Strategy
- [ ] Configure Android permissions in AndroidManifest
- [ ] Configure iOS permissions in Info.plist
- [ ] Implement permission service using MAUI Essentials
- [ ] Create permission request flow
- [ ] Handle permission denied scenarios
- [ ] Implement runtime permission checks
- [ ] Test permission flow on all platforms
- [ ] Document permission requirements
- [ ] Create user-friendly permission explanations

### ⚠️ Risks
- Platform-specific permission differences
- iOS restrictions stricter than Android
- Permission denial handling complexity
- Runtime permission request timing

### 📦 Dependencies
- ISSUE-009 (Project Structure)

### ✅ Definition of Done
- [ ] All permissions configured for Android
- [ ] All permissions configured for iOS
- [ ] Permission service implemented
- [ ] Request flow working
- [ ] Denial handling implemented
- [ ] Tested on physical devices
- [ ] User explanations implemented
- [ ] Documentation complete

**Labels:** `type:migration`, `platform:maui`, `risk:high`, `phase:setup`, `type:infra`, `permissions`  
**Milestone:** MAUI Foundation  
**Priority:** P0  
**Estimate:** 2 days

---

## ISSUE-018: Configure Lottie Animations with SkiaSharp

### 🎯 Objective
Setup SkiaSharp.Extended.UI.Maui and prepare for Lottie animation migration.

### 📍 Current Implementation
- **Location:** Com.Airbnb.Android.Lottie (8 animation files)
- **Type:** UI Component
- **Complexity:** Medium
- **Risk Level:** MEDIUM

### 🔄 Migration Strategy
- [ ] Install SkiaSharp.Extended.UI.Maui NuGet
- [ ] Configure SkiaSharp in MauiProgram
- [ ] Copy Lottie JSON files to Resources/Raw
- [ ] Create reusable LottieView component
- [ ] Test loading and playback
- [ ] Implement playback controls
- [ ] Test on all platforms
- [ ] Create usage examples
- [ ] Document animation integration

### ⚠️ Risks
- Animation playback performance issues
- File loading path differences
- Platform-specific rendering issues
- Memory management concerns

### 📦 Dependencies
- ISSUE-009 (Project Structure)
- ISSUE-006 (Lottie Analysis)

### ✅ Definition of Done
- [ ] SkiaSharp.Extended configured
- [ ] All 8 animation files copied
- [ ] LottieView component created
- [ ] Sample animation playing on all platforms
- [ ] Playback controls working
- [ ] Performance validated
- [ ] Usage documentation created

**Labels:** `type:migration`, `platform:maui`, `risk:medium`, `phase:setup`, `type:ui`  
**Milestone:** MAUI Foundation  
**Priority:** P2  
**Estimate:** 2 days

---

## ISSUE-019: Setup Maps Infrastructure

### 🎯 Objective
Configure maps infrastructure for MAUI (Microsoft.Maui.Controls.Maps or alternative).

### 📍 Current Implementation
- **Location:** GooglePlayServices.Maps in HomePageMapFragment
- **Type:** UI Component
- **Complexity:** High
- **Risk Level:** HIGH

### 🔄 Migration Strategy
- [ ] Install chosen maps package (based on ISSUE-005 decision)
- [ ] Configure maps in MauiProgram
- [ ] Setup API keys per platform
- [ ] Create sample map page
- [ ] Test basic map rendering
- [ ] Implement location tracking on map
- [ ] Test marker placement
- [ ] Validate performance
- [ ] Document maps integration

### ⚠️ Risks
- Feature parity with Google Maps
- API key management complexity
- Platform-specific configuration issues
- Performance concerns with many markers

### 📦 Dependencies
- ISSUE-009 (Project Structure)
- ISSUE-005 (Maps Analysis)

### ✅ Definition of Done
- [ ] Maps package installed and configured
- [ ] API keys configured securely
- [ ] Sample map rendering on all platforms
- [ ] Location tracking working
- [ ] Marker placement tested
- [ ] Performance acceptable
- [ ] Documentation created

**Labels:** `type:migration`, `platform:maui`, `risk:high`, `phase:setup`, `type:ui`, `maps`  
**Milestone:** MAUI Foundation  
**Priority:** P1  
**Estimate:** 3 days

---

## ISSUE-020: Setup Unit Testing Infrastructure

### 🎯 Objective
Configure unit testing infrastructure for MAUI project with xUnit or NUnit.

### 📍 Current Implementation
- **Location:** No existing test project identified
- **Type:** Testing
- **Complexity:** Medium
- **Risk Level:** MEDIUM

### 🔄 Migration Strategy
- [ ] Create test project (xUnit.net recommended)
- [ ] Configure test dependencies
- [ ] Setup mocking framework (Moq/NSubstitute)
- [ ] Create test base classes
- [ ] Configure test coverage tools
- [ ] Write sample tests for core services
- [ ] Setup CI integration for tests
- [ ] Document testing standards
- [ ] Create test templates

### ⚠️ Risks
- Test framework compatibility issues
- Mocking platform-specific services difficulty
- CI configuration complexity

### 📦 Dependencies
- ISSUE-009 (Project Structure)

### ✅ Definition of Done
- [ ] Test project created and building
- [ ] Test framework configured
- [ ] Mocking framework setup
- [ ] 5+ sample tests passing
- [ ] Test coverage tool configured
- [ ] CI integration working
- [ ] Testing standards documented

**Labels:** `type:migration`, `platform:maui`, `risk:medium`, `phase:setup`, `type:test`  
**Milestone:** MAUI Foundation  
**Priority:** P2  
**Estimate:** 2 days


