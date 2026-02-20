# TASK-3.10 — Implementation Roadmap & Checklist
## ISSUE-003: Background Location Service Migration

**Date:** 2026-02-20  
**Status:** ✅ COMPLETE  
**Owner:** Tech Lead / Project Manager  
**Duration:** 0.5 days  
**Deliverables:** Implementation checklist, test strategy, performance plan, rollout plan

---

## 📌 Executive Summary

TASK-3.10 creates the detailed implementation roadmap and validation strategy that will guide the 3-week full implementation of the background location service migration. All PHASE 2 POCs (TASK-3.5 → 3.8) are complete and validated by TASK-3.9.

**This task provides:**
- ✅ Week-by-week sprint breakdown (3 weeks)
- ✅ 9 detailed subtasks with acceptance criteria
- ✅ Comprehensive test strategy (unit, integration, device, performance)
- ✅ Performance benchmarks (battery, CPU, memory targets)
- ✅ Effort estimates (87 hours, 3-4 people)
- ✅ Risk mitigations for implementation phase

---

## 🏗️ Implementation Roadmap (3 Weeks)

### Week 1: Plugin Integration & Platform Services

#### TASK 3.10.1 — LocalNotification Plugin Integration (2 days)

**Objective:** Replace POC `MauiLocalNotificationSender` stub with production `Plugin.LocalNotification`

**Deliverables:**
- [ ] NuGet package `Plugin.LocalNotification` added to `tabApp.CrossPlatform.csproj`
- [ ] `MauiProgram.cs` updated: `.UseLocalNotification()`
- [ ] `Services/Implementations/Notifications/LocalNotificationSender.cs` (replaces stub)
- [ ] `ILocalNotificationSender` adapter complete
- [ ] Unit tests updated for real notification API
- [ ] Android notification channel configuration review
- [ ] iOS notification permission request flow validated

**Validation:**
- [ ] Build succeeds on Android TFM
- [ ] Build succeeds on iOS TFM
- [ ] Task37Tests: 22 tests pass
- [ ] Manual test: Notification appears on Android device
- [ ] Manual test: Notification appears on iOS device

---

#### TASK 3.10.2 — Platform Service Registration (2 days)

**Objective:** Register platform-specific services in startup

**Deliverables:**
- [ ] `ISQLiteService` interface defined
- [ ] Android `SQLiteService` implementation
- [ ] iOS `SQLiteService` implementation
- [ ] `IFileService` interface defined
- [ ] Android `FileService` implementation
- [ ] iOS `FileService` implementation
- [ ] `MauiProgram.cs` updated: Platform service registration
- [ ] `DIConfiguration.cs` updated: References to platform services

**Validation:**
- [ ] `ServiceProvider` resolves all platform services without error
- [ ] Task38Tests: 20 tests pass
- [ ] No circular dependencies detected
- [ ] Android and iOS projects compile

---

### Week 2: Page/ViewModel Implementation & Testing

#### TASK 3.10.3 — Location Tracking ViewModel (2 days)

**Objective:** Create ViewModel that orchestrates location tracking lifecycle

**Deliverables:**
- [ ] `ViewModels/LocationTrackingViewModel.cs` (MVVM pattern)
- [ ] Properties: `IsTracking` (bool), `CurrentLocation` (string), `ProximityAlerts` (ObservableCollection)
- [ ] Commands: `StartTrackingCommand`, `StopTrackingCommand`, `RequestPermissionsCommand`
- [ ] Lifecycle integration: Start on app resume, stop on app suspend
- [ ] Error handling: Permission denied, location unavailable, service failures
- [ ] Unit tests: 15+ tests covering all commands

**Key Methods:**
```csharp
public class LocationTrackingViewModel : INotifyPropertyChanged
{
    private IBackgroundLocationTracker _tracker;
    private IProximityService _proximityService;
    private IProximityNotificationService _notificationService;
    
    public Command StartTrackingCommand { get; }
    public Command StopTrackingCommand { get; }
    public bool IsTracking { get; set; }
    public string CurrentLocation { get; set; }
    
    private async Task StartTracking();
    private async Task StopTracking();
    private async Task RequestLocationPermissions();
}
```

---

#### TASK 3.10.4 — Location Tracking Page UI (2 days)

