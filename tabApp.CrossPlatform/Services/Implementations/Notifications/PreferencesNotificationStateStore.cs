using tabApp.CrossPlatform.Services.Interfaces.Notifications;

namespace tabApp.CrossPlatform.Services.Implementations.Notifications;

/// <summary>
/// Thin key-value abstraction over MAUI Preferences.
/// Defined here (not in Interfaces/) intentionally — it is an implementation detail
/// of PreferencesNotificationStateStore, not a public contract.
/// In tests, replace with InMemoryKeyValueStore.
/// </summary>
public interface IKeyValueStore
{
    bool GetBool(string key, bool defaultValue = false);
    void SetBool(string key, bool value);
    string GetString(string key, string defaultValue = "");
    void SetString(string key, string value);
    void Remove(string key);
}

/// <summary>
/// MAUI Preferences-backed implementation of <see cref="IKeyValueStore"/>.
/// Production singleton — wraps <c>Preferences.Default</c>.
/// </summary>
public sealed class MauiKeyValueStore : IKeyValueStore
{
    public bool GetBool(string key, bool defaultValue = false)
        => Microsoft.Maui.Storage.Preferences.Default.Get(key, defaultValue);

    public void SetBool(string key, bool value)
        => Microsoft.Maui.Storage.Preferences.Default.Set(key, value);

    public string GetString(string key, string defaultValue = "")
        => Microsoft.Maui.Storage.Preferences.Default.Get(key, defaultValue);

    public void SetString(string key, string value)
        => Microsoft.Maui.Storage.Preferences.Default.Set(key, value);

    public void Remove(string key)
        => Microsoft.Maui.Storage.Preferences.Default.Remove(key);
}

/// <summary>
/// MAUI Preferences-backed implementation of <see cref="INotificationStateStore"/>.
///
/// POC rationale (from TASK-3.4 analysis):
///   MAUI Preferences is the simplest cross-platform persistent key-value store.
///   It requires zero setup and is sufficient for the daily deduplication window.
///   In production this can be swapped for a SQLite-backed store without touching callers.
///
/// Platform backing:
///   Android → SharedPreferences
///   iOS     → NSUserDefaults
///
/// Key format: "proximity_notif_{itemType}_{itemId}_{yyyy-MM-dd}"
/// Example:    "proximity_notif_Order_42_2026-02-20"
///
/// Expiry strategy: keys carry the date in their name. <see cref="ClearExpired"/>
/// enumerates via a companion index key and removes entries whose date is before today.
/// </summary>
public sealed class PreferencesNotificationStateStore : INotificationStateStore
{
    internal const string KeyPrefix      = "proximity_notif_";
    internal const string IndexKey       = "proximity_notif_index";
    internal const string IndexSeparator = "|";

    private readonly IKeyValueStore _store;

    /// <summary>Production constructor — uses MAUI Preferences via MauiKeyValueStore.</summary>
    public PreferencesNotificationStateStore() : this(new MauiKeyValueStore()) { }

    /// <summary>
    /// Testable constructor — accepts any <see cref="IKeyValueStore"/> implementation.
    /// </summary>
    public PreferencesNotificationStateStore(IKeyValueStore store)
    {
        _store = store ?? throw new ArgumentNullException(nameof(store));
    }

    // ── INotificationStateStore ────────────────────────────────────────────────

    /// <inheritdoc />
    public bool IsNotified(string key)
        => _store.GetBool(ToStorageKey(key));

    /// <inheritdoc />
    public void MarkNotified(string key)
    {
        var storageKey = ToStorageKey(key);
        _store.SetBool(storageKey, true);
        AddToIndex(storageKey);
    }

    /// <inheritdoc />
    public void ClearExpired()
    {
        var index = ReadIndex();
        var today = DateTime.Today;
        var toRemove = new List<string>();

        foreach (var storageKey in index)
        {
            if (TryExtractDate(storageKey, out var keyDate) && keyDate < today)
            {
                _store.Remove(storageKey);
                toRemove.Add(storageKey);
            }
        }

        if (toRemove.Count > 0)
            WriteIndex(index.Except(toRemove).ToList());
    }

    /// <inheritdoc />
    public void ClearAll()
    {
        foreach (var storageKey in ReadIndex())
            _store.Remove(storageKey);
        _store.Remove(IndexKey);
    }

    // ── Key helpers ────────────────────────────────────────────────────────────

    private static string ToStorageKey(string logicalKey) => KeyPrefix + logicalKey;

    internal static bool TryExtractDate(string storageKey, out DateTime date)
    {
        date = DateTime.MinValue;
        var parts = storageKey.Split('_');
        if (parts.Length < 2) return false;
        var datePart = parts[^1];
        return DateTime.TryParseExact(datePart, "yyyy-MM-dd",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None,
            out date);
    }

    // ── Index management ───────────────────────────────────────────────────────

    private List<string> ReadIndex()
    {
        var raw = _store.GetString(IndexKey);
        if (string.IsNullOrEmpty(raw)) return [];
        return [.. raw.Split(IndexSeparator, StringSplitOptions.RemoveEmptyEntries)];
    }

    private void WriteIndex(List<string> keys)
        => _store.SetString(IndexKey, string.Join(IndexSeparator, keys));

    private void AddToIndex(string storageKey)
    {
        var index = ReadIndex();
        if (!index.Contains(storageKey))
        {
            index.Add(storageKey);
            WriteIndex(index);
        }
    }
}




