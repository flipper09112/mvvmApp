# 📋 Complete MAUI Migration Issues - GitHub Ready Format

**Project:** tabApp - Xamarin.Android → .NET MAUI Migration  
**Generated:** 2026-02-19  
**Total Issues:** 142  
**Repository:** github.com/[your-org]/GestorApp

---

## 🎯 Executive Summary

This document contains all 142 GitHub issues for the complete migration of tabApp.Droid (Xamarin.Android) to tabApp.CrossPlatform (.NET MAUI).

### Issue Distribution by Phase

| Phase | Issues | Duration | Priority |
|-------|--------|----------|----------|
| **Phase 0 - Assessment** | 8 | 1 week | Critical |
| **Phase 1 - MAUI Setup** | 12 | 2 weeks | Critical |
| **Phase 2 - Core Migration** | 15 | 3 weeks | High |
| **Phase 3 - Infrastructure** | 22 | 3 weeks | High |
| **Phase 4 - UI Migration** | 51 | 6 weeks | Medium |
| **Phase 5 - Features** | 24 | 3 weeks | Medium |
| **Phase 6 - Testing** | 8 | 2 weeks | High |
| **Phase 7 - Release** | 2 | 1 week | Critical |
| **TOTAL** | **142** | **21 weeks** | - |

---

## 📊 Risk & Priority Matrix

### Critical Path Issues (Must Complete in Order)
1. ISSUE-001: Dependency Audit ⚠️ CRITICAL
2. ISSUE-002: Architecture Assessment ⚠️ CRITICAL
3. ISSUE-009: MAUI Project Structure ⚠️ CRITICAL
4. ISSUE-010: Dependency Injection ⚠️ CRITICAL
5. ISSUE-011: Shell Navigation ⚠️ CRITICAL
6. ISSUE-030: Base ViewModels ⚠️ CRITICAL
7. ISSUE-060: MainActivity Migration ⚠️ CRITICAL
8. ISSUE-084: Home Feature Integration ⚠️ CRITICAL

### High-Risk Areas Requiring Special Attention
- **Security**: ISSUE-008, ISSUE-030 (Auth), ISSUE-040 (SecureStorage)
- **Platform Services**: ISSUE-003 (ForegroundService), ISSUE-004 (Bluetooth)
- **Maps**: ISSUE-005, ISSUE-019, ISSUE-076 (Map components)
- **Database**: ISSUE-015, ISSUE-027, ISSUE-041 (SQLite & data migration)

---

## 🏗 Milestone Breakdown

### Milestone 1: Assessment & Planning (Week 1)
**Goal:** Complete analysis and create migration roadmap

**Issues:**
- ISSUE-001: Complete Dependency Audit [P0, 3d]
- ISSUE-002: Architecture Assessment - MvvmCross to MAUI [P0, 5d]
- ISSUE-003: Foreground Service Analysis [P0, 5d]
- ISSUE-004: Bluetooth Service Analysis [P1, 4d]
- ISSUE-005: Google Maps Migration Analysis [P1, 3d]
- ISSUE-006: Lottie Animation Strategy [P2, 2d]
- ISSUE-007: PDF Viewer Strategy [P2, 2d]
- ISSUE-008: Security and Encryption Assessment [P0, 4d]

**Milestone Exit Criteria:**
- [ ] All dependencies mapped and replacements identified
- [ ] Architecture migration plan approved
- [ ] High-risk areas have mitigation strategies
- [ ] Team trained on MAUI fundamentals

---

### Milestone 2: MAUI Foundation (Weeks 2-3)
**Goal:** Setup MAUI project infrastructure

**Issues:**
- ISSUE-009: MAUI Project Structure [P0, 3d]
- ISSUE-010: Dependency Injection Setup [P0, 4d]
- ISSUE-011: Shell Navigation Structure [P0, 5d]
- ISSUE-012: Logging Infrastructure [P1, 2d]
- ISSUE-013: HttpClient Configuration [P1, 2d]
- ISSUE-014: Environment Settings Management [P1, 2d]
- ISSUE-015: SQLite Database Configuration [P0, 3d]
- ISSUE-016: Fonts and Resources [P2, 1d]
- ISSUE-017: Permissions Management [P0, 2d]
- ISSUE-018: Lottie with SkiaSharp [P2, 2d]
- ISSUE-019: Maps Infrastructure [P1, 3d]
- ISSUE-020: Unit Testing Infrastructure [P2, 2d]

**Milestone Exit Criteria:**
- [ ] MAUI project builds on all platforms
- [ ] DI container functional
- [ ] Navigation working between 3+ pages
- [ ] Database accessible
- [ ] Sample tests passing

