# 🚗 Vehicle Tablet UX Guidelines
## Critical Design Requirements for In-Vehicle Use

**Application Context:** Tablet-based fleet management system mounted horizontally in vehicle cabs with real-time driver interaction while vehicle is moving.

**Date:** 2026-02-20  
**Version:** 1.0  
**Audience:** UI/UX Designers, Frontend Developers, Product Managers

---

## 🚨 Safety & Usability Principles

### PRIMARY RULE: Driver Distraction Prevention
**The app must not distract the driver or require attention while driving.**

Every feature, button, animation, and interaction must pass this test:
> "Can a driver use this safely while maintaining full attention on the road?"

If the answer is NO → feature needs redesign.

---

## 📐 1. Layout & Orientation Guidelines

### Landscape-Only Orientation ✅
```
Android: AndroidManifest.xml
└─ android:screenOrientation="landscape"

iOS: Info.plist
└─ UISupportedInterfaceOrientations = (
     UIInterfaceOrientationLandscapeLeft,
     UIInterfaceOrientationLandscapeRight
   )
```

**Why:** Tablets mounted horizontally in dashboards. Rotation = unsafe.

### Safe Area Margins
```
Top:    40dp  (below status bar)
Bottom: 40dp  (above car controls)
Left:   40dp  (left side of cab)
Right:  40dp  (right side of cab)
```

**Why:** Drivers reach for dashboard controls. Don't obscure them.

---

## 👆 2. Touch Target Sizing (Automotive Standard)

### Minimum Button Size
```
48dp × 48dp (Apple Human Interface Guidelines = 44px)
Automotive Standard = 48dp
```

### Button Spacing
```
Horizontal spacing: ≥ 8dp between buttons
Vertical spacing:   ≥ 8dp between buttons
```

**Why:** Vehicles vibrate. Large targets = fewer accidental taps.

### Landscape Layout Example
```
┌─────────────────────────────────────┐
│  STATUS: Online    Battery: 87%     │  ← Header (18sp)
├─────────────────────────────────────┤
│                                     │
│  ┌──────────────────┐  ┌──────────┐ │
│  │  Current Location│  │  Orders  │ │  ← Large buttons
│  │                  │  │  (3)     │ │     48dp × 48dp+
│  └──────────────────┘  └──────────┘ │
│                                     │
│  ┌──────────────────┐  ┌──────────┐ │
│  │ Tracking:  ON    │  │  Alerts  │ │
│  │ GPS: Strong      │  │  (1)     │ │
│  └──────────────────┘  └──────────┘ │
│                                     │
│  [EMERGENCY STOP] - Always visible  │
└─────────────────────────────────────┘
```

---

## 🎨 3. Visual Design for Glanceability

### Font Sizes (Minimum)
```
Status/Alerts:  24sp - 32sp (LARGE, visible at a glance)
Body Text:      16sp - 18sp (readable at arm's length)
Labels:         14sp minimum (no smaller)
```

**Why:** Driver glances at screen for 1-2 seconds max. Tiny fonts = unreadable.

### Color & Contrast
```
✅ Use WCAG AAA standards (minimum 7:1 contrast ratio)
✅ Test colors in bright sunlight (window reflection)
✅ Use distinct colors for states:
   - Green:  Active, Good
   - Yellow: Warning, Attention
   - Red:    Error, Stop
✅ Avoid color-only differentiation (red/green colorblind)
```

### No Animations or Flashing
```
❌ NO blinking text
❌ NO animations that move
❌ NO progress bars with movement
✅ Static text with occasional refresh
✅ State changes with clear visual feedback
```

**Why:** Moving elements = driver's eye follows = distraction.

---

## ⚡ 4. Minimal Interaction While Driving

### CRITICAL: Location Tracking Should Be AUTOMATIC
```
❌ BAD:   User taps "Start Tracking" → opens dialog → selects options
✅ GOOD: App auto-starts tracking on launch, shows status only
```

