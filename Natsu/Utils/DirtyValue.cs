namespace Natsu.Utils;

public class DirtyValue<T> where T : class {
    public T? Value { get; private set; }
    public bool IsDirty { get; private set; }

    public void Invalidate() {
        IsDirty = true;
        if (Value is IDisposable disposable) disposable.Dispose();

        Value = null;
    }

    public T Validate(T t) {
        Value = t;
        IsDirty = false;
        return t;
    }
}