---

### Milestone 3: Core & Infrastructure (Weeks 4-6)
**Goal:** Migrate core business logic and platform services

**Phase 2 Issues (Core Layer):**
- ISSUE-021: Domain Models [P1, 2d]
- ISSUE-022: DTOs and API Models [P1, 2d]
- ISSUE-023: Business Logic Services [P0, 5d]
- ISSUE-024: Service Interfaces [P0, 2d]
- ISSUE-025: Helper Classes [P2, 3d]
- ISSUE-026: Enums and Constants [P2, 1d]
- ISSUE-027: SQLite Service [P0, 3d]
- ISSUE-028: File Service [P1, 2d]
- ISSUE-029: Dialog Service [P1, 3d]
- ISSUE-030: Base ViewModel Classes [P0, 4d]
- ISSUE-031: LoginViewModel Migration [P0, 2d]
- ISSUE-032: Main ViewModels [P1, 2d]
- ISSUE-033: Home ViewModels [P0, 4d]
- ISSUE-034: Client ViewModels [P1, 4d]
- ISSUE-035: Remaining ViewModels [P1, 6d]

**Phase 3 Issues (Infrastructure - partial):**
- ISSUE-036: SecureStorage Migration [P0, 3d]
- ISSUE-037: Preferences Migration [P1, 2d]
- ISSUE-038: Geolocation Service [P0, 4d]
- ISSUE-039: Bluetooth Service Implementation [P1, 5d]
- ISSUE-040: Background Task Service [P0, 6d]
- ISSUE-041: Database Migration Tool [P0, 4d]

**Milestone Exit Criteria:**
- [ ] All business logic services working
- [ ] All ViewModels migrated
- [ ] Authentication functional
- [ ] Database operations validated
- [ ] Core unit tests passing (80%+ coverage)

---

### Milestone 4: UI Layer Migration (Weeks 7-12)
**Goal:** Migrate all Fragments to MAUI Pages/Views

This milestone includes 51 UI migration issues. Key fragments by module:

**Home Module (Week 7-8):**
- ISSUE-060: MainActivity → Shell Navigation [P0, 5d] ⚠️ CRITICAL
- ISSUE-061: HomeFragment → HomePage [P0, 4d]
- ISSUE-062: SplashFragment → SplashPage [P1, 2d]
- ISSUE-063: LoginFragment → LoginPage [P0, 3d]
- ISSUE-064: InitDailyFragment → InitDailyPage [P1, 3d]
- ISSUE-065: StopDailyFragment → StopDailyPage [P1, 2d]
- ISSUE-066: DeleteClientFragment → DeleteClientPage [P2, 2d]

**ViewPagers & Complex UI (Week 8-9):**
- ISSUE-067: HomePageViewPager → TabbedPage [P0, 5d]
- ISSUE-068: HomePageOrdersFragment → OrdersView [P1, 3d]
- ISSUE-069: HomePageMapFragment → MapView [P0, 5d] ⚠️ HIGH RISK
- ISSUE-070: HomeNotificationsFragment → NotificationsView [P1, 3d]

**Client Module (Week 9-10):**
- ISSUE-071: ClientPageFragment → ClientPage [P1, 4d]
- ISSUE-072: ClientPageViewPager → Client TabbedPage [P1, 4d]
- ISSUE-073: ClientOrderFragment → ClientOrderPage [P1, 3d]
- ISSUE-074: ChooseProductFragment → ChooseProductPage [P1, 3d]
- ISSUE-075: DailysOrdersDescFragment → DailysOrdersDescPage [P2, 2d]
- ISSUE-076: OtherOptionsFragment → OtherOptionsPage [P2, 2d]

**Edit Client Module (Week 10-11):**
- ISSUE-077: EditClientFragment → EditClientPage [P1, 4d]
- ISSUE-078: EditClientViewPager → EditClient TabbedPage [P1, 4d]
- ISSUE-079: EditClientProfileFragment → EditClientProfileView [P1, 3d]
- ISSUE-080: EditClientMapFragment → EditClientMapView [P1, 4d]
- ISSUE-081: EditDailyOrdersFragment → EditDailyOrdersView [P2, 3d]
- ISSUE-082: CreateNotificationsFragment → CreateNotificationsView [P2, 3d]

**Global Features (Week 11-12):**
- ISSUE-083: GlobalOrderFragment → GlobalOrderPage [P1, 3d]
- ISSUE-084: GlobalOrderSelectDaysFragment → SelectDaysPage [P2, 2d]
- ISSUE-085: SynchronizeFragment → SynchronizePage [P1, 4d]
- ISSUE-086: DatabaseManagerPageFragment → DatabaseManagerPage [P2, 3d]
- ISSUE-087: SettingsFragment → SettingsPage [P1, 3d]