### Maximum User Actions
```
Glance-only (0 taps):
  - Status display
  - Current location
  - Proximity alerts

Single-tap actions:
  - Acknowledge alert
  - Accept order
  - Emergency stop

Multi-tap actions (≥2 taps):
  - Settings (should require parking first)
  - Advanced features
```

### NO Complex Workflows While Driving
```
❌ Multi-step dialogs
❌ Forms to fill out
❌ Menu navigation (more than 1 level deep)
❌ Text input
✅ One-tap acceptance
✅ One-tap dismissal
✅ Clear, unambiguous status display
```

---

## 🚨 5. Critical Alerts & Notifications

### Proximity Alerts (80m threshold)
```
MUST have:
  ✅ Loud audio alert (min 85dB)
  ✅ Haptic feedback (device vibration)
  ✅ Large visual alert (color change, icon)
  ✅ Persistent until acknowledged (1 tap to dismiss)
  
Display:
  "ORDER #1234 NEARBY"
  "Distance: 65 meters"
  
  [ACKNOWLEDGE] [DETAILS]
```

### Network/GPS Loss
```
When signal lost:
  ✅ Show "No Signal" clearly
  ✅ Continue operating (don't crash)
  ✅ Queue alerts for send when signal returns
  ✅ DON'T show error dialogs (driver can't read)
  ✅ DON'T restart location tracking
  
Display:
  "GPS: No Signal"
  "Status: Offline (Data will sync)"
```

### Battery Low Warning
```
At < 10% battery:
  ✅ Yellow alert in header
  ✅ No action required
  ✅ Don't disable features (let driver decide)

Display:
  "⚠️ Battery: 8% - Vehicle Power Recommended"
```

---

## 🔋 6. Power Management

### Battery Drain Optimization
```
Profile the app:
  - GPS + continuous polling = ~15% battery/hour
  - Screen on + brightness = ~10% battery/hour
  - Total: ~25% battery drain/hour in sunny conditions

Recommendations:
  ✅ Use vehicle 12V power ALWAYS
  ✅ Set screen brightness to maximum (sunlight)
  ✅ Disable screen sleep (≥10 min timeout)
  ✅ Background tasks: only when charging
  ✅ Alert driver if on battery < 2 hours capacity
```

### Screen Behavior
```
While driving:
  ✅ Screen NEVER sleeps
  ✅ Brightness ALWAYS maximum
  ✅ Keep location service running

While parked (optional):
  ✅ Screen can sleep after 5 min
  ✅ Reduce brightness
  ✅ Continue location background service
```

---

## 🗺️ 7. Background Location Service Specifics

### Auto-Start on App Launch
```csharp
// App.xaml.cs or MauiProgram.cs
protected override Window CreateWindow(IActivationState activationState)
{
    // Auto-start location tracking
    var tracker = ServiceProvider.GetService<IBackgroundLocationTracker>();
    _ = tracker.StartAsync(); // Fire-and-forget, don't await
    
    return new Window(new AppShell());
}
```

### Show Status Only
```
❌ DON'T ask user "Enable location?"
❌ DON'T show permission dialogs while driving
✅ DO show current location every 2 seconds
✅ DO show "Tracking: ON / OFF" status
✅ DO show GPS signal strength
```

### Location Update Frequency
```
Android (Foreground Service):
  ✅ 1-second polling = responsive proximity detection
  ✅ Trade-off: ~5-10% battery drain
  
iOS (CLLocationManager):
  ⚠️ Movement-triggered updates (OS limitation)
  ⚠️ Approximately every 500m or 15 min
  ✅ Much lower battery drain
  
DOCUMENT this asymmetry to users:
  "Location updates are more frequent on Android.
   Both platforms support real-time proximity alerts."
```

---

## 🌐 8. Network Resilience

