namespace Natsu.Utils;

public class FallbackDictionary<TKey, TValue> where TKey : notnull {
    private readonly object _lock = new();

    public FallbackDictionary(TValue fallback) => Fallback = fallback;

    public FallbackDictionary(TValue fallback, Dictionary<TKey, TValue> primary) {
        Fallback = fallback;
        Primary = primary;
    }

    public Dictionary<TKey, TValue> Primary { get; } = new();
    public TValue Fallback { get; }

    public TValue this[TKey key] {
        get {
            lock (_lock) {
                return Primary.TryGetValue(key, out TValue? value) ? value : Fallback;
            }
        }
        set {
            lock (_lock) {
                Primary[key] = value;
            }
        }
    }

    public bool ContainsKey(TKey key) {
        lock (_lock) {
            return Primary.ContainsKey(key);
        }
    }

    public bool Remove(TKey key) {
        lock (_lock) {
            return Primary.Remove(key);
        }
    }

    public void Clear() {
        lock (_lock) {
            Primary.Clear();
        }
    }

    public void Add(TKey key, TValue value) {
        lock (_lock) {
            Primary.Add(key, value);
        }
    }
}