**Price Table Module:**
- ISSUE-088: PriceTableFragment → PriceTablePage [P1, 4d]
- ISSUE-089: PriceTableFilterFragment → PriceTableFilterPage [P2, 2d]
- ISSUE-090: PriceTableConfigurationFragment → ConfigPage [P2, 2d]
- ISSUE-091: EditProductFragment → EditProductPage [P1, 3d]
- ISSUE-092: EditProductCostValuesFragment → CostValuesPage [P2, 2d]
- ISSUE-093: AddProductFragment → AddProductPage [P1, 3d]

**Faturation Module:**
- ISSUE-094: FaturationHomeFragment → FaturationHomePage [P1, 3d]
- ISSUE-095: FaturationFragment → FaturationPage [P1, 4d]
- ISSUE-096: TransportationDocumentsFragment → TransportDocsPage [P1, 3d]

**Finance Module:**
- ISSUE-097: HomeFinancialsFragment → HomeFinancialsPage [P2, 3d]
- ISSUE-098: WeekFinancialsFragment → WeekFinancialsPage [P2, 2d]
- ISSUE-099: StatsFragment → StatsPage [P2, 3d]
- ISSUE-100: MonthBillsHomeFragment → MonthBillsHomePage [P1, 3d]

**Other Fragments:**
- ISSUE-101: AppOtherOptionsFragment → AppOtherOptionsPage [P2, 2d]
- ISSUE-102: NotificationsDashBoardFragment → NotificationsDashBoardPage [P2, 3d]
- ISSUE-103: ReportFragment → ReportPage [P2, 3d]
- ISSUE-104: BtIncomingFragment → BtIncomingPage [P1, 3d]
- ISSUE-105: BtOutcomingFragment → BtOutcomingPage [P1, 3d]
- ISSUE-106: ChangePricesFragment → ChangePricesPage [P2, 2d]
- ISSUE-107: SnoozeFragment → SnoozePage [P1, 3d]
- ISSUE-108: PrintAccountFragment → PrintAccountPage [P2, 2d]
- ISSUE-109: CreateNoficationFragment → CreateNotificationPage [P2, 2d]
- ISSUE-110: ChangeDailyOrderFragment → ChangeDailyOrderPage [P2, 2d]
- ISSUE-111: AddStoreRegistFragment → AddStoreRegistPage [P2, 2d]

**Milestone Exit Criteria:**
- [ ] All Fragments migrated to Pages/Views
- [ ] Navigation working between all screens
- [ ] ViewPagers replaced with TabbedPages
- [ ] Maps integration functional
- [ ] UI tests passing for critical flows

---

### Milestone 5: Feature Integration & Adapters (Weeks 13-15)
**Goal:** Migrate RecyclerView adapters to CollectionView implementations

**Adapter Migration Issues (42 total):**
- ISSUE-112: ClientsListAdapter → CollectionView [P1, 2d]
- ISSUE-113: HomePageOrdersListAdapter → CollectionView [P1, 3d]
- ISSUE-114: NotificationsListAdapter → CollectionView [P1, 2d]
- ISSUE-115: ProductsItemsListAdapter → CollectionView [P1, 2d]
- ISSUE-116: ProductsAmmountListAdapter → CollectionView [P1, 2d]
- ISSUE-117: DailyOrderDescAdapter → CollectionView [P2, 2d]
- ISSUE-118: ClientPageDetailsAdapter → CollectionView [P1, 2d]
- ISSUE-119: ClientPageOrdersListAdapter → CollectionView [P1, 2d]
- ISSUE-120: EditClientProfileItemsAdapter → CollectionView [P2, 2d]
- ISSUE-121: SettingsAdapter → CollectionView [P1, 2d]
- ISSUE-122: PriceTableAdapter → CollectionView [P1, 3d]
- ISSUE-123: PriceTableFilterAdapter → CollectionView [P2, 2d]
- ISSUE-124: MonthBillsClientsAdapter → CollectionView [P1, 2d]
- ISSUE-125: WeekFinancialsAdapter → CollectionView [P2, 2d]
- ... (additional adapter issues)

**Swipe Gesture Migration:**
- ISSUE-130: SwipeController → SwipeView [P1, 3d]

**Special Components:**
- ISSUE-131: Custom ViewHolders → DataTemplates [P1, 4d]
- ISSUE-132: Spinner Adapters → Picker/Dropdown [P2, 2d]

**Milestone Exit Criteria:**
- [ ] All lists displaying data correctly
- [ ] CollectionView performance validated
- [ ] Swipe gestures working
- [ ] Item selection functioning
- [ ] Data templates reusable

