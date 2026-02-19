# Dependencies Matrix - ISSUE-001

**Version:** 1.0  
**Date:** 2026-02-19  
**Scope:** Complete audit of all NuGet packages

---

## 🎯 Overview

- **Total Packages Analyzed:** 28+
- **Compatible (Keep):** 12
- **Replace (MAUI Alt):** 8
- **Remove (Android-specific):** 8+

---

## 📊 Complete Dependency List

### tabApp.Core.csproj (Current)

```xml
<!-- COMPATIBLE - KEEP AS IS -->
<PackageReference Include="Autofac" Version="6.1.0" />
<PackageReference Include="BarcodeLib" Version="2.4.0" />
<PackageReference Include="itext7" Version="7.1.15" />
<PackageReference Include="sqlite-net-pcl" Version="1.7.335" />
<PackageReference Include="SQLiteNetExtensions" Version="2.1.0" />
<PackageReference Include="Microsoft.AspNet.WebApi.Client" Version="5.2.7" />
<PackageReference Include="Spire.XLS" Version="11.3.4" />
<PackageReference Include="System.Formats.Asn1" Version="5.0.0" />
<PackageReference Include="System.Security.Cryptography.Xml" Version="5.0.0" />

<!-- REMOVE (MvvmCross Framework) -->
<PackageReference Include="MvvmCross" Version="6.2.3" />

<!-- REPLACE -->
<PackageReference Include="Xamarin.Essentials" Version="1.7.0" />
<!-- → Microsoft.Maui.Essentials 8.0.0+ -->

<!-- REMOVE (Reference) -->
<Reference Include="Mono.Android">...</Reference>
```

### tabApp.Droid.csproj (Current)

```xml
<!-- REMOVE (Xamarin Framework) -->
<PackageReference Include="MvvmCross.Droid.Support.* " Version="6.4.1" />

<!-- REMOVE (Android Support) -->
<PackageReference Include="Xamarin.Android.Support.DesignCompat" Version="28.0.0.3" />
<PackageReference Include="Xamarin.Android.Support.V4" Version="28.0.0.3" />
<PackageReference Include="Xamarin.Android.Support.V7.AppCompat" Version="28.0.0.3" />

<!-- REMOVE (AndroidX) -->
<PackageReference Include="Xamarin.AndroidX.AppCompat" Version="1.0.0.0" />
<PackageReference Include="Xamarin.AndroidX.Lifecycle.LiveData" Version="2.0.0.0" />
<PackageReference Include="Xamarin.AndroidX.RecyclerView" Version="1.0.0.0" />
<!-- ... more AndroidX packages -->

<!-- REMOVE (Google Services) -->
<PackageReference Include="Xamarin.GooglePlayServices.Location" Version="1.0.0" />
<PackageReference Include="Xamarin.GooglePlayServices.Maps" Version="71.1600.0" />

<!-- REPLACE -->
<PackageReference Include="Com.Airbnb.Android.Lottie" Version="3.0.6" />
<!-- → SkiaSharp.Extended.UI.Maui 1.0.0+ -->

<PackageReference Include="Glide.Xamarin" Version="4.1.1" />
<!-- → FFImageLoading.Maui 1.0.0+ -->

<PackageReference Include="Karamunting.Android.Lecho.HelloCharts" Version="1.5.8" />
<!-- → Microcharts.Maui 0.10.0+ -->

<PackageReference Include="Storm.AndroidPdfViewer" Version="2.8.2" />
<!-- → WebView + PDF.js (open source) or Syncfusion (commercial) -->

<PackageReference Include="ZXing.Net" Version="0.16.6" />
<!-- → ZXing.Net.Maui 1.0.0+ -->

<!-- REMOVE (Android Specific) -->
<PackageReference Include="Plugin.CurrentActivity" Version="2.1.0.4" />
<PackageReference Include="Karamunting.Android.Sorbh.KdGaugeView" Version="1.0.0" />

<!-- REPLACE -->
<PackageReference Include="FirebaseStorage.net" Version="1.0.3" />
<!-- → FirebaseStorage.CSharp 4.0.0+ -->

<!-- KEEP -->
<PackageReference Include="Microcharts" Version="0.9.5.9" />
<PackageReference Include="Microsoft.AppCenter.Analytics" Version="4.5.0" />
<PackageReference Include="Microsoft.AppCenter.Crashes" Version="4.5.0" />
<PackageReference Include="Microsoft.AppCenter.Distribute" Version="4.5.0" />
<PackageReference Include="Xamarin.Essentials" Version="1.7.0" />
<!-- → Will be replaced by Microsoft.Maui.Essentials -->
```