**Objective:** Create XAML page for location tracking control and status display

**🚗 CRITICAL: Vehicle Tablet Design** (Landscape, Driver-Safe)
- ✅ Landscape orientation ONLY (never auto-rotate)
- ✅ 48dp minimum touch targets (safe for moving vehicle)
- ✅ Large fonts (18sp+ for status, 14sp+ minimum for text)
- ✅ High contrast (WCAG AAA for visibility in sunlight)
- ✅ NO animations or flashing (driver distraction prevention)
- ✅ Emergency stop button always visible
- ✅ Current location updates every 1-2 seconds (glanceable)
- ✅ Proximity alerts with audio + haptic (safety-critical)

See [VEHICLE_TABLET_UX_GUIDELINES.md](../VEHICLE_TABLET_UX_GUIDELINES.md) for complete design requirements.

**Deliverables:**
- [ ] `Views/LocationTrackingPage.xaml` + `.xaml.cs`
- [ ] BindingContext set to `LocationTrackingViewModel`
- [ ] UI layout (landscape-optimized):
  - [ ] Status indicator (top-left, always visible)
  - [ ] Current location display (large, glanceable)
  - [ ] GPS signal strength indicator
  - [ ] Proximity alerts section (large, high-contrast)
  - [ ] Emergency stop button (prominent, always visible)
- [ ] Navigation: Register in `AppShell.xaml.cs`
- [ ] Styling: High-contrast for vehicle use
- [ ] NO animations or keyboard auto-show
- [ ] Accessibility: Proper labels, WCAG AAA colors

**Landscape Layout Example:**
```
┌─────────────────────────────────┐
│ Status: Online   GPS: Strong ⚡ │  ← Header (18sp, always visible)
├─────────────────────────────────┤
│                                 │
│  CURRENT LOCATION              │
│  40.7128°N, -74.0060°W         │  ← Large (24sp), glanceable
│                                 │
│  ┌────────────────────────────┐ │
│  │ PROXIMITY ALERTS:          │ │
│  │ Order #1234  65 meters away│ │  ← High contrast, large
│  │                 [DISMISS]  │ │
│  └────────────────────────────┘ │
│                                 │
│  [EMERGENCY STOP]               │  ← Always visible & reachable
└─────────────────────────────────┘
```

---

#### TASK 3.10.5 — End-to-End Integration Tests (2 days)

**Objective:** Write automated E2E tests validating full tracking → proximity → notification flow

**Test Suite: `tabApp.Tests/IntegrationTests.cs` (new file)**

**Test Cases (minimum 10):**
1. Start Tracking → Location Update → Proximity Detection → Notification
2. App Restart → Notification State Persists
3. Date Boundary → Notification Resends
4. Stop Tracking → Cleanup
5. Permission Denied → Graceful Fallback

**Validation:**
- [ ] 10+ integration tests passing
- [ ] All major user flows covered
- [ ] No regressions in existing 70+ unit tests

---

#### TASK 3.10.6 — Device Testing (3 days)

**Objective:** Manual tests on real Android & iOS devices

**Android (Pixel 6, Android 14+):**
- [ ] Foreground Service starts
- [ ] Location updates in background
- [ ] Geofence trigger works
- [ ] Notification persists after restart
- [ ] Service stops cleanly
- [ ] Secondary permission dialog (Android 10+)

**iOS (iPhone 15, iOS 17+):**
- [ ] Background mode enabled
- [ ] Significant change updates work
- [ ] Geofence trigger works
- [ ] Foreground tracking (active use)
- [ ] Cleanup on stop (memory released)

**Validation:**
- [ ] All Android tests pass
- [ ] All iOS tests pass
- [ ] No crashes or exceptions
- [ ] No ANRs (Android) or hangs (iOS)

---

### Week 3: Performance & Documentation

#### TASK 3.10.7 — Performance & Battery Testing (2 days)

**Objective:** Validate battery drain, CPU usage, and memory consumption

**Battery Testing:**
- [ ] Baseline: 2% per hour idle
- [ ] Foreground: Battery drain with app active
- [ ] Background: Battery drain with app backgrounded + tracking ON
- [ ] Target: Background ≤ 5% per hour

**CPU Testing:**
- [ ] Location loop: CPU during continuous polling (<15%)
- [ ] Proximity calculation: CPU for O(n) filtering (<5%)
- [ ] Notification dedup: Preferences lookup (<1%)

