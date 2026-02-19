# Action Plan - ISSUE-001

**Implementation Guide for Development Team**

**SCOPE:** Direct migration to tabApp.CrossPlatform  
**MIGRATE:** tabApp.Core + tabApp.Droid  
**PROTECTED:** tabApp.DroidClients, tabApp.DroidWear (NOT MIGRATED)

---

## 🎯 Development Tasks

### Task 1.1: Audit Dependencies (Day 1 - 2h)

**Deliverable:** `docs/tech/issue-001/DEPENDENCIES_MATRIX.md` ✅ DONE

**Steps:**
1. ✅ List all packages from original projects
2. ✅ Categorize each (Keep/Replace/Remove)
3. ✅ Identify MAUI alternatives
4. ✅ Document risks per package

---

### Task 1.2: Analyze Current Code (Day 1 - 3h)

**What to do:**
```bash
# 1. List all files to be migrated
find tabApp.Core -name "*.cs" | wc -l
find tabApp.Droid -name "*.cs" | wc -l
find tabApp.DroidClients -name "*.cs" | wc -l

# 2. Identify dependencies
dotnet list package --include-transitive > docs/tech/issue-001/CURRENT_DEPENDENCIES.txt

# 3. Document code structure
# - Number of ViewModels, Services, Models
# - Namespaces used
# - Key dependencies
```

**Deliverable:** Code structure documented, no changes to original projects

---

### Task 1.3: Create tabApp.CrossPlatform Core Structure (Day 2 - 4h)

**Create directories (if not exist):**
```
tabApp.CrossPlatform/
├── Models/              (copy from tabApp.Core/Models)
├── Services/
│   ├── Interfaces/      (copy from tabApp.Core/Services/Interfaces)
│   └── Implementations/ (copy from tabApp.Core/Services/Implementations)
├── ViewModels/          (copy from tabApp.Core/ViewModels)
├── Helpers/             (copy from tabApp.Core/Helpers + tabApp.Droid/Helpers)
├── Converters/          (copy from tabApp.Core/Converters)
├── Enums/               (copy from tabApp.Core/Enums)
├── UI/                  (copy from tabApp.Droid/UI)
└── Views/               (will be created in Phase 4)
```

**What to do:**
```bash
# 1. Create folder structure in tabApp.CrossPlatform
mkdir -p tabApp.CrossPlatform/Models
mkdir -p tabApp.CrossPlatform/Services/Interfaces
mkdir -p tabApp.CrossPlatform/Services/Implementations
mkdir -p tabApp.CrossPlatform/ViewModels
mkdir -p tabApp.CrossPlatform/Helpers
mkdir -p tabApp.CrossPlatform/Converters
mkdir -p tabApp.CrossPlatform/Enums
mkdir -p tabApp.CrossPlatform/UI

# 2. Copy files from tabApp.Core
cp -r tabApp.Core/Models/* tabApp.CrossPlatform/Models/
cp -r tabApp.Core/Services/Interfaces/* tabApp.CrossPlatform/Services/Interfaces/
cp -r tabApp.Core/Services/Implementations/* tabApp.CrossPlatform/Services/Implementations/
cp -r tabApp.Core/ViewModels/* tabApp.CrossPlatform/ViewModels/
cp -r tabApp.Core/Helpers/* tabApp.CrossPlatform/Helpers/
cp -r tabApp.Core/Converters/* tabApp.CrossPlatform/Converters/
cp -r tabApp.Core/Enums/* tabApp.CrossPlatform/Enums/

# 3. Copy files from tabApp.Droid
cp -r tabApp/Helpers/* tabApp.CrossPlatform/Helpers/Droid/
cp -r tabApp/UI/* tabApp.CrossPlatform/UI/
cp tabApp/MainApplication.cs tabApp.CrossPlatform/
cp tabApp/Setup.cs tabApp.CrossPlatform/

# 4. DO NOT COPY from tabApp.DroidClients or tabApp.DroidWear
```

**Deliverable:** All source files from Core + Droid copied to MAUI project

---

### Task 1.4: Update tabApp.CrossPlatform.csproj (Day 2 - 2h)

**File:** `tabApp.CrossPlatform/tabApp.CrossPlatform.csproj`