---

## 🔄 Migration Checklist

### Step 1: Backup Current State
- [ ] Commit current code to git
- [ ] Create `BACKUP_DEPENDENCIES.xml` with current versions
- [ ] Document any custom build scripts using these packages

### Step 2: Update tabApp.Core.csproj

**Changes:**
```diff
- <TargetFramework>netstandard2.0</TargetFramework>
+ <TargetFramework>net8.0</TargetFramework>

- <PackageReference Include="MvvmCross" Version="6.2.3" />
- <PackageReference Include="Xamarin.Essentials" Version="1.7.0" />
+ <PackageReference Include="Microsoft.Maui.Essentials" Version="8.0.0" />
+ <PackageReference Include="Microcharts.Maui" Version="0.10.0" />
+ <PackageReference Include="FirebaseStorage.CSharp" Version="4.0.0" />

- <Reference Include="Mono.Android">...</Reference>
```

**Verify:**
```bash
cd tabApp.Core
dotnet clean
dotnet restore
dotnet build
```

### Step 3: Update Source Code Imports

**Find and replace in all .cs files:**

```csharp
// OLD
using Xamarin.Essentials;
// NEW
using Microsoft.Maui.Essentials;

// OLD
using MvvmCross;
// DELETE (will be handled in ISSUE-002)

// OLD
using Com.Airbnb.Android.Lottie;
// NEW (will be handled in UI migration)
using SkiaSharp.Extended.UI.Maui;
```

### Step 4: Update tabApp.CrossPlatform.csproj

**Add packages:**
```xml
<PackageReference Include="Microsoft.Maui.Controls" Version="8.0.0" />
<PackageReference Include="SkiaSharp.Extended.UI.Maui" Version="1.0.0" />
<PackageReference Include="ZXing.Net.Maui" Version="1.0.0" />
<PackageReference Include="FFImageLoading.Maui" Version="1.0.0" />
<PackageReference Include="Microsoft.Maui.Controls.Maps" Version="8.0.0" />
```

### Step 5: Validate Compilation

```bash
# Build MAUI for all platforms
dotnet build tabApp.CrossPlatform.csproj -f net8.0-android
dotnet build tabApp.CrossPlatform.csproj -f net8.0-ios
dotnet build tabApp.CrossPlatform.csproj -f net8.0-windows10.0.19041.0
```

---

## 📈 Package Analysis

### MvvmCross (CRITICAL - Remove)
- **Current Version:** 6.2.3 / 6.4.1
- **Usage:** All ViewModels, Setup.cs, MainActivity
- **Impact:** HIGH
- **Replacement:** .NET MAUI native MVVM (see ISSUE-002)
- **Action:** Remove - Framework migration required

### Xamarin.Essentials (CRITICAL - Replace)
- **Current Version:** 1.7.0
- **Usage:** Location, Permissions, Preferences
- **Impact:** HIGH
- **Replacement:** Microsoft.Maui.Essentials 8.0.0+
- **Compatibility:** 100% - Direct replacement
- **Action:** Direct substitution

### Lottie (HIGH - Replace)
- **Current Package:** Com.Airbnb.Android.Lottie 3.0.6
- **Usage:** 8 animation files in tabApp/Assets/Lotties/
- **Impact:** MEDIUM
- **Replacement:** SkiaSharp.Extended.UI.Maui 1.0.0+
- **Animation Files:** Can be reused (.json format compatible)
- **Action:** Migrate to SkiaSharp