**Memory Testing:**
- [ ] Baseline: App memory without location service
- [ ] With service: Delta ≤ 20 MB
- [ ] After stop: Memory released, no leaks
- [ ] 100-cycle test: Start → Stop → Memory stable

**Deliverable:**
- [ ] `docs/PERFORMANCE_TEST_RESULTS.md` (with numbers, graphs, conclusions)

---

#### TASK 3.10.8 — Code Review & Sign-Off (1 day)

**Objective:** Peer review all implementation, validate quality, obtain approval

**Reviewers:**
- [ ] Tech Lead (architecture compliance)
- [ ] Android Specialist (platform code)
- [ ] iOS Specialist (platform code)
- [ ] QA Lead (test coverage)

**Review Checklist:**
- [ ] All acceptance criteria met
- [ ] Code follows style guide
- [ ] No TODOs without tickets
- [ ] Test coverage >80%
- [ ] No performance regressions
- [ ] Documentation complete
- [ ] No blocking issues

**Approval Gate:**
- [ ] Tech Lead sign-off
- [ ] QA Lead sign-off
- [ ] PM approval

---

#### TASK 3.10.9 — Documentation Package (1 day)

**Objective:** Create comprehensive docs for developers and stakeholders

**Deliverables:**
- [ ] `IMPLEMENTATION_CHECKLIST.md` (detailed task breakdown)
- [ ] `TEST_STRATEGY.md` (QA guide)
- [ ] `PERFORMANCE_TEST_PLAN.md` (benchmark plan)
- [ ] `ROLLOUT_PLAN.md` (deployment strategy)
- [ ] Updated `README.md` with quick links
- [ ] Code comments & XML doc comments updated

---

## 🧪 Test Strategy Summary

### Test Pyramid
```
           △
          /|\  
         / | \  E2E (10 tests, 2 days)
        /  |  \
       / ─ ─ ─ \
      /Integration\ (10 tests, 1 day)
     / ─ ─ ─ ─ ─ ─ \
    / (70 Unit Tests) \
   /________________\_\
```

### Test Coverage by Component

| Component | Unit | Integration | Device | Target |
|-----------|------|-------------|--------|--------|
| IProximityService | 28 | 2 | 0 | >85% |
| LocationTrackingViewModel | 15 | 3 | 0 | >85% |
| PreferencesNotificationStateStore | 22 | 2 | 0 | >85% |
| LocationForegroundService | stub | 2 | 5 | >70% |
| BackgroundLocationTracker (iOS) | stub | 2 | 5 | >70% |

### Regression Testing
- Run after each major change
- Compare against baseline
- Alert if regression >10%

---

## 📊 Estimated Effort

| Task | Duration | Effort | Resource |
|------|----------|--------|----------|
| 3.10.1 Plugin | 2 days | 8h | 1 Dev |
| 3.10.2 Platform | 2 days | 10h | 2 Devs (Android + iOS) |
| 3.10.3 ViewModel | 2 days | 10h | 1 Dev |
| 3.10.4 UI Page | 2 days | 8h | 1 Dev + Designer |
| 3.10.5 E2E Tests | 2 days | 10h | 1 QA + 1 Dev |
| 3.10.6 Device Tests | 3 days | 15h | 2 QA + 2 Devs |
| 3.10.7 Performance | 2 days | 10h | 1 Mobile Specialist |
| 3.10.8 Review | 1 day | 4h | 4 Reviewers |
| 3.10.9 Documentation | 1 day | 4h | Tech Lead |
| **TOTAL** | **3 weeks** | **87 hours** | **Cross-functional team** |

---

## ✅ Success Criteria

All of the following must be true to mark TASK-3.10 COMPLETE:

1. ✅ All 9 subtasks completed (3.10.1 → 3.10.9)
2. ✅ 100+ unit tests passing (existing 70 + new 30+)
3. ✅ 10+ integration tests passing
4. ✅ 10+ device tests passed (Android + iOS)
5. ✅ Battery drain ≤ 5% per hour (background)
6. ✅ CPU usage ≤ 15% peak
7. ✅ Memory delta ≤ 20 MB
8. ✅ No critical bugs (blocker severity)
9. ✅ Code review approved by all reviewers
10. ✅ Documentation complete and published
11. ✅ Ready for TASK-3.11 (Final Documentation & Archive)

