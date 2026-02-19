# ISSUE-001: Complete Dependency Audit and Compatibility Analysis

**Status:** 🔄 In Progress  
**Date:** 2026-02-19  
**Priority:** P0  
**Risk Level:** HIGH  
**Estimate:** 3-5 days

---

## 📋 Summary

Complete audit of all NuGet packages in tabApp.Droid and tabApp.Core to identify MAUI-compatible versions, required replacements, and obsolete dependencies.

**IMPORTANT:** Direct migration to tabApp.CrossPlatform:
- ✅ Migrate: tabApp.Core + tabApp.Droid
- ❌ Don't migrate: tabApp.DroidClients, tabApp.DroidWear

---

## 🎯 Objective

Create a comprehensive dependency compatibility matrix for Xamarin.Android → .NET MAUI migration, enabling informed decisions on package updates, replacements, and removals.

---

## 📊 Dependency Audit Results

### ✅ COMPATIBLE - NO CHANGES (12 packages)

| Package | Current | MAUI | Action |
|---------|---------|------|--------|
| Autofac | 6.1.0 | 6.1.0+ | Keep |
| BarcodeLib | 2.4.0 | 2.4.0+ | Keep |
| itext7 | 7.1.15 | 7.1.15+ | Keep |
| sqlite-net-pcl | 1.7.335 | 1.7.335+ | Keep |
| SQLiteNetExtensions | 2.1.0 | 2.1.0+ | Keep |
| Microsoft.AppCenter.Analytics | 4.5.0 | 4.5.0+ | Keep |
| Microsoft.AppCenter.Crashes | 4.5.0 | 4.5.0+ | Keep |
| Microsoft.AppCenter.Distribute | 4.5.0 | 4.5.0+ | Keep |
| Microsoft.AspNet.WebApi.Client | 5.2.7 | 5.2.7+ | Keep |
| Spire.XLS | 11.3.4 | 11.3.4+ | Keep |
| System.Formats.Asn1 | 5.0.0 | 5.0.0+ | Keep |
| System.Security.Cryptography.Xml | 5.0.0 | 5.0.0+ | Keep |

---

### ⚠️ REPLACE - MAUI ALTERNATIVES (8 packages)

| Current | Version | MAUI Alternative | Version | Notes |
|---------|---------|------------------|---------|-------|
| Xamarin.Essentials | 1.7.0 | Microsoft.Maui.Essentials | 8.0.0+ | Direct replacement |
| Com.Airbnb.Android.Lottie | 3.0.6 | SkiaSharp.Extended.UI.Maui | 1.0.0+ | Animation support |
| Microcharts | 0.9.5.9 | Microcharts.Maui | 0.10.0+ | Charts library |
| GooglePlayServices.Location | 1.0.0 | Microsoft.Maui.Essentials | Built-in | Geolocation |
| ZXing.Net | 0.16.6 | ZXing.Net.Maui | 1.0.0+ | Barcode scanning |
| Glide.Xamarin | 4.1.1 | FFImageLoading.Maui | 1.0.0+ | Image loading |
| Storm.AndroidPdfViewer | 2.8.2 | WebView + PDF.js | TBD | PDF viewing |
| FirebaseStorage.net | 1.0.3 | FirebaseStorage.CSharp | 4.0.0+ | Firebase storage |

---

### ❌ REMOVE - ANDROID SPECIFIC (8+ packages)

| Package | Reason |
|---------|--------|
| MvvmCross 6.2.3/6.4.1 | Framework-specific |
| MvvmCross.Droid.* | Framework-specific |
| Xamarin.Android.Support.* | Android-specific |
| Xamarin.AndroidX.* | Android-specific |
| Xamarin.GooglePlayServices.* | Android-specific |
| Plugin.CurrentActivity | Android-specific |
| Karamunting.Android.* | Android-specific |
| Mono.Android (reference) | Android-specific |

---

## 🔄 Migration Path

### Phase 1: Preparation (Day 1)
- [ ] Document all transitive dependencies
- [ ] Identify potential version conflicts
- [ ] Create dependency matrix spreadsheet
- [ ] Review breaking changes for each package

### Phase 2: Remove Android-Specific (Day 1-2)
- [ ] Remove MvvmCross packages
- [ ] Remove Xamarin.Android.Support.* packages
- [ ] Remove Xamarin.AndroidX.* packages
- [ ] Remove Plugin.CurrentActivity
- [ ] Test Core layer compilation