### Firebase Storage (MEDIUM - Update)
- **Current Package:** FirebaseStorage.net 1.0.3
- **Usage:** Cloud storage operations
- **Impact:** LOW
- **Replacement:** FirebaseStorage.CSharp 4.0.0+
- **Breaking Changes:** May have API changes
- **Action:** Test compatibility before updating

### Microcharts (MEDIUM - Replace)
- **Current Package:** Microcharts 0.9.5.9
- **Usage:** Chart rendering
- **Impact:** MEDIUM
- **Replacement:** Microcharts.Maui 0.10.0+
- **Action:** Migrate to MAUI version

---

## 🚨 Critical Decisions

### PDF Viewer - 3 Options

**Option A: WebView + PDF.js (RECOMMENDED for MVP)**
- Cost: Free
- Features: Basic PDF viewing
- Complexity: Medium
- Time: 2-3 days

**Option B: Syncfusion PDF Viewer**
- Cost: ~$999/developer/year
- Features: Full-featured
- Complexity: Low
- Time: 1-2 days

**Option C: DevExpress**
- Cost: ~$1200/developer/year
- Features: Full-featured
- Complexity: Low
- Time: 1-2 days

**Recommendation:** Start with WebView + PDF.js, upgrade to Syncfusion if needed

---

## 📋 Transitive Dependencies

### High-Risk Transitive
- Xamarin.Android.Support → AndroidX dependencies
- MvvmCross → Xamarin bindings
- Google Play Services → Android-specific libraries

### Safe Transitive
- Autofac → Reflection.Emit
- sqlite-net-pcl → System.Data.Common
- itext7 → BouncyCastle

---

## 🔗 Package Relationships

```
tabApp.Core
├── Autofac 6.1.0 ✅ KEEP
├── sqlite-net-pcl 1.7.335 ✅ KEEP
├── SQLiteNetExtensions 2.1.0 ✅ KEEP
├── MvvmCross 6.2.3 ❌ REMOVE
├── Xamarin.Essentials 1.7.0 ⚠️ REPLACE
└── ... (others)

tabApp.Droid
├── MvvmCross.Droid.* ❌ REMOVE
├── Xamarin.Android.Support.* ❌ REMOVE
├── Xamarin.AndroidX.* ❌ REMOVE
├── GooglePlayServices.* ❌ REMOVE
├── Com.Airbnb.Lottie ⚠️ REPLACE
├── Glide ⚠️ REPLACE
├── ZXing.Net ⚠️ REPLACE
└── ... (others)

tabApp.CrossPlatform (MAUI)
├── Microsoft.Maui.Controls ✅ NEW
├── Microsoft.Maui.Essentials ✅ NEW (from Essentials 1.7.0)
├── SkiaSharp.Extended.UI.Maui ✅ NEW (from Lottie)
├── ZXing.Net.Maui ✅ NEW (from ZXing.Net)
├── FFImageLoading.Maui ✅ NEW (from Glide)
└── (compatible packages from Core)
```

---

## 📊 Summary Table

| Status | Count | Packages |
|--------|-------|----------|
| ✅ Keep | 12 | Autofac, BarcodeLib, itext7, sqlite-net-pcl, SQLiteNetExtensions, AppCenter*, AspNet.WebApi, Spire.XLS, System.*, etc |
| ⚠️ Replace | 8 | Xamarin.Essentials, Lottie, Microcharts, GooglePlayServices.Location, ZXing.Net, Glide, Storm.PDFViewer, Firebase |
| ❌ Remove | 8+ | MvvmCross*, Xamarin.Android.Support.*, Xamarin.AndroidX.*, GooglePlayServices.*, Plugin.CurrentActivity, Karamunting.*, Mono.Android |

---

**Total:** 28+ packages analyzed  
**Action Required:** 16 packages  
**No Change:** 12 packages


