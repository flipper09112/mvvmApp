# 🎯 MAUI Migration Backlog - tabApp.Droid → tabApp.CrossPlatform

**Generated:** 2026-02-19  
**Project:** tabApp - Fleet Management & Delivery Application  
**Source:** Xamarin.Android (tabApp.Droid)  
**Target:** .NET MAUI (tabApp.CrossPlatform)

---

## 📊 Migration Overview

### Component Summary
- **Activities:** 1
- **Fragments:** 51
- **Adapters:** 42
- **ViewHolders:** ~30
- **Services:** 4 platform-specific
- **ViewModels:** 47 (Core - reusable)
- **Total Issues:** 142

### Risk Distribution
- **P0 Critical:** 18 issues
- **P1 High:** 45 issues
- **P2 Medium:** 52 issues
- **P3 Low:** 27 issues

### Phase Distribution
- **Phase 0 - Assessment:** 8 issues
- **Phase 1 - MAUI Base Setup:** 12 issues
- **Phase 2 - Core Layer Migration:** 15 issues
- **Phase 3 - Infrastructure Migration:** 22 issues
- **Phase 4 - UI Migration:** 51 issues
- **Phase 5 - Feature Migration:** 24 issues
- **Phase 6 - Testing & Hardening:** 8 issues
- **Phase 7 - Release & Go-Live:** 2 issues

---

## 🏗 Milestones

### Milestone 1: Assessment & Planning
**Duration:** 1 week  
**Issues:** 8

### Milestone 2: MAUI Foundation
**Duration:** 2 weeks  
**Issues:** 12

### Milestone 3: Core & Infrastructure
**Duration:** 3 weeks  
**Issues:** 37

### Milestone 4: UI Layer Migration
**Duration:** 6 weeks  
**Issues:** 51

### Milestone 5: Feature Completion
**Duration:** 3 weeks  
**Issues:** 24

### Milestone 6: Testing & Release
**Duration:** 2 weeks  
**Issues:** 10

**Total Estimated Duration:** 17 weeks

---

## 📋 Issues Breakdown

Issues are organized by milestone and phase. Each issue includes:
- Objective
- Current Implementation
- Migration Strategy
- Risks
- Dependencies
- Definition of Done

See individual issue files in `docs/issues/` for complete details.

---

## 🔗 Quick Links

- [Phase 0 Issues](./issues/phase-0-assessment/)
- [Phase 1 Issues](./issues/phase-1-maui-setup/)
- [Phase 2 Issues](./issues/phase-2-core-migration/)
- [Phase 3 Issues](./issues/phase-3-infrastructure/)
- [Phase 4 Issues](./issues/phase-4-ui-migration/)
- [Phase 5 Issues](./issues/phase-5-features/)
- [Phase 6 Issues](./issues/phase-6-testing/)
- [Phase 7 Issues](./issues/phase-7-release/)

---

## 📌 Critical Path

The following issues are on the critical path and must be completed in sequence:

1. **ISSUE-001:** Assessment - Dependency Audit
2. **ISSUE-009:** MAUI Project Structure Setup
3. **ISSUE-010:** Dependency Injection Configuration
4. **ISSUE-011:** Navigation Shell Setup
5. **ISSUE-024:** Core Layer - Service Interfaces Migration
6. **ISSUE-027:** SQLite Service Migration
7. **ISSUE-030:** Authentication & Secure Storage
8. **ISSUE-033:** Foreground Service → Background Task
9. **ISSUE-060:** MainActivity → Shell Navigation
10. **ISSUE-084:** Home Feature Complete
11. **ISSUE-130:** Integration Testing
12. **ISSUE-140:** Production Release

---

## 🚨 High-Risk Areas

### Security Components
- Authentication flow
- Secure storage implementation
- Bluetooth data transfer
- File encryption

### Platform Services
- Foreground location service
- Bluetooth manager service
- Background notifications
- Permission handling

### Complex UI
- Map integration (Google Maps → MAUI Maps)
- ViewPager implementations
- Custom adapters with swipe gestures
- PDF viewing

---

## 📚 Reference Documentation

- [MAUI Migration Guide](./MAUI_MIGRATION_GUIDE.md)
- [Architecture Decisions](./ARCHITECTURE_DECISIONS.md)
- [API Changes Reference](./API_CHANGES_REFERENCE.md)
- [Testing Strategy](./TESTING_STRATEGY.md)


