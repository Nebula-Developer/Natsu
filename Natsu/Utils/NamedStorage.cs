namespace Natsu.Utils;

public class NamedStorage<T> : IDisposable where T : class {
    private readonly Dictionary<string, T> _storage = new();

    public T? this[string name] {
        get => _storage.TryGetValue(name, out T? value) ? value : null;
        set => _storage[name] = value!;
    }

    public void Dispose() {
        foreach (T item in _storage.Values)
            if (item is IDisposable disposable)
                disposable.Dispose();

        _storage.Clear();
    }

    public void Add(string name, T value) => _storage[name] = value;

    public void Remove(string name) => _storage.Remove(name);

    public void Clear() => _storage.Clear();
}