**Add these packages:**
```xml
<ItemGroup>
  <!-- Keep Compatible Packages -->
  <PackageReference Include="Autofac" Version="6.1.0" />
  <PackageReference Include="BarcodeLib" Version="2.4.0" />
  <PackageReference Include="itext7" Version="7.1.15" />
  <PackageReference Include="sqlite-net-pcl" Version="1.7.335" />
  <PackageReference Include="SQLiteNetExtensions" Version="2.1.0" />
  <PackageReference Include="Microsoft.AspNet.WebApi.Client" Version="5.2.7" />
  <PackageReference Include="Spire.XLS" Version="11.3.4" />
  <PackageReference Include="System.Formats.Asn1" Version="5.0.0" />
  <PackageReference Include="System.Security.Cryptography.Xml" Version="5.0.0" />
  <PackageReference Include="Microsoft.AppCenter.Analytics" Version="4.5.0" />
  <PackageReference Include="Microsoft.AppCenter.Crashes" Version="4.5.0" />
  <PackageReference Include="Microsoft.AppCenter.Distribute" Version="4.5.0" />

  <!-- MAUI Replacements -->
  <PackageReference Include="Microsoft.Maui.Essentials" Version="8.0.0" />
  <PackageReference Include="Microcharts.Maui" Version="0.10.0" />
  <PackageReference Include="FirebaseStorage.CSharp" Version="4.0.0" />

  <!-- MAUI UI Libraries -->
  <PackageReference Include="SkiaSharp.Extended.UI.Maui" Version="1.0.0" />
  <PackageReference Include="ZXing.Net.Maui" Version="1.0.0" />
  <PackageReference Include="FFImageLoading.Maui" Version="1.0.0" />
  <PackageReference Include="Microsoft.Maui.Controls.Maps" Version="8.0.0" />
</ItemGroup>
```

**Verification:**
```bash
cd tabApp.CrossPlatform
dotnet clean
dotnet restore
# Check all packages resolve
```

**Deliverable:** MAUI project configured with all dependencies

---

### Task 1.5: Update Namespace Imports (Day 3 - 6h)

**Update imports in copied files:**

```csharp
// In all copied .cs files, replace:

// OLD
using Xamarin.Essentials;
// NEW
using Microsoft.Maui.Essentials;

// OLD
using MvvmCross;
using MvvmCross.ViewModels;
// REMOVE (will be handled in ISSUE-002)

// OLD
using Com.Airbnb.Android.Lottie;
// WILL BE HANDLED IN: ISSUE-006 (UI migration)
// For now: MARK as TODO

// OLD
using Xamarin.Android.Support.*;
// REMOVE (Android specific)
```

**Use Find & Replace in Visual Studio:**
1. Ctrl+H (Find & Replace)
2. **Replace 1:**
   - Find: `using Xamarin.Essentials;`
   - Replace: `using Microsoft.Maui.Essentials;`
3. **Replace 2:**
   - Find: `using Com.Airbnb.Android.Lottie;`
   - Replace: `// TODO: Migrate Lottie in ISSUE-006`
4. **Delete:**
   - Find: `using MvvmCross` (all occurrences)
   - Delete (will be handled in ISSUE-002)

**Scope:** Only in tabApp.CrossPlatform folder

**Deliverable:** All imports updated, Android-specific removed

---

### Task 1.6: Remove Android-Specific Code (Day 3-4 - 6h)

**Remove from copied files:**
```csharp
// Android-specific using statements
using Android.*;
using Xamarin.Android.*;
using Xamarin.AndroidX.*;

// Android-specific code blocks
#if __ANDROID__
  // Android code
#endif

// MvvmCross specific code
using MvvmCross.Droid.*;
```

**Files most affected:**
- `Services/Implementations/Native/*` (Android native services)
- `Services/Implementations/CrossPlat/*` (cross-platform implementations)

**Action:**
```bash
# 1. Search for Android-specific files
grep -r "using Android" tabApp.CrossPlatform/Services/

# 2. Review each file and remove Android code
# 3. Keep only cross-platform implementations

# 4. Files to completely remove:
# - BluetoothManagerService.cs (Android-specific → ISSUE-004)
# - ForegroundService.cs (Android-specific → ISSUE-003)
```

**Deliverable:** No Android-specific code in MAUI project

---

### Task 1.7: Compilation Testing (Day 4-5 - 6h)

**Test 1: Initial Build**
```bash
cd tabApp.CrossPlatform
dotnet clean
dotnet restore
dotnet build
# Note all compilation errors
```

**Test 2: By Platform**
```bash
dotnet build -f net8.0-android -c Debug
dotnet build -f net8.0-ios -c Debug
dotnet build -f net8.0-windows10.0.19041.0 -c Debug
```

**Expected Errors:**
- MvvmCross references → Mark for ISSUE-002
- Android code blocks → Remove
- Lottie imports → Mark for ISSUE-006
- Services/Interfaces missing implementations → Mark for future issues

**Checklist:**
- [ ] No Android-specific compilation errors
- [ ] All cross-platform packages resolve
- [ ] Namespace imports correct
- [ ] No duplicate class definitions

**Deliverable:** Clean compilation (or documented blocking issues)

---

### Task 1.8: Create Missing Implementations Stubs (Day 5 - 4h)

**For Android-specific services removed, create stubs in MAUI:**