---

## 🔗 Next Steps

Upon completion of TASK-3.10:
1. Schedule sign-off meeting with stakeholders
2. Prepare TASK-3.11 handoff materials
3. Plan TASK-3.11 (Final Documentation & Archive)
4. Prepare release notes for stakeholders

---

## 📋 Key Implementation Decisions

### 1. Persistent Location State
**Recommendation:** MAUI Preferences for TASK-3.10
- Simple, no schema migration
- Sufficient for last-known location
- Upgrade to SQLite later if needed

### 2. Notification Plugin Choice
**Recommended:** `Plugin.LocalNotification` (NuGet)
- Cross-platform abstraction
- MAUI-native API surface
- Good community support
- MIT license

### 3. Android Secondary Permission Dialog UX
**Recommendation for TASK-3.10:**
- Display one-time intro screen
- User clicks "Enable Location" → system shows both dialogs
- Settings link: "You can change this in Settings"
- Graceful fallback: foreground-only if user denies

### 4. Platform-Specific Initialization Order
```csharp
// MauiProgram.cs initialization order (RECOMMENDED):
1. CreateMauiApp()
2. ConfigureFonts()
3. .ConfigureCoreServices()          // DIConfiguration
4. .UseLocalNotification()           // Plugin.LocalNotification
5. RegisterViewModels()
6. RegisterPages()
7. .Build()
8. App.StartLocationTracking()       // ViewModel/Service starts service
```

---

## ⚠️ Risk Mitigations (Implementation Phase)

| Risk | Mitigation | Owner |
|------|-----------|-------|
| **Plugin.LocalNotification** not compatible | Evaluate alternative: Xamarin.Essentials | Dev |
| **Platform services not ready** | Create minimal stubs, defer full impl | Tech Lead |
| **Battery drain exceeds target** | Tune update interval (2 sec → 5 sec) | Mobile Specialist |
| **iOS permissions dialog not appearing** | Review Info.plist keys, test on simulator | iOS Specialist |
| **Notification dedup has edge cases** | Expand unit tests, track in JIRA | QA |
| **Memory leak in LocationForegroundService** | Instrument all callbacks, add lifecycle tests | Android Specialist |

---

## 📍 Code Organization Recommendations

```
tabApp.CrossPlatform/
├── Services/
│   ├── Interfaces/
│   │   ├── Location/
│   │   │   ├── IBackgroundLocationTracker.cs (✅ DONE)
│   │   │   └── IProximityService.cs (✅ DONE)
│   │   └── Notifications/
│   │       ├── INotificationStateStore.cs (✅ DONE)
│   │       ├── IProximityNotificationService.cs (✅ DONE)
│   │       └── ILocalNotificationSender.cs (✅ DONE)
│   ├── Implementations/
│   │   ├── Location/
│   │   │   ├── ProximityService.cs (✅ DONE)
│   │   │   ├── HaversineCalculator.cs (✅ DONE)
│   │   │   └── BackgroundLocationStatus.cs (✅ DONE)
│   │   └── Notifications/
│   │       ├── PreferencesNotificationStateStore.cs (✅ DONE)
│   │       ├── ProximityNotificationService.cs (✅ DONE)
│   │       ├── DeduplicationKeyBuilder.cs (✅ DONE)
│   │       └── LocalNotificationSender/ (TBD: impl)
│   └── Location/
│       └── (legacy location helpers)
├── Platforms/
│   ├── Android/
│   │   ├── BackgroundLocationTracker.cs (✅ DONE)
│   │   ├── LocationForegroundService.cs (✅ DONE)
│   │   └── AndroidManifest.xml (✅ DONE)
│   └── iOS/
│       ├── BackgroundLocationTracker.cs (✅ DONE)
│       └── Info.plist (✅ DONE)
├── ViewModels/
│   ├── LocationTrackingViewModel.cs (📋 TBD: impl)
│   └── (others)
└── Views/
    ├── LocationTrackingPage.xaml (📋 TBD: impl)
    └── (others)
```

---

**Prepared by:** Tech Lead (Planning)  
**Date:** 2026-02-20  
**Status:** ✅ COMPLETE