### Phase 3: Update Replacements (Day 2-3)
- [ ] Replace Xamarin.Essentials → Microsoft.Maui.Essentials
- [ ] Replace Lottie → SkiaSharp.Extended.UI.Maui
- [ ] Replace Microcharts → Microcharts.Maui
- [ ] Update ZXing.Net → ZXing.Net.Maui
- [ ] Update Firebase → FirebaseStorage.CSharp 4.0.0+
- [ ] Add FFImageLoading.Maui

### Phase 4: Update Framework (Day 3-4)
- [ ] Update tabApp.Core to .NET 8.0 (from netstandard2.0)
- [ ] Update all source code imports
- [ ] Test compilation for all platforms
- [ ] Validate no breaking changes

### Phase 5: Validation (Day 5)
- [ ] Core compiles without warnings
- [ ] MAUI targets compile (Android/iOS/Windows)
- [ ] All namespaces resolve correctly
- [ ] IntelliSense functions properly

---

## 📦 File Changes Required

### tabApp.Core.csproj
**NO CHANGES** - File remains as-is, source files will be copied to MAUI

### tabApp.Droid.csproj (tabApp/)
**NO CHANGES** - File remains as-is, source files will be copied to MAUI

### tabApp.DroidClients.csproj
**NO CHANGES** - Not part of this migration

### tabApp.DroidWear.csproj
**NO CHANGES** - Not part of this migration

### tabApp.CrossPlatform.csproj
**Change:** Add all dependencies and update references

```xml
<TargetFramework>net8.0</TargetFramework>

<!-- Add Compatible Packages -->
<PackageReference Include="Autofac" Version="6.1.0" />
<PackageReference Include="sqlite-net-pcl" Version="1.7.335" />
<!-- ... all compatible packages ... -->

<!-- Add MAUI Replacements -->
<PackageReference Include="Microsoft.Maui.Essentials" Version="8.0.0" />
<PackageReference Include="Microcharts.Maui" Version="0.10.0" />

<!-- Add MAUI UI Libraries -->
<PackageReference Include="SkiaSharp.Extended.UI.Maui" Version="1.0.0" />
<PackageReference Include="ZXing.Net.Maui" Version="1.0.0" />
<PackageReference Include="FFImageLoading.Maui" Version="1.0.0" />
```

---

## ⚠️ Critical Risks

| Risk | Impact | Mitigation |
|------|--------|------------|
| **PDF Viewer Replacement** | HIGH | Evaluate Syncfusion/WebView, plan fallback |
| **Breaking API Changes** | MEDIUM | Test each package update, document changes |
| **Lottie Performance** | MEDIUM | Benchmark SkiaSharp vs Com.Airbnb |
| **Firebase Storage** | LOW | Test data compatibility with new version |
| **MvvmCross Removal** | CRITICAL | Depends on ISSUE-002 (Architecture) |

---

## 🔗 Dependencies

- ✅ None (First issue in migration)
- ⬇️ Blocks: ISSUE-002, ISSUE-003, ISSUE-004

---

## 📋 Deliverables

- [ ] `docs/tech/issue-001/DEPENDENCIES_MATRIX.md` - Complete audit spreadsheet
- [ ] `docs/tech/issue-001/MIGRATION_STRATEGY.md` - Detailed migration plan
- [ ] `docs/tech/issue-001/BREAKING_CHANGES.md` - API changes documentation
- [ ] Updated `tabApp.Core.csproj` with new dependencies
- [ ] Updated `tabApp.CrossPlatform.csproj` with MAUI packages
- [ ] No compilation warnings/errors on all platforms

---

## ✅ Definition of Done

- [ ] All packages audited and categorized
- [ ] MAUI alternatives identified and researched
- [ ] Compatibility matrix created
- [ ] Breaking changes documented
- [ ] Migration strategy approved by team
- [ ] Proof of concept: Core compiles in .NET 8.0
- [ ] All new dependencies resolved correctly
- [ ] Code compiles for Android/iOS/Windows targets

---

## 📚 References

- [MAUI Essentials](https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/)
- [SkiaSharp Extended](https://github.com/skia-sharp/SkiaSharp)
- [ZXing.Net.Maui](https://github.com/thudsonlu/ZXing.Net.Maui)
- [Firebase CSharp SDK](https://github.com/googleapis/firebase-admin-dotnet)

---

**Created:** 2026-02-19  
**Last Updated:** 2026-02-19  
**Assigned to:** Development Team