**Example - BluetoothService stub:**
```csharp
// tabApp.CrossPlatform/Services/Implementations/BluetoothService.cs
namespace tabApp.CrossPlatform.Services.Implementations
{
  public class BluetoothService : IBluetoothService
  {
    // TODO: ISSUE-004 - Implement Bluetooth service for MAUI
    // Temporary stub implementation
    
    public Task<bool> ConnectAsync(string deviceId)
    {
      throw new NotImplementedException("ISSUE-004: Bluetooth service");
    }
    
    // ... other interface methods
  }
}
```

**Files to create stubs for:**
- BluetoothService (ISSUE-004)
- GeolocationService (ISSUE-038)
- NotificationService (ISSUE-032)
- PermissionsService (ISSUE-017)

**Deliverable:** Stub implementations with TODO references

---

### Task 1.9: Documentation & Validation (Day 5 - 3h)

**Create:**
1. ✅ `docs/tech/issue-001/README.md`
2. ✅ `docs/tech/issue-001/DEPENDENCIES_MATRIX.md`
3. [ ] `docs/tech/issue-001/MIGRATION_REPORT.md`
4. [ ] `docs/tech/issue-001/BLOCKING_ISSUES.md`

**Migration Report Template:**
```markdown
# Migration Report - ISSUE-001

## Summary
- Files copied: X
- Files modified: Y
- Lines of code: Z
- Namespaces updated: N

## Status
- [x] Dependencies configured
- [x] Source files copied
- [x] Imports updated
- [x] Android code removed
- [x] Stub implementations created

## Blocking Issues
- ISSUE-002: MvvmCross removal
- ISSUE-004: Bluetooth service
- ISSUE-006: Lottie animations
- ISSUE-038: Geolocation service

## Next Steps
1. Resolve ISSUE-002 (architecture)
2. Implement platform services
3. Create Views (ISSUE-060+)
```

**Deliverable:** Complete documentation of migration

---

## ✅ Validation Checklist

Before marking ISSUE-001 as DONE:

**Code Quality:**
- [ ] No compilation errors
- [ ] No compilation warnings
- [ ] All packages referenced
- [ ] No duplicate package references
- [ ] Correct versions specified

**Compatibility:**
- [ ] Core compiles in .NET 8.0
- [ ] Android builds successfully
- [ ] iOS builds successfully
- [ ] Windows builds successfully

**Documentation:**
- [ ] README.md created
- [ ] DEPENDENCIES_MATRIX.md complete
- [ ] BREAKING_CHANGES.md documented
- [ ] VALIDATION_REPORT.md filled

**Team Sign-off:**
- [ ] Code reviewed by TechLead
- [ ] PM approved migration strategy
- [ ] Dev team confirmed understanding

---

## 🚨 If Compilation Fails

**Common Issues & Solutions:**

### Issue: "Cannot resolve NuGet package"
```
Solution:
1. Delete bin/ and obj/ folders
2. dotnet clean
3. dotnet restore --no-cache
4. dotnet build
```

### Issue: "Type or namespace not found"
```
Solution:
1. Check using statements updated
2. Run dotnet clean && dotnet restore
3. Rebuild IntelliSense: Ctrl+Alt+Z
```

### Issue: "Multiple versions of same package"
```
Solution:
1. Check .csproj for duplicate references
2. Check for transitive dependencies
3. Use: dotnet list package --include-transitive
```

### Issue: "Android/iOS-specific errors"
```
Solution:
1. Ensure package supports target framework
2. Check package is not Android-only
3. Review platform-specific conditional compilation
```

---

## 📞 Questions & Escalation

**If stuck:**
1. Check `DEPENDENCIES_MATRIX.md` for package info
2. Review `BREAKING_CHANGES.md` for migration guide
3. Contact TechLead Agent
4. Check official NuGet package documentation

---

## 📈 Time Estimate

| Task | Duration | Days |
|------|----------|------|
| 1.1 - Audit | 2h | 1 |
| 1.2 - Backup | 1h | 1 |
| 1.3 - Remove Android | 4h | 2 |
| 1.4 - Update Framework | 2h | 2 |
| 1.5 - Add MAUI Packages | 4h | 3 |
| 1.6 - Update Imports | 6h | 3-4 |
| 1.7 - Update MAUI Project | 3h | 4 |
| 1.8 - Testing | 4h | 5 |
| 1.9 - Documentation | 2h | 5 |
| **TOTAL** | **28h** | **5 days** |

---

## 🎯 Success Criteria

✅ All packages audited  
✅ Compatibility matrix created  
✅ Core compiles in .NET 8.0  
✅ MAUI compiles for all platforms  
✅ No breaking changes in critical paths  
✅ Documentation complete  
✅ Team approved  

---

**Status:** Ready for Development  
**Next Issue:** ISSUE-002 (Architecture Assessment)