### Handle Signal Loss Gracefully
```
When cellular signal lost:
  ✅ Continue running normally
  ✅ Queue notifications for send
  ✅ Show "Offline" badge in header
  ✅ Don't show error dialogs
  ✅ Auto-retry when signal returns

When GPS signal weak/lost:
  ✅ Show "GPS: Weak" or "GPS: No Signal"
  ✅ Continue with last-known location
  ✅ DON'T disable proximity alerts
  ✅ DON'T require user action
```

### Notification Queue
```
If notification fails to send (no signal):
  ✅ Queue in SQLite with timestamp
  ✅ Retry every 30 seconds when signal returns
  ✅ Show retry status in UI (not intrusive)
  ✅ After 24 hours, expire old queued items
  
Example queue entry:
  {
    id: UUID,
    type: "OrderProximity",
    orderId: 1234,
    timestamp: 2026-02-20T14:35:00,
    status: "queued",
    retryCount: 0
  }
```

---

## 🔐 9. Security & Permissions

### Runtime Permissions
```
❌ DON'T ask for permissions while driving
✅ DO ask once on first app launch (vehicle parked)
✅ DO handle denied permissions gracefully
✅ DO show explanation: "Location required for proximity alerts"
```

### Permission Dialog Example
```
Device: Location Permission Required

"tabApp needs your location to:
 • Detect nearby orders
 • Show current vehicle position
 
This data is only used while the app is running.

[ALLOW] [NOT NOW]"

-- On denial: show permanent explanation --
"Location permission was denied.
 Enable in Settings > Apps > tabApp to use this feature."
```

---

## 📱 10. Landscape-Specific UI Components

### TabBar (if needed)
```
ANDROID:
  ✅ Bottom bar (landscape-optimized)
  ✅ Icon + text label
  ✅ 48dp height minimum
  
  [Location] [Orders] [History] [Settings]

MAUI:
  <ShellContent
      Title="Location"
      Icon="location.png"
      ContentTemplate="{DataTemplate pages:LocationPage}" />
```

### Header/Status Bar
```
┌─────────────────────────────────────────────────┐
│ STATUS: Online │ GPS: Strong │ Battery: 87% ⚡ │
│ Current: 40.7128, -74.0060 │ Tracking: ON     │
└─────────────────────────────────────────────────┘
```

### Keyboard Behavior
```
❌ NEVER auto-show keyboard
❌ NEVER auto-focus text input
✅ IF input needed: make explicit (tap field first)
✅ Use numeric keyboards for numeric input (no letters)
✅ Provide default values (driver shouldn't type)
```

---

## 🧪 11. Testing Checklist

### Landscape Rendering
```
[ ] All buttons visible in landscape orientation
[ ] No cut-off text
[ ] Status always visible (top-left or top-center)
[ ] Emergency actions always accessible
[ ] Safe areas respected (40dp margins)
```

### Glanceability (Real-World)
```
[ ] Test with actual tablet in vehicle
[ ] Bright sunlight (not office lighting)
[ ] Driver can read status in 1-2 second glance
[ ] Buttons are reachable while belted in
[ ] No distracting animations or movements
[ ] No color-only state differentiation
```

### Accessibility
```
[ ] WCAG AAA contrast (7:1 minimum)
[ ] Font sizes: min 14sp body, 18sp+ status
[ ] Touch targets: 48dp minimum
[ ] No time limits on reading information
[ ] Haptic feedback enabled (not just audio)
```

### Distraction Testing
```
[ ] Can driver use app at 50 km/h without swerving?
[ ] Can driver use app at 100 km/h safely?
[ ] Does proximity alert interrupt driver appropriately?
[ ] Are multi-step workflows avoided?
[ ] Is information glanceable (≤2 seconds)?
```

---

## 📋 12. Implementation Checklist for TASK-3.10+

When implementing LocationTrackingPage and LocationTrackingViewModel:

### ViewModel (LocationTrackingViewModel.cs)
```csharp
[ ] Auto-start tracking in constructor or OnAppearing
[ ] NO manual "Start" button for end users
[ ] Show status: "Tracking: ON/OFF"
[ ] Show GPS signal strength
[ ] Show current location (update every 1-2 sec)
[ ] Show proximity alerts (large, clear)
[ ] Implement one-tap alert dismissal
[ ] Handle network loss gracefully
[ ] Handle GPS signal loss gracefully
[ ] Show battery warning at <10%
```

### Page (LocationTrackingPage.xaml)
```xaml
[ ] Landscape orientation only
[ ] 40dp safe area margins
[ ] Status bar (always visible, top)
[ ] Large location display (18sp+ font)
[ ] Large alert display (when triggered)
[ ] Emergency stop button (always visible)
[ ] High contrast colors (WCAG AAA)
[ ] NO animations or flashing
[ ] NO auto-keyboard
[ ] Touch targets ≥48dp
```

### Platform Configuration
```
Android:
[ ] AndroidManifest.xml: screenOrientation="landscape"
[ ] Permissions: ACCESS_FINE_LOCATION, ACCESS_BACKGROUND_LOCATION
[ ] Foreground service notification configured
[ ] Haptic feedback enabled
[ ] Audio alert configured (85dB minimum)

iOS:
[ ] Info.plist: Only landscape orientations
[ ] Info.plist: NSLocationAlwaysAndWhenInUseUsageDescription
[ ] Info.plist: UIBackgroundModes includes "location"
[ ] Haptic feedback enabled
[ ] Audio alert configured
```

---

## 🎯 Design Review Checklist (Pre-Implementation)

Before starting TASK-3.10.3 (ViewModel) & TASK-3.10.4 (Page):

```
[ ] Designer has reviewed all mockups
[ ] Mockups tested on actual tablet device
[ ] Landscape orientation confirmed
[ ] Font sizes validated (minimum 14sp body)
[ ] Touch targets verified (≥48dp)
[ ] Color contrast checked (WCAG AAA)
[ ] Animations removed or minimized
[ ] Emergency stop action clearly marked
[ ] Status information prioritized
[ ] No multi-step workflows
[ ] Accessibility requirements documented
[ ] Voice-over/screen reader support planned
[ ] All stakeholders approved mockups
```

---

## 📞 Questions for Product Owner

Before implementation, clarify:

1. **Auto-Start Location?**
   - Should tracking auto-start on app launch?
   - Or require user to tap "Start"?
   - → Recommendation: Auto-start (safest for vehicle use)

2. **Proximity Alert Behavior?**
   - Audio alert volume (default: 85dB)?
   - Haptic feedback strength?
   - Alert persistence (dismiss how)?
   - → Recommendation: Loud + haptic, 1-tap dismiss

3. **Parked vs. Driving Mode?**
   - Different UI for parked vehicle?
   - Disable features at speed > 5 km/h?
   - → Recommendation: Same UI always (safe at any speed)

4. **Display Driver Name/Vehicle ID?**
   - Show on header for accountability?
   - → Recommendation: Yes (helps fleet management)

5. **Settings Access?**
   - Require parking to access settings?
   - OR: Block settings while moving?
   - → Recommendation: Show "Parked Mode" indicator, lock settings while moving

---

## 📚 Reference Standards

- **Automotive UX:** ISO/IEC 26262 (Functional Safety for Vehicles)
- **Accessibility:** WCAG 2.1 AAA (Web Content Accessibility Guidelines)
- **Apple:** Human Interface Guidelines (iOS) - Landscape considerations
- **Android:** Material Design for Automotive
- **Safety:** NHTSA Driver Distraction Guidelines

---

**Document Version:** 1.0  
**Created:** 2026-02-20  
**For MAUI Implementation:** TASK-3.10.3 (ViewModel) & TASK-3.10.4 (Page)  
**Status:** Ready for Design Review & Implementation