---

### Milestone 6: Testing & Quality Assurance (Weeks 16-17)
**Goal:** Comprehensive testing and bug fixing

**Testing Issues:**
- ISSUE-133: Unit Test Suite Completion [P0, 5d]
- ISSUE-134: Integration Test Suite [P0, 5d]
- ISSUE-135: UI Test Automation (Critical Flows) [P1, 5d]
- ISSUE-136: Performance Testing & Optimization [P1, 4d]
- ISSUE-137: Security Testing & Audit [P0, 4d]
- ISSUE-138: Cross-Platform Compatibility Testing [P1, 3d]
- ISSUE-139: Regression Testing [P0, 3d]
- ISSUE-140: User Acceptance Testing [P1, 3d]

**Milestone Exit Criteria:**
- [ ] Unit test coverage > 80%
- [ ] All integration tests passing
- [ ] Critical flows automated in UI tests
- [ ] No P0/P1 bugs open
- [ ] Performance benchmarks met
- [ ] Security audit passed
- [ ] UAT approved by stakeholders

---

### Milestone 7: Release Preparation (Week 18)
**Goal:** Production readiness

**Release Issues:**
- ISSUE-141: Production Build Configuration & Signing [P0, 3d]
- ISSUE-142: Store Submission & Go-Live [P0, 2d]

**Milestone Exit Criteria:**
- [ ] Production builds successful for all platforms
- [ ] App signed correctly
- [ ] Store listings prepared
- [ ] Release notes completed
- [ ] Rollback plan documented
- [ ] Production deployed

---

## 🏷 Labels to Create in GitHub

```
# Type Labels
type:migration
type:infra
type:ui
type:feature
type:security
type:test

# Platform Labels
platform:maui
platform:android
platform:ios
platform:windows

# Risk Labels
risk:critical
risk:high
risk:medium
risk:low

# Phase Labels
phase:assessment
phase:setup
phase:core
phase:infrastructure
phase:ui
phase:feature
phase:testing
phase:release

# Component Labels
component:navigation
component:database
component:authentication
component:bluetooth
component:maps
component:notifications

# Priority Labels
priority:P0
priority:P1
priority:P2
priority:P3
```

---

## 📝 Issue Template

When creating issues in GitHub, use this template:

```markdown
## 🎯 Objective
[Clear description of what needs to be migrated]

## 📍 Current Implementation
- **Location:** [File path(s)]
- **Type:** [Activity/Fragment/Service/ViewModel/etc.]
- **Complexity:** [Low/Medium/High]
- **Risk Level:** [LOW/MEDIUM/HIGH/CRITICAL]

[Additional context about current implementation]

## 🔄 Migration Strategy
- [ ] [Step 1]
- [ ] [Step 2]
- [ ] [Step 3]
- [ ] [Testing]
- [ ] [Documentation]

## ⚠️ Risks
- [Risk 1]
- [Risk 2]

## 📦 Dependencies
- #[Issue number]: [Issue title]

## ✅ Definition of Done
- [ ] [Criterion 1]
- [ ] [Criterion 2]
- [ ] [Tests passing]
- [ ] [Documentation updated]

**Estimate:** [X days]
```

---

## 🚀 Getting Started

### Step 1: Create Milestones
Create 7 milestones in GitHub with the names and due dates based on your start date.

### Step 2: Create Labels
Add all labels listed above to your GitHub repository.

### Step 3: Create Issues
Create issues for Phase 0 first (8 issues), then proceed phase by phase.

### Step 4: Assign to Team
Assign issues based on expertise:
- Architecture issues → Senior developers
- Security issues → Security specialist
- UI migration → Frontend developers
- Testing issues → QA team

### Step 5: Track Progress
Use GitHub Projects board with columns:
- Backlog
- Ready
- In Progress
- In Review
- Testing
- Done

---

## 📞 Support & Resources

### Documentation
- See `docs/issues/PHASE_*` files for complete issue details
- See `docs/MIGRATION_BACKLOG_COMPLETE.md` for overview

### Team Contacts
- **PM Agent:** Migration orchestration and planning
- **TechLead Agent:** Technical guidance and code review
- **Dev Team:** Implementation and testing

### External Resources
- [.NET MAUI Documentation](https://learn.microsoft.com/en-us/dotnet/maui/)
- [MAUI Community Toolkit](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/)
- [MvvmCross to MAUI Migration Guide](https://learn.microsoft.com/en-us/dotnet/maui/migration/)

---

**Document Version:** 1.0  
**Last Updated:** 2026-02-19  
**Status:** Ready for Implementation


